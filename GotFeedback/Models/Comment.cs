using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GotFeedback.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        public int TopicId { get; set; }

        [Required]
        public string Message { get; set; }

        [ForeignKey("TopicId")]
        public virtual Topic Topic { get; set; }

        public DateTime Date { get; set; }
    }
}