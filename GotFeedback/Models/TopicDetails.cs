using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Cryptography;
using System.Text;
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

        public string GravatarUrl { get; set; }

        public int ViewCount { get; set; }
        public bool IsOwner { get; set; }

        public List<Tag> Tags { get; set; }

        private string _tagsLiteral;

        [NotMapped]
        public string TagsLiteral
        {
            get
            {
                if (string.IsNullOrEmpty(_tagsLiteral)) _tagsLiteral = GetTagsAsLitteral();
                return _tagsLiteral;
            }
            set
            {
                if (_tagsLiteral == value) return;
                _tagsLiteral = value;

            }
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
        public int LikesCount { get; set; }
        public IEnumerable<string> TagLabels { get; set; }
    }
}