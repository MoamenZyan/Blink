using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using BCrypt;
using Microsoft.EntityFrameworkCore;

public class AuthenticationController : Controller
{
    private readonly DatabaseContext _context;
    public AuthenticationController(DatabaseContext databaseContext)
    {
        _context = databaseContext;
    }

    [HttpPost("/api/v1/users/login")]
    public async Task<IActionResult> Login()
    {
        var body = await new FormReader(Request.Body).ReadFormAsync();
        if (body.ContainsKey("email") && body.ContainsKey("password"))
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.email == body["email"].FirstOrDefault());
            if (user is not null)
            {
                if (BCrypt.Net.BCrypt.EnhancedVerify(body["password"].FirstOrDefault(), user.password))
                {
                    var token = JwtService.GenerateToken(user.id);
                    Response.Headers["token"] = token;
                    Response.Cookies.Append("jwt", token,
                    new CookieOptions
                    {
                        Path="/",
                        Expires = DateTime.UtcNow.AddDays(30),
                        HttpOnly = true,
                    });
                    return Ok(new {status=true, message="user authenticated"});
                }
                else
                {
                    return Unauthorized(new {status=false, message="incorrect credentials"});
                }
            }
            else
            {
                return new JsonResult(new {status=false, message="user isn't found"}){StatusCode=404};
            }
        }
        else
        {
            return BadRequest(new {status=false, message="corrupted data"});
        }
    }
}
