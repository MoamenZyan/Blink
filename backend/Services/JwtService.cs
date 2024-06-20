using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

public class JwtService
{
    // Creating JWT token
    public static string GenerateToken(int UserId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(DotNetEnv.Env.GetString("JWT_SECRET_KEY"));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, Convert.ToString(UserId))
            }),
            Expires = DateTime.UtcNow.AddDays(30),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }


    // To verify and get token payload (user id)
    public static int VerifyToken(string token)
    {
        if (token is not null)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(DotNetEnv.Env.GetString("JWT_SECRET_KEY"));
            var userId = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            }, out _).FindFirst(ClaimTypes.NameIdentifier)!.Value;
            if (userId is not null)
                return Convert.ToInt32(userId);
            else
                return default;
        }
        else
        {
            return default;
        }
    }
}
