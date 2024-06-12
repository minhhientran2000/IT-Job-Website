using Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly MyDbContext _context;
        public LocationController(MyDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Location>>> GetLocation()
        {
            /*return await _context.jobLists.ToListAsync();*/
            return await _context.locations.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Location>> GetLocation(int id)
        {
            var loca = await _context.locations.FirstOrDefaultAsync(j => j.LocationId == id);

            if (loca == null)
            {
                return NotFound("Khong ton tai location");
            }
            return loca;
        }
        [HttpPost]
        public async Task<ActionResult<Location>> CreateLocation(Location item)
        {
            Location loca = new Location();
            loca.Name = item.Name;

            _context.locations.Add(loca);
            await _context.SaveChangesAsync();
            return Ok(loca);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLocation(int id, Location item)
        {
            var loca = await _context.locations.FindAsync(id);
            if (loca != null)
            {
                loca.Name = item.Name;
                await _context.SaveChangesAsync();
                return Ok("Updated");
            }
            return NotFound();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            var item = await _context.locations.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            _context.locations.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }

}
