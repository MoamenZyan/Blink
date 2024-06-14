using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using BCrypt;
using Microsoft.EntityFrameworkCore;

public class AuthenticationController : Controller
{
    private readonly UserService _userService;
    public AuthenticationController(UserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Check user's credentials
    /// </summary>
    /// <remarks>
    /// inputs:-
    /// username,
    /// password,
    /// </remarks>
    /// <response code="200">User Authenticated and JWT session created</response>
    /// <response code="401">Unauthorized User</response>
    /// <response code="422">Body is null</response>
    /// <response code="500">internal server error</response>
    [HttpPost("/api/v1/login")]
    public async Task<IActionResult> Login()
    {
        try
        {
            var body = await new FormReader(Request.Body).ReadFormAsync();
            if (body is not null)
            {
                (bool, int) result = _userService.UserLogin(Convert.ToString(body["Username"]), Convert.ToString(body["Password"]));
                if (result.Item1)
                {
                    var token = JwtService.GenerateToken(result.Item2);
                    if (token is null)
                        return new JsonResult(new {status=false,message="internal server error"}){StatusCode=500};

                    Response.Cookies.Append("jwt", token,
                    new CookieOptions
                    {
                        SameSite = SameSiteMode.Lax,
                        Path="/",
                        Expires=DateTime.UtcNow.AddDays(30),
                        HttpOnly=false,
                    });
                    return new JsonResult(new {status=true,message="user authorized",Id=result.Item2}){StatusCode=200};
                }
                else
                {
                    return new JsonResult(new {status=false,message="user unauthorized"}){StatusCode=401};
                }
            }
            else
            {
                return new JsonResult(new {status=false,message="body is null"}){StatusCode=422};
            }
        }
        catch(Exception exp)
        {
            Console.WriteLine(exp);
            return new JsonResult(new {status=false,message="internal server error"}){StatusCode=500};
        }
    }
}
