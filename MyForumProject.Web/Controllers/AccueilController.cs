using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Blog)
                .FirstOrDefaultAsync(m => m.PostId == id);
            //display comments
            var comments = await _context.Comments.Where(c => c.PostId == id).ToListAsync();
            //add comment to post
            post.Comments = comments;





            if (post == null)
            {
                return NotFound();
            }
            //return view with the post of type list 
            
            return View(post);
        }


        public IActionResult AddComment()
        {
            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["CommentId"] = new SelectList(_context.Posts, "PostId", "OwnerId");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment([Bind("CommentId,Body,CreatedDate,PostId,OwnerId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id", comment.OwnerId);
            ViewData["CommentId"] = new SelectList(_context.Posts, "PostId", "OwnerId", comment.CommentId);
            return View(comment);
        }
    }
   
}

