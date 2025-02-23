using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PawfectCate.Models;

namespace PawfectCate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private readonly PawfectCareContext _context;
        private readonly IConfiguration _configuration;
        public LoginController(PawfectCareContext context,IConfiguration configuration)
        {
            _context = context;
            _configuration=configuration;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginRequestDTO request)
        {
            var user = _context.Customers.FirstOrDefault(u => u.Email == request.Email && u.Password == request.Password);
            if (user == null)
            {
                throw new CustomException("Invalid username or password ! ");

            }
            var token=GenerateJwtToken(user);
            return Ok(new { message = user.Role=="admin" ? "Welcome admin" : "Welcome customer ",token=token,
                user = new
                {
                    id=user.CustomerId,
                    email=user.Email,
                    name=user.Name,
                    role=user.Role
                }
            
            
            });


        }

        private string GenerateJwtToken(Customer user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSuperLongSecretKey1234567890123456")); // At least 32 chars
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, user.Email),
            new Claim(ClaimTypes.Role, user.Role),  // Store role in token
            new Claim("CustomerId",user.CustomerId.ToString()),
            new Claim("FullName",user.Name)

        };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(2), // Token expires in 2 hours
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public class LoginRequestDTO
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
    }
}
