using System.Text.RegularExpressions;
using Ganss.Xss;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

public class UserService
{
    private readonly IRepository<User> _userRepository;
    private readonly IRedisCache _redis;
    private readonly HtmlSanitizer sanitizer = new HtmlSanitizer();
    public UserService(IRepository<User> repository, IRedisCache redis)
    {
        _userRepository = repository;
        _redis = redis;
    }

    /// <summary>
    /// Do validation before adding user to database
    /// </summary>
    /// <param name="body">dictionary has user information</param>
    /// <returns>User that created and message</returns>
    public async Task<(UserDto?, string)> AddUserAsync(Dictionary<string, StringValues> body)
    {
        switch (CheckUserInfo(body))
        {
            case UserInfoValidator.Validated:
                {
                    User user = new User
                    {
                        Username = sanitizer.Sanitize(body["Username"]!),
                        FirstName = sanitizer.Sanitize(body["FirstName"]!),
                        LastName = sanitizer.Sanitize(body["LastName"]!),
                        Password = BCrypt.Net.BCrypt.EnhancedHashPassword(sanitizer.Sanitize(body["Password"]!), 10),
                        Email = sanitizer.Sanitize(body["Email"]!),
                        Photo = "null",
                        Verified = false,
                        CreatedAt = DateTime.Now,
                        Privacy = "public"
                    };
                    var result = await _userRepository.AddAsync(user);
                    if (result is not null)
                    {
                        // Updating cache
                        var usersFromRedis = _redis.Get("users");
                        if (usersFromRedis is null)
                        {
                            var usersFromDb = await _userRepository.GetAllAsync();
                            _redis.Set("users", JsonConvert.SerializeObject(usersFromDb, JsonSettings.DefaultSettings), new TimeSpan(1, 0, 0));
                        }
                        else
                        {
                            List<User> users = JsonConvert.DeserializeObject<List<User>>(usersFromRedis)!.ToList();
                            users.Add(user);
                            _redis.Set("users", JsonConvert.SerializeObject(users, JsonSettings.DefaultSettings), new TimeSpan(1, 0, 0));
                        }
                        return (new UserDto(result), "");
                    }
                    else
                    {
                        return (null, "");
                    }
                }
            case UserInfoValidator.UsernameError:
                return (null, "username has error");
            case UserInfoValidator.FirstnameError:
                return (null, "FirstName has error");
            case UserInfoValidator.LastnameError:
                return (null, "LastName has error");
            case UserInfoValidator.PasswordError:
                return (null, "password has error");
            case UserInfoValidator.EmailError:
                return (null, "email has error");
        }
        return (null, "");
    }

    /// <summary>
    /// Get specific user from database
    /// </summary>
    /// <param name="id">Id of the user</param>
    /// <returns>If found returns user, null otherwise</returns>
    public async Task<UserFullDto?> GetUserByIdAsync(int id)
    {
        // Get user from redis
        var user = _redis.Get($"user?id={id}");
        if (user is null)
        {
            var result = await _userRepository.GetByIdAsync(id);
            if (result is null)
                return null;
            // Set user in redis
            _redis.Set($"user?id={id}", JsonConvert.SerializeObject(result, JsonSettings.DefaultSettings), new TimeSpan(1, 0, 0));
            return new UserFullDto(result);
        }
        User userFromRedis = JsonConvert.DeserializeObject<User>(user)!;
        return new UserFullDto(userFromRedis);
    }

    /// <summary>
    /// Get all users from database
    /// </summary>
    /// <returns>if found returns users, null otherwise</returns>
    public async Task<List<UserDto>?> GetAllAsync()
    {
        // Get users from redis
        var json = _redis.Get("users");
        if (json is null)
        {
            List<User>? users = await _userRepository.GetAllAsync();
            if (users is null)
                return null;
            // Set users in redis
            _redis.Set("users", JsonConvert.SerializeObject(users, JsonSettings.DefaultSettings), new TimeSpan(1, 0, 0));
            List<UserDto> userDtos = users.Select(u => new UserDto(u)).ToList();
            return userDtos;
        }
        List<User> usersFromRedis = JsonConvert.DeserializeObject<List<User>>(json)!;
        return usersFromRedis.Select(u => new UserDto(u)).ToList();
    }

