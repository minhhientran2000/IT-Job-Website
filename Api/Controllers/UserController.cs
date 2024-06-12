using Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MyDbContext _context;
        public UserController(MyDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            /*return await _context.jobLists.ToListAsync();*/
            return await _context.users.ToListAsync();
        }
        [HttpGet("registration-stats")]
        public async Task<ActionResult<IEnumerable<UserRegistrationStats>>> GetUserRegistrationStats()
        {
            var stats = await _context.users
                .GroupBy(u => new { u.CreatedDate.Year, u.CreatedDate.Month })
                .Select(g => new UserRegistrationStats
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    UserCount = g.Count()
                })
                .ToListAsync();

            return stats;
        }
        [HttpGet("compare")]
        public async Task<ActionResult<object>> CompareUser()
        {
            var currentDate = DateTime.Now;
            var startOfCurrentMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            var startOfLastMonth = startOfCurrentMonth.AddMonths(-1);
            var endOfLastMonth = startOfCurrentMonth.AddDays(-1);

            var usersThisMonth = await _context.users
                .Where(u => u.CreatedDate >= startOfCurrentMonth && u.CreatedDate <= currentDate)
                .ToListAsync();

            var usersLastMonth = await _context.users
                .Where(u => u.CreatedDate >= startOfLastMonth && u.CreatedDate <= endOfLastMonth)
                .ToListAsync();

            var thisMonthCount = usersThisMonth.Count;
            var lastMonthCount = usersLastMonth.Count;

            // Tính toán phần trăm tăng/giảm
            double percentChange = 0;
            string changeStatus = "";
            if (lastMonthCount != 0)
            {
                percentChange = ((double)thisMonthCount - lastMonthCount) / lastMonthCount * 100;
                changeStatus = percentChange >= 0 ? "Tăng" : "Giảm";
                percentChange = Math.Round(percentChange, 1); // Làm tròn số sau dấu phẩy đến 1 chữ số
                percentChange = Math.Abs(percentChange); // Lấy giá trị tuyệt đối                
            }
            var result = new
            {
                ThisMonth = usersThisMonth.Count,
                LastMonth = usersLastMonth.Count,
                PercentChange = percentChange,
                ChangeStatus = changeStatus
            };

            return result;
        }
    }
    public class UserRegistrationStats
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int UserCount { get; set; }
    }

}
