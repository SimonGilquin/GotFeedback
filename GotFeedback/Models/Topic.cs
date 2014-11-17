using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace GotFeedback.Models
{
    public class Topic
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public DateTime? CreatedDate { get; set; }

        public virtual ICollection<Comment> Comments { get; set; } 
    }
}