using System;
using System.Collections.Generic;
using System.Linq;
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

        public string GravatarUrl
        {
            get { return Username == null ? null : string.Format("http://www.gravatar.com/avatar/{0}", BitConverter.ToString(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(Username.ToLowerInvariant()))).Replace("-", "").ToLowerInvariant()); }
        }
    }
}