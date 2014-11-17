using System.Web.UI.WebControls;

namespace GotFeedback.Domain
{
    public class Comment
    {
        public int Id { get; set; }

        public int TopicId { get; set; }

        public string Message { get; set; }
    }
}