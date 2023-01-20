using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MyForumProject.BL.Entities;
using MyForumProject.DAL;
using System.Linq;
using System.Reflection.Metadata;

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
            //comments of each post 
            var comments = await _context.Comments.ToListAsync();
           




            return View(await myForumProjectDbContext.ToListAsync());



		}
        public async Task<IActionResult> PostWithComments(int? id)
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
            
                var newComments = new List<Comment>();
                for (var comment = comments.Count - 1; comment >= 0; comment--)
                {
                    newComments.Add(comments[comment]);
                }


                foreach (var comment in newComments)
                {
                    Console.WriteLine(comment.Body);
                }
                post.Comments = newComments;
                //
            






            if (post == null)
            {
                return NotFound();
            }
            //return view with the post of type list 
            
            return View(post);
        }


        public IActionResult AddComment(int? id)
        {
            // Say hello and show the id of the post
            Console.WriteLine("Hello from Createcomment");
            Console.WriteLine(id);
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = _context.Posts.Find(id);
            if (post == null)
            {
                return NotFound();
            }
            // create a new comment and set the post id 
            var comment = new Comment();
            comment.PostId = (int)id;
            Console.WriteLine(comment.PostId);
            return View(comment);
            


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int id, [Bind("CommentId,Body,CreatedDate,PostId,OwnerId")] Comment comment)
        {
            // find the post by id and set it to the comment
            var post = _context.Posts.Find(id);
            comment.Post = post;
            // set the post id
            comment.PostId = post.PostId;
            

            if (ModelState.IsValid)
            {
              
                // set owner
               comment.OwnerId = User.Identity.GetUserId();
               comment.Owner = _context.Users.Find(post.OwnerId);
                comment.OwnerName = post.Owner.UserName;
               
                DateTime now = DateTime.Now;
                comment.CreatedAt = now;
                _context.Add(comment);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
               
            }
            // return View("Details",post);
            //return RedirectToAction("Details", "Posts", new { id = post.PostId });
            return RedirectToAction("PostWithComments", "Accueil", new { id = post.PostId });

        }

        public async Task<IActionResult> Delete(int? id)
        {
            return RedirectToAction("Delete", "Comments", new { id });
        }

        


    }
   
}

