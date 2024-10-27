using MarketWebApp.Data;
using MarketWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace MarketWebApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;


        public LoginController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoginAction(Customer customer)
        {
            if (ModelState.IsValid)
            {
                var dbCustomer = _context.Customer.FirstOrDefault(c => c.Email == customer.Email);
                if (dbCustomer != null && BCrypt.Net.BCrypt.Verify(customer.Password, dbCustomer.Password))
                {
                    var token = GenerateJwtToken(dbCustomer);

                    HttpContext.Response.Cookies.Append("AuthToken", token, new CookieOptions
                    {
                        HttpOnly = true,
                        Expires = DateTime.Now.AddHours(2)
                    });

                    var userJson = JsonSerializer.Serialize(dbCustomer);
                
                    HttpContext.Response.Cookies.Append("User", userJson, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        Expires = DateTime.Now.AddHours(2)
                    });

                    return RedirectToAction("Index", "Home");
                }
                TempData["FailedMessage"] = "Email atau Password Salah!";
            }
            return View("Login", customer);
        }

        private string GenerateJwtToken(Customer user)
        {

            var secretKey = "asdASDSACGiAIdjha123asdasd2131231";

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                 new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                 new Claim(ClaimTypes.Name, user.Name),
                 new Claim(ClaimTypes.Sid, user.Id.ToString()),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
             };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
