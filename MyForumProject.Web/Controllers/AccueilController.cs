using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
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
            //blog de chaque post
            var blogs = await _context.Blogs.ToListAsync();
            
            var posts = await _context.Posts.ToListAsync();
            // importer le blog de chaque post 
            //comments of each post 
            var comments = await _context.Comments.ToListAsync();
            var newposts = new List<Post>();
            for (var i = posts.Count - 1; i >= 0; i--)
            { // chercher et importer le blog de cette post
                var blog = await _context.Blogs.Where(b => b.BlogId == posts[i].BlogId).FirstOrDefaultAsync();
                //ajouter le blog a la post
                posts[i].Blog = blog;
                newposts.Add(posts[i]);
            }

            
            foreach (var item in newposts)
            {
                Console.WriteLine(item.Title);
            }
            //check if post in not null
            if (posts != null)
            { posts = newposts; }

            if (posts == null)
            {
                return NotFound();
            }


            // retourner la liste newposts
            return View(posts);



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
            //check if post in not null
            if (post != null)
            { post.Comments = newComments; }
                //set the comments to the post
                
               
                //
            






            if (post == null)
            {
                return NotFound();
            }
            //return view with the post of type list 
            
            return View(post);
        }

        [Authorize]
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
            return RedirectToAction("Create", "Comments", new { id = post.PostId });



        }

        [Authorize]
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
                comment.OwnerName = User.Identity.GetUserName();
                User user = _context.Users.Find(comment.OwnerId);

                if (user.Comments == null)
                {
                    user.Comments = new List<Comment>();
                }
                user.Comments.Append(comment);
                DateTime now = DateTime.Now;
                comment.CreatedAt = now;
                _context.Add(comment);
                _context.Update(user);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
               
            }
            // return View("Details",post);
            //return RedirectToAction("Details", "Posts", new { id = post.PostId });
            return RedirectToAction("PostsWithComments", "Accueil", new { id = post.PostId });

        }

        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            return RedirectToAction("Delete", "Comments", new { id });
        }

        public async Task<IActionResult> BlogDetails(int id)
        {
            Console.WriteLine("BlogDetails");
            return RedirectToAction("Details", "Blogs", new { id = id });
        }

        public async Task<IActionResult> EditBlog(int id)
        {
            if (User.Identity.GetUserId() != _context.Blogs.Find(id).OwnerId)
            {
                return RedirectToAction("Index", "Accueil");
            }
            return RedirectToAction("Edit", "Blogs", new { id = id });
        }

        public async Task<IActionResult> DeleteBlog(int id)
        {
            if (User.Identity.GetUserId() != _context.Blogs.Find(id).OwnerId)
            {
                return RedirectToAction("Index", "Accueil");
            }
            return RedirectToAction("Delete", "Blogs", new { id = id });
        }

        public async Task<IActionResult> PostDetails(int id)
        {
            Console.WriteLine("BlogDetails");
            return RedirectToAction("Details", "Posts", new { id = id });
        }

        public async Task<IActionResult> EditPost(int id)
        {
            if (User.Identity.GetUserId() != _context.Posts.Find(id).OwnerId)
            {
                return RedirectToAction("Index", "Accueil");
            }
            return RedirectToAction("Edit", "Posts", new { id = id });
        }

        public async Task<IActionResult> DeletePost(int id)
        {
            if (User.Identity.GetUserId() != _context.Posts.Find(id).OwnerId)
            {
                return RedirectToAction("Index", "Accueil");
            }
            return RedirectToAction("Delete", "Posts", new { id = id });
        }

        public async Task<IActionResult> DeleteComment(int id)
        {
            if (User.Identity.GetUserId() != _context.Comments.Find(id).OwnerId)
            {
                return RedirectToAction("Index", "Accueil");
            }
            return RedirectToAction("Delete", "Comments", new { id = id });
        }


    }
   
}

