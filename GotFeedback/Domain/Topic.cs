using System;

namespace GotFeedback.Domain
{
    public class Topic
    {
        public int Id { get; set; }

        public string Message { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}