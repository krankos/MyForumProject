using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace MyForumProject.BL.Entities
{
    public class User :IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? UsernameChangeLimit { get; set; } = 10;
        public byte[]? ProfilePicture { get; set; }
		public virtual List<Post>? Posts { get; set; }
		public virtual List<Comment>? Comments { get; set; }
		public virtual List<Blog>? Blogs { get; set; }
        
	}
}
