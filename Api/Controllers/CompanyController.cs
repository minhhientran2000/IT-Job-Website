using Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly MyDbContext _context;
        public CompanyController(MyDbContext context)
        {
            _context = context;
        }
        

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompany()
        {
            /*return await _context.jobLists.ToListAsync();*/
            return await _context.companies.Include(j => j.User).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompany(int id)
        {
            var comp = await _context.companies.Include(j => j.User).FirstOrDefaultAsync(j => j.CompanyId == id);

            if (comp == null)
            {
                return NotFound("Khong ton tai company");
            }
            return comp;
        }
       
        [HttpGet("GetByUserId/{id}")]
        public async Task<ActionResult<Company>> GetByUserId(int id)
        {
            var comp = await _context.companies.FirstOrDefaultAsync(j => j.UserId == id);

            if (comp == null)
            {
                return NotFound("Khong ton tai company");
            }
            return comp;
        }

        [HttpGet("Search")]
        public ActionResult<IEnumerable<Company>> Search(string keyword)
        {
            // Tìm kiếm các công việc dựa trên từ khóa
            var comp = _context.companies.OrderByDescending(item => item.CompanyId).Where(j => j.CompanyName.Contains(keyword) || j.CompanyAddress.Contains(keyword)).ToList();
            return comp;
        }
        [HttpPost]
        public async Task<ActionResult<Company>> CreateCompany(int id)
        {
            Company comp = new Company();          
            comp.UserId = id;
           /* comp.CompanyName = "Chua co";
            comp.CompanyDescription = "Chua co";
            comp.CompanyWebsiteUrl = "Chua co";
            comp.CompanyPhoto = "Chua co";
            comp.CompanyEmail = "Chua co";*/
            comp.IsActive = true;

            var randomNumber = new Random().Next(1000, 99999);
            comp.CompanyName = "Company " + randomNumber.ToString();

            _context.companies.Add(comp);
            await _context.SaveChangesAsync();
            return Ok(comp);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(int id, CompanyItem item)
        {
            var comp = await _context.companies.FindAsync(id);
            if (comp != null)
            {
                comp.CompanyName = item.CompanyName;
                comp.CompanyDescription = item.CompanyDescription;
                comp.CompanyEmail = item.CompanyEmail;
                comp.CompanyWebsiteUrl = item.CompanyWebsiteUrl;
                comp.CompanyPhoto = item.CompanyPhoto;
                comp.CompanyAddress = item.CompanyAddress;
                await _context.SaveChangesAsync();
                return Ok("Updated");
            }
            return NotFound();
        }



    }
}
