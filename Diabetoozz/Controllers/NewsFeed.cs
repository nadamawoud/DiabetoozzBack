using Diabetes.Repository.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Diabetoozz.APIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsFeedController : ControllerBase
    {
        private readonly StoreContext _context;

        public NewsFeedController(StoreContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetNewsFeed()
        {
            // استرجاع المنشورات من الـ NewsFeed، مع ترتيب المنشورات بناءً على تاريخ الإنشاء
            var posts = await _context.NewsFeedPosts
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return Ok(posts); // إرجاع المنشورات
        }
    }
}