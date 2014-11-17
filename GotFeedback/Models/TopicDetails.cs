﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GotFeedback.Models
{
    public class TopicDetails
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public TopicCategory Category { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Username { get; set; }
    }
}