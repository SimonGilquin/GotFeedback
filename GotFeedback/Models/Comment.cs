using System.ComponentModel.DataAnnotations.Schema;

namespace GotFeedback.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public int TopicId { get; set; }

        public string Message { get; set; }

        [ForeignKey("TopicId")]
        public virtual Topic Topic { get; set; }
    }
}