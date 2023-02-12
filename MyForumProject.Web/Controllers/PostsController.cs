using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyForumProject.BL.Entities;
using MyForumProject.DAL;
using Org.BouncyCastle.Asn1.X509;

namespace MyForumProject.Web.Controllers
{
    public class PostsController : Controller
    {
        private readonly MyForumProjectDbContext _context;

        public PostsController(MyForumProjectDbContext context)
        {
            _context = context;
        }

        // GET: Posts
        public async Task<IActionResult> Index(int? id)
        {
            var myForumProjectDbContext = _context.Posts.Include(p => p.Blog);
            return RedirectToAction("Details", "Blogs", new { id = id });




        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Blog)
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        [Authorize]
        public IActionResult Create(int? id)
        {
            if (id == null || _context.Blogs == null)
            {
                return NotFound();
            }
            var blog = _context.Blogs.Find(id);
            if (blog == null)
            {
                return NotFound();
            }
            return View();

        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, [Bind("PostId,Title,Content,PublishedDateTime,BlogId")] Post post)
        {
            var blog = _context.Blogs.Find(id);
            if (blog == null)
            {
                return NotFound();
            }
            post.BlogId = id;
            post.Blog = blog;


            if (ModelState.IsValid)
            {
                var OwnerId = User.Identity.GetUserId();
                var OwnerName = User.Identity.GetUserName();
                post.OwnerId = OwnerId;
                post.OwnerName = OwnerName;
                post.Owner = _context.Users.Find(OwnerId);

                User user = _context.Users.Find(OwnerId);
                user.Posts = user.Posts ?? new List<Post>();
                user.Posts.Append(post);
                post.CreatedAt = DateTime.Now;
                _context.Add(post);
                _context.Update(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Blogs", new { id = id });
            }
            return View(post);
        }

        // GET: Posts/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            ViewData["BlogId"] = new SelectList(_context.Blogs, "BlogId", "BlogId", post.BlogId);
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PostId,Title,Content,CreatedAt,BlogId, OwnerName,OwnerId,Owner,Blog")] Post post)
        {
            if (id != post.PostId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            { // set the date time using the id of the post
                Console.WriteLine("jit");
                // print post on the console 
                Console.WriteLine(post.CreatedAt);
                Console.WriteLine(post.Content);

                // get the blog from the database
                var blog = _context.Blogs.Find(id);
                if (blog == null)
                {
                    return NotFound();
                }
                // set the blog id using the id of the post7
                post.BlogId = _context.Posts.Find(id).BlogId;
                // set the blog using the id of the post
                post.Blog = _context.Posts.Find(id).Blog;



                // set post owner name using the id of the post
                post.OwnerName = _context.Posts.Find(id).OwnerName;
                // set post owner id using the id of the post
                post.OwnerId = _context.Posts.Find(id).OwnerId;
                // set post owner using the id of the post
                post.Owner = _context.Posts.Find(id).Owner;
                // set post created at using the id of the post
                post.CreatedAt = _context.Posts.Find(id).CreatedAt;




                    _context.Update(post);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Blogs", new { id =post.BlogId });
            }
          
            return View(post);
        }

        // GET: Posts/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Posts == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Blog)
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Posts == null)
            {
                return Problem("Entity set 'MyForumProjectDbContext.Posts'  is null.");
            }
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
            }

            await _context.SaveChangesAsync();
            return View(post);
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.PostId == id);
        }
    }
}
