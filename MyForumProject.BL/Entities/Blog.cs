using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyForumProject.BL.Entities
{
    public class Blog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BlogId { get; set; }
		[Required]
		public string? Name { get; set; }
        public string? Description { get; set; }
        public string? OwnerName { get; set; }
        public virtual List<Post>? Posts { get; set; }
        
        [ForeignKey("Owner")]
        public string? OwnerId { get; set; }
        public User? Owner { get; set; }
    }
}
