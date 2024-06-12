using Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly MyDbContext _context;

        public RegisterController(MyDbContext context)
        {
            _context = context;

        }
        [HttpPost]
        public async Task<IActionResult> Register(RegistrationModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var mail = await _context.users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (mail != null)
            {
                return BadRequest("Email is exist");
            }
            var user = new User();
            user.Email = model.Email;
            user.Password = model.Password;
            user.CreatedDate = DateTime.UtcNow;
            user.UserTypeId = model.UserTypeId;
            user.IsActive = true;
            _context.Add(user);
            _context.SaveChanges();
            return Ok(user);
        }
    }
}
