using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyForumProject.BL.Entities
{
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int PostId { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime PublishedDateTime { get; set; }
        [ForeignKey("Blog")]

        public int BlogId { get; set; }
        public Blog? Blog { get; set; }
		public virtual List<Comment>? Comments { get; set; }

        [ForeignKey("Owner")]
        public string OwnerId { get; set; }
        public User? Owner { get; set; }

	}
}
