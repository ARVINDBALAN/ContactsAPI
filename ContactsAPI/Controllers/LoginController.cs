using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ContactsAPI.Model;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace ContactsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly IConfiguration _config;

        public LoginController(IConfiguration configuration)
        {
                this._config = configuration;
        }


        private Users AuthenticateUser(Users users)
        {
            Users _users = null;

            if (users.Username == "admin" &&  users.Password =="12345")
            {
                _users = new Users { Username = "Aravind" };
            }


            return _users;
        }


        private string GenerateTokens(Users users)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"],null,
                expires : DateTime.Now.AddMinutes(5),
                signingCredentials : credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(Users users)
        {
            IActionResult response = Unauthorized();
            var user = AuthenticateUser(users);

            if (user != null)
            {
                var token = GenerateTokens(user);
                response = Ok(new {token = token});
            }

            return response;
        }
    }
}
