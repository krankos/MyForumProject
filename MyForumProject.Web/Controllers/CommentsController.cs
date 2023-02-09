using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyForumProject.BL.Entities;
using MyForumProject.DAL;

namespace MyForumProject.Web.Controllers
{
    public class CommentsController : Controller
    {
        private readonly MyForumProjectDbContext _context;

        public CommentsController(MyForumProjectDbContext context)
        {
            _context = context;
        }

        // GET: Comments
        public async Task<IActionResult> Index()
        {
            var myForumProjectDbContext = _context.Comments.Include(c => c.Owner).Include(c => c.Post);
            return View(await myForumProjectDbContext.ToListAsync());
        }

        // GET: Comments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Comments == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.Owner)
                .Include(c => c.Post)
                .FirstOrDefaultAsync(m => m.CommentId == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // GET: Comments/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["CommentId"] = new SelectList(_context.Posts, "PostId", "OwnerId");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, [Bind("CommentId,Body,CreatedDate,PostId,OwnerId")] Comment comment)
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
            return RedirectToAction("PostWithComments", "Accueil", new { id = post.PostId });

        }


        // GET: Comments/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Comments == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id", comment.OwnerId);
            ViewData["CommentId"] = new SelectList(_context.Posts, "PostId", "OwnerId", comment.CommentId);
            return View(comment);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CommentId,Body,CreatedDate,PostId,OwnerId")] Comment comment)
        {
            if (id != comment.CommentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment.CommentId))
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
            ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id", comment.OwnerId);
            ViewData["CommentId"] = new SelectList(_context.Posts, "PostId", "OwnerId", comment.CommentId);
            return View(comment);
        }

        // GET: Comments/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Comments == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.Owner)
                .Include(c => c.Post)
                .FirstOrDefaultAsync(m => m.CommentId == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        { // find the post of this commentid
            
            if (_context.Comments == null)
            {
                return Problem("Entity set 'MyForumProjectDbContext.Comments'  is null.");
            }
            var comment = await _context.Comments.FindAsync(id);
            var post = await _context.Posts.FindAsync(comment.PostId);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("PostWithComments", "Accueil", new { id = post.PostId });
        }

        private bool CommentExists(int id)
        {
          return _context.Comments.Any(e => e.CommentId == id);
        }
    }
}
