using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

public class UsersController : Controller
{
    private readonly UserService _userService;
    private readonly FriendService _friendService;
    public UsersController(UserService userService, FriendService friendService)
    {
        _userService = userService;
        _friendService = friendService;
    }

    /// <summary>
    /// Get All Users
    /// </summary>
    /// <response code="200">Got Users</response>
    /// <response code="404">No Users Found</response>
    [HttpGet("/api/v1/users")]
    public async Task<IActionResult> GetUsers()
    {
        var token = Request.Cookies["jwt"];
        if (token is null)
            return Unauthorized();

        var users = await _userService.GetAllAsync(token);
        if (users!.Count > 0)
            return new JsonResult(new {status=true,users=users});
        else
            return new JsonResult(new {status=false, message="no users found"}){StatusCode=404};
    }

    [HttpGet("/api/v1/non-friends")]
    public async Task<IActionResult> GetNonFriendsUsers()
    {
        var token = Request.Cookies["jwt"];
        if (token is null)
            return Unauthorized();

        var users = await _userService.GetAllNonFriendsUsers(token);
        if (users!.Count > 0)
            return new JsonResult(new {status=true,users=users});
        else
            return new JsonResult(new {status=false, message="no users found"}){StatusCode=404};
    }

    [HttpGet("/api/v1/friends")]
    public async Task<IActionResult> GetFriendsUsers()
    {
        var token = Request.Cookies["jwt"];
        if (token is null)
            return Unauthorized();

        var users = await _userService.GetAllFriendsUsers(token);
        if (users!.Count > 0)
            return new JsonResult(new {status=true,users=users});
        else
            return new JsonResult(new {status=false, message="no users found"}){StatusCode=404};
    }
    /// <summary>
    /// Get Specific User By Id
    /// </summary>
    /// <response code="200">User Found</response>
    /// <response code="404">No User Found With That Id</response>
    [HttpGet("/api/v1/user/{username}")]
    public IActionResult GetUserByName(string username)
    {
        var token = Request.Cookies["jwt"];
        if (token is null)
            return Unauthorized();

        var user = _userService.GetByUsername(token, username);
        if (user is not null)
            return new JsonResult(new {status=true,user=user});
        else
            return new JsonResult(new {status=false, message=$"no user found with username {username}"}){StatusCode=404};
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

    [HttpPost("/api/v1/user-photo")]
    public async Task<IActionResult> UploadPhoto(IFormFile photo)
    {
        if (photo is null || photo.Length == 0)
            return BadRequest("No photo uploaded");

        var userId = JwtService.VerifyToken(Convert.ToString(Request.Cookies["jwt"])!);
        if (userId is not default(int))
        {
            var result = await _userService.UploadProfilePhoto(photo, userId);
            if (result is false)
                return new JsonResult(new {status=false,message="error in uploading photo"}){StatusCode=500};
            else
                return Ok("photo uploaded successfully");
        }
        else
        {
            return new JsonResult(new {status=false,message="corrupted token"}){StatusCode=422};
        }
    }

    [HttpPost("/api/v1/user-banner")]
    public async Task<IActionResult> UploadBanner(IFormFile photo)
    {
        if (photo is null || photo.Length == 0)
            return BadRequest("No banner uploaded");

        var userId = JwtService.VerifyToken(Convert.ToString(Request.Cookies["jwt"])!);
        if (userId is not default(int))
        {
            var result = await _userService.UploadProfileBanner(photo, userId);
            if (result is false)
                return new JsonResult(new {status=false,message="error in uploading banner"}){StatusCode=500};
            else
                return Ok("banner uploaded successfully");
        }
        else
        {
            return new JsonResult(new {status=false,message="corrupted token"}){StatusCode=422};
        }
    }

    [HttpGet("/api/v1/user/logout")]
    public IActionResult Logout()
    {
        try
        {
            Response.Cookies.Append("jwt", "", new CookieOptions
            {
                Path = "/",
                Expires = DateTime.UtcNow.AddDays(-1),
                SameSite = SameSiteMode.Lax,
                HttpOnly = true
            });
            return Ok("logged out successfully");
        }
        catch (Exception exp)
        {
            Console.WriteLine(exp);
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpPost("/api/v1/user-about")]
    public async Task<IActionResult> UpdateAbout()
    {
        var UserId = JwtService.VerifyToken(Request.Cookies["jwt"]!);
        if (UserId == default(int))
            return new JsonResult(new {status=false,message="corrupted token"}){StatusCode=422};
        var body = await new FormReader(Request.Body).ReadFormAsync();
        if (body is null)
            return BadRequest("body is null");
        var result = await _userService.UpdateAbout(UserId, body);
        if (result is true)
            return Ok("about updated");
        else
            return StatusCode(500, "internal server error");
    }

    [HttpPatch("/api/v1/users")]
    public async Task<IActionResult> UpdateUser()
    {
        var token = Request.Cookies["jwt"];
        if (token is null)
            return Unauthorized();

        var body = await new FormReader(Request.Body).ReadFormAsync();
        if (body is null)
            return BadRequest(new {status = false, message = "body is null"});
        
        var result = await _userService.UpdateUser(token, body);
        if (result is true)
            return Ok();
        else
            return BadRequest();
    }

    [HttpGet("/api/v1/user/{id}/send-request")]
    public async Task<IActionResult> SendFriendRequest(int id)
    {
        var token = Request.Cookies["jwt"];
        if (token is null)
            return Unauthorized();
        
        var result = await _friendService.SendRequest(token, id);
        if (result)
            return Ok();
        else
            return StatusCode(500, new {status=false, message="error in sending request"});
    }

    [HttpGet("/api/v1/user/{id}/accept-request")]
    public async Task<IActionResult> AcceptFriendRequest(int id)
    {
        var token = Request.Cookies["jwt"];
        if (token is null)
            return Unauthorized();
        
        var result = await _friendService.AcceptRequest(token, id);
        if (result)
            return Ok();
        else
            return StatusCode(500, new {status=false, message="error in accepting request"});
    }

    [HttpDelete("/api/v1/user/{id}/delete-friend")]
    public async Task<IActionResult> DeleteFriend(int id)
    {
        var token = Request.Cookies["jwt"];
        if (token is null)
            return Unauthorized();
        
        var result = await _friendService.DeleteFriend(token, id);
        if (result)
            return Ok();
        else
            return StatusCode(500, new {status=false, message="error in deleting request"});
    }

    [HttpDelete("/api/v1/user/{id}/reject-friend")]
    public async Task<IActionResult> RejectFriendRequest(int id)
    {
        var token = Request.Cookies["jwt"];
        if (token is null)
            return Unauthorized();
        
        var result = await _friendService.RejectFriendRequest(token, id);
        if (result)
            return Ok();
        else
            return StatusCode(500, new {status=false, message="error in rejecting request"});
    }
}
