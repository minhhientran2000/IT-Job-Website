using Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobListController : ControllerBase
    {
        private readonly MyDbContext _context;
        public JobListController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobList>>> GetJob()
        {
            /*return await _context.jobLists.ToListAsync();*/
            return await _context.jobLists.OrderByDescending(item => item.JobId)
                .Include(j => j.Company)
                .Include(j => j.Category)
                .Include(j => j.Type)
                .Include(j => j.Lev)
                .Include(j => j.Location)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<JobList>> GetJob(int id)
        {
            var job = await _context.jobLists.OrderByDescending(item => item.JobId).Include(j => j.Company).Include(j => j.Category).Include(j => j.Type).Include(j => j.Lev).Include(j => j.Location).FirstOrDefaultAsync(j => j.JobId == id);

            if (job == null)
            {
                return NotFound("Khong ton tai job");
            }
            return job;
        }

        [HttpGet("search")]
        public ActionResult<IEnumerable<JobList>> Search(string keyword)
        {
            // Tìm kiếm các công việc dựa trên từ khóa
            var jobs = _context.jobLists.OrderByDescending(item => item.JobId).Include(j => j.Company).Include(j => j.Category).Include(j => j.Type).Include(j => j.Lev).Include(j => j.Location).Where(j => j.JobTitle.Contains(keyword)).ToList();
            return jobs;
        }

        [HttpGet("SearchByCategory")]
        public ActionResult<IEnumerable<JobList>> SearchByCategory(int id)
        {
            // Tìm kiếm các công việc dựa trên từ khóa
            var jobs = _context.jobLists.OrderByDescending(item => item.JobId).Include(j => j.Company).Include(j => j.Category).Include(j => j.Type).Include(j => j.Lev).Include(j => j.Location).Where(j => j.Category.CategoryId == id).ToList();
            return jobs;
        }

        [HttpGet("SearchByLocation")]
        public ActionResult<IEnumerable<JobList>> SearchByLocation([FromQuery] List<int> ids)
        {
            // Tìm kiếm các công việc dựa trên danh sách các ID
            var jobs = _context.jobLists.OrderByDescending(item => item.JobId)
                .Include(j => j.Company)
                .Include(j => j.Category)
                .Include(j => j.Type)
                .Include(j => j.Lev)
                .Include(j => j.Location)
                .Where(j => ids.Contains(j.Location.LocationId))
                .ToList();

            return jobs;
        }

        [HttpGet("GetJobByCompanyId/{id}")]
        public async Task<ActionResult<IEnumerable<JobList>>> GetJobByCompanyId(int id)
        {
            var jobs = await _context.jobLists.OrderByDescending(item => item.JobId)
                .Include(j => j.Company)
                .Include(j => j.Category)
                .Include(j => j.Type)
                .Include(j => j.Lev)
                .Include(j => j.Location)
                .Where(j => j.CompanyId == id)
                .ToListAsync();

            if (jobs.Count == 0) // Check for empty list
            {               
                return NotFound("Khong ton tai job");
            }

            return jobs;
        }

        [HttpPost]
        public async Task<ActionResult<JobList>> CreateJob(JobItem item)
        {
            JobList job = new JobList();
            job.JobTitle = item.JobTitle;
            job.JobDescription = item.JobDescription;
            job.JobLocation = item.JobLocation;
            job.SalaryRange = item.SalaryRange;
            job.IsActive = true;
            job.CreatedDate = DateTime.Now;
            job.ExpiredDate = item.ExpiredDate;
            job.CompanyId = item.CompanyId;
            job.CategoryId = item.CategoryId;
            job.TypeId = item.TypeId;
            job.LevId = item.LevId;
            job.LocationId = item.LocationId;
            job.JobRequirement = item.JobRequirement;
            _context.jobLists.Add(job);
            await _context.SaveChangesAsync();
            return Ok(job);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJob(int id, JobItem item)
        {
            var job = await _context.jobLists.FindAsync(id);
            if (job != null)
            {
                job.JobTitle = item.JobTitle;
                job.JobDescription = item.JobDescription;
                job.JobLocation = item.JobLocation;
                job.SalaryRange = item.SalaryRange;
                job.IsActive = item.IsActive;
                job.ExpiredDate = item.ExpiredDate;
                job.CompanyId = item.CompanyId;
                job.CategoryId = item.CategoryId;
                job.TypeId = item.TypeId;
                job.LevId = item.LevId;
                job.LocationId = item.LocationId;
                job.JobRequirement = item.JobRequirement;
                await _context.SaveChangesAsync();
                return Ok("Updated");
            }
            return NotFound();            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteJob(int id)
        {
            var item = await _context.jobLists.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            _context.jobLists.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool JobExists(int id)
        {
            return _context.jobLists.Any(e => e.JobId == id);
        }
    }
}
