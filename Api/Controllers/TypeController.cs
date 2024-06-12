using Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeController : ControllerBase
    {
        private readonly MyDbContext _context;
        public TypeController(MyDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<TypeWork>>> GetType()
        {
            /*return await _context.jobLists.ToListAsync();*/
            return await _context.typeWorks.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TypeWork>> GetType(int id)
        {
            var cate = await _context.typeWorks.FirstOrDefaultAsync(j => j.TypeId == id);

            if (cate == null)
            {
                return NotFound("Khong ton tai type");
            }
            return cate;
        }
    }
}
