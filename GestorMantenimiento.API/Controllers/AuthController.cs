using GestorMantenimiento.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GestorMantenimiento.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _cfg;
        public AuthController(IConfiguration cfg) => _cfg = cfg;

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest req)
        {
            if (req.Username == "admin" && req.Password == "admin123")
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, req.Username),
                    new Claim(ClaimTypes.Role, "Admin")
                };
                return Ok(GenerateJwt(claims));
            }
            if (req.Username == "tech" && req.Password == "tech123")
            {
                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, req.Username),
                    new Claim(ClaimTypes.Role, "Technician")
                };
                return Ok(GenerateJwt(claims));
            }
            return Unauthorized();
        }

        private object GenerateJwt(Claim[] claims)
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_cfg["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _cfg["Jwt:Issuer"],
                audience: _cfg["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );
            return new { token = new JwtSecurityTokenHandler().WriteToken(token) };
        }
    }
}