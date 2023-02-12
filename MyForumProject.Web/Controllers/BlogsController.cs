using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MyForumProject.BL.Entities;
using System.Security.Principal;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;

using MyForumProject.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyForumProject.Web.Controllers
{
    public class BlogsController : Controller
	{
		private readonly MyForumProjectDbContext _context;
        //get current user id from the session
        

		
        

        public BlogsController(MyForumProjectDbContext context)
        {
            _context = context;
           
		}

		// GET: Blogs
		public async Task<IActionResult> Index()
        {
              return View(await _context.Blogs.ToListAsync());

			
            
		}

		// GET: Blogs/Details/5
		public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Blogs == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs
                .FirstOrDefaultAsync(m => m.BlogId == id);
            if (blog == null)
            {
                return NotFound();
            }
            // get posts of the blog
            var posts = await _context.Posts.Where(p => p.BlogId == id).ToListAsync();
            blog.Posts = posts;
            // sort posts by CreatedAt
            blog.Posts = blog.Posts.OrderByDescending(p => p.CreatedAt).ToList();
            if (blog.Posts == null)
            {
                return NotFound();
            }
            

            return View(blog);
        }

        // GET: Blogs/Create
        [Authorize]
        public IActionResult Create()
        {
           

            return View();
        }

        // POST: Blogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BlogId,Nom,Description,OwnerId")] Blog blog)
        {
            if (ModelState.IsValid)
            {  // get current user id from the session
               
                
                //write userId in the console
                


                blog.OwnerId = User.Identity.GetUserId();
                blog.Owner = _context.Users.Find(blog.OwnerId);
                blog.OwnerName = User.Identity.GetUserName();

                User owner = _context.Users.Find(blog.OwnerId);

                if (owner.Blogs == null)
                {
                    owner.Blogs = new List<Blog>();
                }

                owner.Blogs.Append(blog);
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                Console.WriteLine(owner.Blogs.Count);
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!");


                Console.WriteLine(blog.OwnerId);
                Console.WriteLine(blog.Owner);
                Console.WriteLine(blog.OwnerName);
                _context.Add(blog);
                _context.Update(owner);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(blog);
        }

        // GET: Blogs/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Blogs == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs.FindAsync(id);
            if (blog == null)
            {
                return NotFound();
            }
            return View(blog);
        }

        // POST: Blogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BlogId,Nom,Description")] Blog blog)
        {
            if (id != blog.BlogId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                //get the blog from id 
                var blogFromId = await _context.Blogs.FindAsync(id);
                //if the blog is not found
                if (blogFromId == null)
                {
                    return NotFound();
                }
               
                
                try
                {
                    _context.Update(blog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogExists(blog.BlogId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(blog);
        }

        // GET: Blogs/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Blogs == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs
                .FirstOrDefaultAsync(m => m.BlogId == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // POST: Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Blogs == null)
            {
                return Problem("Entity set 'MyForumProjectDbContext.Blogs'  is null.");
            }
            var blog = await _context.Blogs.FindAsync(id);
            if (blog != null)
            {
                _context.Blogs.Remove(blog);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogExists(int id)
        {
          return _context.Blogs.Any(e => e.BlogId == id);
        }

        // create new blog post
        [Authorize]
        public IActionResult CreatePost(int? id)
        {
            // Say hello and show the id of the blog
            Console.WriteLine("Hello from CreatePost");
            Console.WriteLine(id);
            if (id == null || _context.Blogs == null)
            {
                return NotFound();
            }

            var blog = _context.Blogs.Find(id);
            if (blog == null)
            {
                return NotFound();
            }
            // create a new post and set the blog id
            var post = new Post();
            post.BlogId = blog.BlogId;
            Console.WriteLine(post.BlogId);
            return RedirectToAction("Create", "Posts", new {id});
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(int id,[Bind("BlogId,PostId,Title,Content")] Post post)
        {
            // Say hello
            //Console.WriteLine("Hello from CreatePost");

            // find the blog by id and set it to post
            var blog = _context.Blogs.Find(id);
            post.Blog = blog;
            // set the blog id
            post.BlogId = blog.BlogId;


            if (ModelState.IsValid)
            {
                // Print the blog id
                Console.WriteLine(post.BlogId);
                // set owner
                post.OwnerId = User.Identity.GetUserId();
                post.Owner = _context.Users.Find(post.OwnerId);
                post.OwnerName = post.Owner.UserName;

                User owner = _context.Users.Find(post.OwnerId);

                if (owner.Posts == null)
                {
                    owner.Posts = new List<Post>();
                }
                owner.Posts.Append(post);
                // dispalay owner info
                Console.WriteLine(post.Owner.UserName);
                DateTime now = DateTime.Now;
                post.CreatedAt = now;
                _context.Add(post);
                _context.Update(owner);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }
        public async Task<IActionResult> DetailsPost(int id)
        {
            Console.WriteLine("BlogDetails");
            return RedirectToAction("Details", "Posts", new { id = id });
        }

        public async Task<IActionResult> EditPost(int id)
        {
            if (User.Identity.GetUserName() != _context.Posts.Find(id).OwnerName)
            {
                return RedirectToAction("Index", "Accueil");
            }
            return RedirectToAction("Edit", "Posts", new { id = id });
        }

        public async Task<IActionResult> DeletePost(int id)
        {
            if (User.Identity.GetUserName() != _context.Posts.Find(id).OwnerName)
            {
                return RedirectToAction("Index", "Accueil");
            }
            return RedirectToAction("Delete", "Posts", new { id = id });
        }

       

    }
}
