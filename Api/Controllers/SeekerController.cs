using Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeekerController : ControllerBase
    {
        private readonly MyDbContext _context;
        public SeekerController(MyDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Seeker>>> GetSeeker()
        {
            /*return await _context.jobLists.ToListAsync();*/
            return await _context.seekers.Include(j => j.User).Include(j => j.Location).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Seeker>> GetSeeker(int id)
        {
            var comp = await _context.seekers.Include(j => j.User).Include(j => j.Location).FirstOrDefaultAsync(j => j.SeekerId == id);

            if (comp == null)
            {
                return NotFound("Khong ton tai");
            }
            return comp;
        }
        [HttpGet("GetByUserId/{id}")]
        public async Task<ActionResult<Seeker>> GetByUserId(int id)
        {
            var comp = await _context.seekers.Include(j => j.User).Include(j => j.Location).FirstOrDefaultAsync(j => j.UserId == id);

            if (comp == null)
            {
                return NotFound("Khong ton tai seeker");
            }
            return comp;
        }
        [HttpPost]
        public async Task<ActionResult<Seeker>> CreateSeeker(int id)
        {
            Seeker comp = new Seeker();
            comp.UserId = id;
            var randomNumber = new Random().Next(1000, 99999);
            comp.Name = "User " + randomNumber.ToString();
            _context.seekers.Add(comp);
            await _context.SaveChangesAsync();
            return Ok(comp);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSeeker(int id, SeekerItem item)
        {
            var comp = await _context.seekers.FindAsync(id);
            if (comp != null)
            {
                comp.Name = item.Name;
                comp.PhoneNumber = item.PhoneNumber;
                comp.FileCV = item.FileCV;
                comp.Photo = item.Photo;
                comp.GPA = item.GPA;
                comp.Major = item.Major;
                comp.SeekerAddress = item.SeekerAddress;
                comp.LocationId = item.LocationId;
                await _context.SaveChangesAsync();
                return Ok("Updated");
            }
            return NotFound();
        }
    }
}
