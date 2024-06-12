using Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly MyDbContext _context;

        public LoginController(MyDbContext context)
        {
            _context = context;
           
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = await _context.users.FirstOrDefaultAsync(u => u.Email.ToLower() == model.Email.ToLower() && u.Password == model.Password);
            if (user == null)
            {
                return BadRequest("Invalid username or password");
            }         
            return Ok(user);          
        }
        /*[HttpGet]
        public async Task<IActionResult> GetId()
        {
            LoginModel loginModel = new LoginModel();
            string str = loginModel.Email;
            User user = _context.users.Where(u => u.Email == str).FirstOrDefault();
            var num = user.UserTypeId;
            return Ok(num);
        }*/
    }
}
