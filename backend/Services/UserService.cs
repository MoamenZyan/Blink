using System.Text.RegularExpressions;
using Ganss.Xss;

public class UserService
{
    private readonly IRepository<User> _userRepository;
    private readonly HtmlSanitizer sanitizer = new HtmlSanitizer();
    public UserService(IRepository<User> repository)
    {
        _userRepository = repository;
    }

    /// <summary>
    /// Do validation before adding user to database
    /// </summary>
    /// <param name="body">dictionary has user information</param>
    /// <returns>User that created and message</returns>
    public async Task<(UserDto?, string)> AddUserAsync(Dictionary<string, Microsoft.Extensions.Primitives.StringValues> body)
    {
        switch (CheckUserInfo(body))
        {
            case UserInfoValidator.Validated:
                {
                    User user = new User
                    {
                        username = sanitizer.Sanitize(body["username"]!),
                        first_name = sanitizer.Sanitize(body["first_name"]!),
                        last_name = sanitizer.Sanitize(body["last_name"]!),
                        password = BCrypt.Net.BCrypt.EnhancedHashPassword(sanitizer.Sanitize(body["password"]!), 10),
                        email = sanitizer.Sanitize(body["email"]!),
                        photo = "null",
                        verified = false,
                        created_at = DateTime.Now
                    };
                    var result = await _userRepository.AddAsync(user);
                    if (result is not null)
                        return (new UserDto(result), "");
                    else
                        return (null, "");
                }
            case UserInfoValidator.UsernameError:
                return (null, "username has error");
            case UserInfoValidator.FirstnameError:
                return (null, "first_name has error");
            case UserInfoValidator.LastnameError:
                return (null, "last_name has error");
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
    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        var result = await _userRepository.GetByIdAsync(id);
        if (result is not null)
            return new UserDto(result);
        else
            return null;
    }

    /// <summary>
    /// Get all users from database
    /// </summary>
    /// <returns>if found returns users, null otherwise</returns>
    public async Task<List<UserDto>?> GetAllAsync()
    {
        List<User>? users = await _userRepository.GetAllAsync();
        if (users is not null)
        {
            List<UserDto> userDtos = new List<UserDto>();
            foreach(User user in users)
                userDtos.Add(new UserDto(user));
            return userDtos;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Filter method to filter user's by specific condition
    /// </summary>
    /// <param name="func">delegate of condition</param>
    /// <returns>if there any metch it returns it, null otherwise</returns>
    public async Task<List<UserDto>?> Filter(Func<User, bool> func)
    {
        IEnumerable<User>? users = await _userRepository.Filter(func);
        if (users is not null)
        {
            List<UserDto> userDtos = new List<UserDto>();
            foreach(User user in users)
                userDtos.Add(new UserDto(user));
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
    public async Task<(bool, int)> UserLogin(string username, string password)
    {
        if (username is not null && password is not null)
        {
            var result = await _userRepository.Filter((user) => user.username == username);
            if (result is not null)
            {
                var user = result.First();
                return (BCrypt.Net.BCrypt.EnhancedVerify(password, user.password), user.id);
            }
            else
                return (false, default);
        }
        else
        {
            return (false, default);
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
            if (Convert.ToString(body["username"]) is null)
                return UserInfoValidator.UsernameError;

            if (Convert.ToString(body["first_name"]) is null)
                return UserInfoValidator.FirstnameError;

            if (Convert.ToString(body["last_name"]) is null)
                return UserInfoValidator.LastnameError;

            // password should contains:-
            // at least one lowercase letter
            // at least one uppercase letter
            // at least one digit
            // Has a minimum length of 8 characters
            if (Convert.ToString(body["password"]) is null ||
            !Regex.IsMatch(Convert.ToString(body["password"]), @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$"))
                return UserInfoValidator.PasswordError;

            if (Convert.ToString(body["email"]) is null
            || !Regex.IsMatch(Convert.ToString(body["email"]), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
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
