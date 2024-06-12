using Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LevelController : ControllerBase
    {
        private readonly MyDbContext _context;
        public LevelController(MyDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<LevelOfWork>>> GetLevel()
        {
            /*return await _context.jobLists.ToListAsync();*/
            return await _context.levelOfWorks.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LevelOfWork>> GetLevel(int id)
        {
            var comp = await _context.levelOfWorks.FirstOrDefaultAsync(j => j.LevId == id);

            if (comp == null)
            {
                return NotFound("Khong ton tai");
            }
            return comp;
        }
    }
}
