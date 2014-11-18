namespace GotFeedback.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public int TopicId { get; set; }

        public string Label { get; set; }
    }
}