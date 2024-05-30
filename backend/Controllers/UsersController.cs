using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

public class UsersController : Controller
{
    private readonly UserService _userService;
    public UsersController(UserService userService)
    {
        _userService = userService;
    }


    /// <summary>
    /// Create User
    /// </summary>
    /// <response code="200">User Created</response>
    /// <response code="422">One of user's info is incorrect / body is null</response>
    [HttpPost("/api/v1/users")]
    public async Task<IActionResult> CreateUser()
    {
        var body = await new FormReader(Request.Body).ReadFormAsync();
        if (body is not null)
        {
            var result = await _userService.AddUserAsync(body);
            if (result.Item1 is not null)
                return new JsonResult(new {status=true, message="user created successfully",user=result.Item1}){StatusCode=201};
            else
                return new JsonResult(new {status=false, message=result.Item2}){StatusCode=400};
        }
        else
        {
            return new JsonResult(new {status=false, message="body is null"}){StatusCode=422};
        }
    }

    /// <summary>
    /// Get All Users
    /// </summary>
    /// <response code="200">Got Users</response>
    /// <response code="404">No Users Found</response>
    [HttpGet("/api/v1/users")]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userService.GetAllAsync();
        if (users is not null)
            return new JsonResult(new {status=true,users=users});
        else
            return new JsonResult(new {status=false, message="no users found"}){StatusCode=404};
    }

    /// <summary>
    /// Get Specific User By Id
    /// </summary>
    /// <response code="200">User Found</response>
    /// <response code="404">No User Found With That Id</response>
    [HttpGet("/api/v1/user/{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user is not null)
            return new JsonResult(new {status=true,user=user});
        else
            return new JsonResult(new {status=false, message=$"no user found with id {id}"}){StatusCode=404};
    }

    /// <summary>
    /// Delete User
    /// </summary>
    /// <remarks>authentication and authorization from cookie</remarks>
    /// <response code="200">User Deleted</response>
    /// <response code="404">No User Found With That Id</response>
    [HttpDelete("/api/v1/users")]
    public async Task<IActionResult> DeleteUser()
    {
        var userId = JwtService.VerifyToken(Convert.ToString(Request.Headers["jwt"]));
        if (userId is not default(int))
        {
            var result = await _userService.Delete(userId, Convert.ToString(Request.Headers["jwt"]));
            if (result)
                return new JsonResult(new {status=true,message="user deleted successfully"});
            else
                return new JsonResult(new {status=false,message="can't delete user"}){StatusCode=401};
        }
        else
        {
            return new JsonResult(new {status=false,message="corrupted token"}){StatusCode=422};
        }
    }
}
