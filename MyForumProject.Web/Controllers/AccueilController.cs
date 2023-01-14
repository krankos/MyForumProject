using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyForumProject.BL.Entities;
using MyForumProject.DAL;

namespace MyForumProject.Web.Controllers
{
	
	public class AccueilController : Controller
	{
		private readonly MyForumProjectDbContext _context;
		public AccueilController(MyForumProjectDbContext context)
		{
			_context = context;
		}
		public async Task<IActionResult> Index()
		{
			var myForumProjectDbContext = _context.Posts.Include(p => p.Blog);
			return View(await myForumProjectDbContext.ToListAsync());



		}
	}
}

