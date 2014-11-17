using System;
using System.ComponentModel.DataAnnotations;

namespace GotFeedback.Domain
{
    public class User
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime? CreatedDate { get; set; }

    }
}