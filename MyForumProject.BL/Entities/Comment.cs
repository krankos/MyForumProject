using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyForumProject.BL.Entities
{
	public class Comment
	{	
		public int CommentId { get; set; }
	    public string? Body { get; set; }
		public DateTime? CreatedDate { get; set; }
		public User? Owner { get; set; }
		public Post? Post { get; set; }

	}
}
