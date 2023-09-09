using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
namespace to_do_api.Controllers;
using to_do_api.DTOs;
using to_do_api.Models;
using System;

[ApiController]
[Route("[controller]")]
public class TokensController : ControllerBase
{

    private readonly ILogger<TokensController> _logger;

    public TokensController(ILogger<TokensController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public IActionResult Post([FromBody] DTOs.LoginDTO data)
    {
        var db = new ToDoDbContext();
        var users = db.Users.ToList();
        var user = null as Users;
        foreach (var tempUser in users)
        {
            if (tempUser.UserName == data.UserName)
            {
                user = tempUser;
                break;
            }
        }
        if (user == null)
        {
            return Unauthorized();
        }
        var hash = CreateHash(data.UserPassword, user.Salt);
        if (hash != user.UserPassword)
        {
            return Unauthorized();
        }
        var desc = new SecurityTokenDescriptor();
        desc.Subject = new ClaimsIdentity(
            new Claim[] {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Role, "user")
            }
        );
        desc.NotBefore = DateTime.UtcNow;
        desc.Expires = DateTime.UtcNow.AddHours(3);
        desc.IssuedAt = DateTime.UtcNow;
        desc.Issuer = "ToDoApp"; // any string is ok
        desc.Audience = "public"; // any string is ok
        desc.SigningCredentials = new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(Program.SecurityKey)
            ),
            SecurityAlgorithms.HmacSha256Signature
        );
        var handler = new JwtSecurityTokenHandler();
        var token = handler.CreateToken(desc);

        return Ok(new { token = handler.WriteToken(token) });
    }

    private string CreateHash(string userPassword, object salt)
        {
            var valueBytes = KeyDerivation.Pbkdf2(
                password: userPassword,
                salt: Encoding.UTF8.GetBytes(salt.ToString()),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 32);
            return Encoding.UTF8.GetString(valueBytes);
        }
}
