using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Sprache;

public class UsersController : Controller
{
    private readonly DatabaseContext _context;
    public UsersController(DatabaseContext databaseContext)
    {
        _context = databaseContext;
    }

    [HttpPost("/api/v1/users")]
    public async Task<IActionResult> CreateUser()
    {
        var body = await new FormReader(Request.Body).ReadFormAsync();
        try
        {
            User user = new User
            {
                username = body["username"]!,
                first_name = body["first_name"]!,
                last_name = body["last_name"]!,
                password = BCrypt.Net.BCrypt.EnhancedHashPassword(body["password"], 10),
                email = body["email"]!,
                photo = null,
                verified = false,
                created_at = DateTime.Now
            };

            var result = await _context.Users.AddAsync(user);
            if (result.State == EntityState.Added)
            {
                var res = new {
                    username=user.username,
                    first_name=user.first_name,
                    last_name=user.last_name,
                    email=user.email,
                    created_at=user.created_at
                };
                await _context.SaveChangesAsync();
                return new JsonResult (new {status=true, message="user created successfully", user=res}){StatusCode=201};
            }
            else
            {
                return new JsonResult (new {status=true, message="error when creating user"}) {StatusCode=500};
            }
        }
        catch (Exception exp)
        {
            Console.WriteLine(exp);
            return new JsonResult(new {status=false, message="incorrect user info"}){StatusCode=400};
        }
    }

    [HttpGet("/api/v1/users")]
    public async Task<IActionResult> GetUsers([FromQuery] int id)
    {
        if (id == -1)
        {
            var users = await (from user in _context.Users
            select new
            {
                user.username,
                user.first_name,
                user.last_name,
                user.email,
                user.photo,
                user.created_at
            }).ToListAsync();
            return new JsonResult(new {status=true, users=users}){StatusCode=200};
        }
        else
        {
            User? user = await _context.Users.FirstOrDefaultAsync(u => u.id == id);
            if (user is not null)
                return new JsonResult(new {status=true, user=user}){StatusCode=200};
            else
                return new JsonResult(new {status=false, user=user}){StatusCode=404};
        }
    }


    [HttpPatch("/api/v1/users")]
    public async Task<IActionResult> UpdateUser([FromBody] Object json)
    {
        if (json is not null && Request.Cookies["jwt"] is not null)
        {
            var userId = JwtService.VerifyToken(Request.Cookies["jwt"]!);
            if (userId != default(int))
            {
                User? user = await _context.Users.FirstOrDefaultAsync(u => u.id == userId);
                if (user is not null)
                {
                    foreach (var field in JObject.Parse(json.ToString()!).Properties())
                    {
                        user.GetType().GetProperty(field.Name)?.SetValue(user, Convert.ToString(field.Value));
                    }
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                    var res = new {
                        photo=user.photo,
                        username=user.username,
                        first_name=user.first_name,
                        last_name=user.last_name,
                        email=user.email,
                        created_at=user.created_at
                    };
                    return new JsonResult(new {status=true, message="user updated successfully", user=res}){StatusCode=200};
                }
                else
                {
                    return new JsonResult(new {status=false, message="user isn't found"}){StatusCode=404};
                }
            }
            else
            {
                return new JsonResult(new {status=false, message="incorrect token"}){StatusCode=400};
            }
        }
        else
        {
            return Unauthorized();
        }
    }

    [HttpDelete("/api/v1/users")]
    public async Task<IActionResult> DeleteUser()
    {
        if (Request.Cookies["jwt"] is not null)
        {
            var userId = JwtService.VerifyToken(Request.Cookies["jwt"]!);
            if (userId != default(int))
            {
                User? user = await _context.Users.FirstOrDefaultAsync(u => u.id == userId);
                if (user is not null)
                {
                    var result = _context.Users.Remove(user);
                    if (result.State == EntityState.Deleted)
                    {
                        await _context.SaveChangesAsync();
                        return new JsonResult(new {status=true, message="user deleted successfully"}){StatusCode=200};
                    }
                    else
                    {
                        return new JsonResult(new {status=true, message="didn't save the change"}){StatusCode=500};
                    }
                }
                else
                {
                    return new JsonResult(new {status=false, message="user isn't found"}){StatusCode=404};
                }
            }
            else
            {
                return new JsonResult(new {status=false, message="incorrect token"}){StatusCode=400};
            }
        }
        else
        {
            return Unauthorized();
        }
    }
}
