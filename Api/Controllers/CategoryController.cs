using Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly MyDbContext _context;
        public CategoryController(MyDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategory()
        {
            /*return await _context.jobLists.ToListAsync();*/
            return await _context.categories.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var cate = await _context.categories.FirstOrDefaultAsync(j => j.CategoryId == id);

            if (cate == null)
            {
                return NotFound("Khong ton tai category");
            }
            return cate;
        }
        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory(Category item)
        {
            Category cate = new Category();
            cate.CategoryName = item.CategoryName;
          
            _context.categories.Add(cate);
            await _context.SaveChangesAsync();
            return Ok(cate);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, Category item)
        {
            var cate = await _context.categories.FindAsync(id);
            if (cate != null)
            {               
                cate.CategoryName = item.CategoryName;               
                await _context.SaveChangesAsync();
                return Ok("Updated");
            }
            return NotFound();           
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var item = await _context.categories.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            _context.categories.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
