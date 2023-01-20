using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyForumProject.BL.Entities
{
	public class Comment
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CommentId { get; set; }
	    public string? Body { get; set; }
		public DateTime? CreatedAt { get; set; }
        public string? OwnerName { get; set; }

        [ForeignKey("Post")]
        public int PostId { get; set; }
        public Post? Post { get; set; }

        [ForeignKey("Owner")]
        public string? OwnerId { get; set; }
        public User? Owner { get; set; }

    }
}
