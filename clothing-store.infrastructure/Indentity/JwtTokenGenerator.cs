using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace clothing_store.infrastructure.Indentity
{
  public class JwtTokenGenerator
  {
    private readonly IConfiguration _config;

    public JwtTokenGenerator(IConfiguration config)
    {
      _config = config;
    }

    public string GenerateToken(ApplicationUser user)
    {
      var claims = new[]
      {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim("isAdmin", user.IsAdmin.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Unique ID for security
        };

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var token = new JwtSecurityToken(
          _config["Jwt:Issuer"],
          _config["Jwt:Audience"],
          claims,
          expires: DateTime.UtcNow.AddHours(3), // Token expires in 3 hours
          signingCredentials: creds
      );

      return new JwtSecurityTokenHandler().WriteToken(token);
    }
  }
}