    /// <summary>
    /// Filter method to filter user's by specific condition
    /// </summary>
    /// <param name="func">delegate of condition</param>
    /// <returns>if there any metch it returns it, null otherwise</returns>
    public List<UserDto>? Filter(Func<User, bool> func)
    {
        List<User>? users = _userRepository.Filter(func)?.ToList();
        if (users is not null)
        {
            List<UserDto> userDtos = users.Select(u => new UserDto(u)).ToList();
            return userDtos;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// It delete user from database
    /// </summary>
    /// <param name="id">id of the user</param>
    /// <param name="token">token of the signed in user (more secure)</param>
    /// <returns>returns true if successfull, false otherwise</returns>
    public async Task<bool> Delete(int id, string token)
    {
        var userId = JwtService.VerifyToken(token);
        // Checking to see if logged in user trying to delete himself
        if (userId is not default(int) && userId == id)
        {
            User? user = await _userRepository.GetByIdAsync(id);
            if (user is not null)
            {
                await _userRepository.Delete(user);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// make jwt and cookie to make user log in
    /// </summary>
    /// <param name="username">User's username</param>
    /// <param name="password">User's password</param>
    /// <returns>return true and user's id if authenticated, false and default otherwise</returns>
    public (bool, int) UserLogin(string username, string password)
    {
        if (username is not null && password is not null)
        {
            var result = _userRepository.Filter((user) => user.Username == username);
            var user = result?.FirstOrDefault();
            if (user is not null)
                return (BCrypt.Net.BCrypt.EnhancedVerify(password, user.Password), user.Id);
            else
                return (false, default);
        }
        else
        {
            return (false, default);
        }
    }

    public async Task<bool> UpdateUser(int id, JObject json)
    {
        User? user = await _userRepository.GetByIdAsync(id);
        if (user is not null)
        {
            try
            {
                foreach (var property in json.Properties())
                {
                    var userProperty = user.GetType().GetProperty(property.Name);
                    if (userProperty is not null)
                    {
                        var convertedValue = property.Value.ToObject(userProperty.PropertyType);
                        userProperty.SetValue(user, convertedValue);
                    }
                }
                return await _userRepository.Update(user);
            }
            catch(Exception exp)
            {
                Console.WriteLine(exp);
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Validate user's information
    /// </summary>
    /// <param name="body">Dictionary has user's info</param>
    /// <returns>returns enum based on user's information</returns>
    private UserInfoValidator CheckUserInfo(Dictionary<string, Microsoft.Extensions.Primitives.StringValues> body)
    {
        if (body is not null)
        {
            if (Convert.ToString(body["Username"]) is null)
                return UserInfoValidator.UsernameError;

            if (Convert.ToString(body["FirstName"]) is null)
                return UserInfoValidator.FirstnameError;

            if (Convert.ToString(body["LastName"]) is null)
                return UserInfoValidator.LastnameError;

            // password should contains:-
            // at least one lowercase letter
            // at least one uppercase letter
            // at least one digit
            // Has a minimum length of 8 characters
            if (Convert.ToString(body["Password"]) is null ||
            !Regex.IsMatch(Convert.ToString(body["Password"]), @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$"))
                return UserInfoValidator.PasswordError;

            if (Convert.ToString(body["Email"]) is null
            || !Regex.IsMatch(Convert.ToString(body["Email"]), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                return UserInfoValidator.EmailError;

            return UserInfoValidator.Validated;
        }
        return UserInfoValidator.Error;
    }
}

public enum UserInfoValidator
{
    UsernameError,
    FirstnameError,
    LastnameError,
    PasswordError,
    EmailError,
    Validated,
    Error
}
