using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace GotFeedback.Models
{
    public class Topic
    {
        private string _tagsLiteral;
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public TopicCategory Category { get; set; }

        public DateTime CreatedDate { get; set; }
        public ApplicationUser User { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }

        public int LikesCount { get; set; }

        public int ViewCount { get; set; }

        [NotMapped]
        public string TagsLiteral
        {
            get
            {
                if (string.IsNullOrEmpty(_tagsLiteral)) _tagsLiteral = GetTagsAsLitteral();
                return _tagsLiteral;
            }
            set { _tagsLiteral = value; }
        }

        private string GetTagsAsLitteral()
        {
            if (Tags == null) return string.Empty;
            var tagsSb = new StringBuilder();
            bool isFirst = true;


            foreach (var tag in Tags)
            {
                if (isFirst)
                {
                    tagsSb.Append(tag.Label);
                    isFirst = false;
                }
                else
                {
                    tagsSb.AppendFormat(",{0}", tag.Label);
                }
            }

            return tagsSb.ToString();
        }



    }

    public enum TopicCategory
    {
        Idea,
        Bug,
        Question
    }
}