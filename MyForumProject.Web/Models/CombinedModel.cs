using MyForumProject.BL.Entities;

namespace MyForumProject.Web.Models
{
    public class CombinedModel
    {
        public IEnumerable<Post>? Posts { get; set; }
        public IEnumerable<Blog>? Blogs { get; set; }
		public IEnumerable<Comment>? Comments { get; set; }
	}
}
