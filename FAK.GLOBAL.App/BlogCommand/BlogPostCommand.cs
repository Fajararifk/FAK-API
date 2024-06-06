using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FAK.GLOBAL.App.BlogCommand
{
    public class BlogPostCommand
    {
        public string Browser {  get; set; }
        [Key]
        public string Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public string Author { get; set; }
        public DateTime Publication_Date { get; set; }
        public string Tags { get; set; }
        public string ImageUrl { get; set; }
        public int Views { get; set; }
        public int Likes { get; set; }
        public BlogCategory Category { get; set; }
    }
}
