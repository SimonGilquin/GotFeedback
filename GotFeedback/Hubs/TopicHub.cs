using System.Linq;
using GotFeedback.Models;
using Microsoft.AspNet.SignalR;

namespace GotFeedback.Hubs
{
    public class TopicHub : Hub
    {
/*
        private readonly ApplicationDbContext db;

        public TopicHub()
        {
            db = new ApplicationDbContext();
        }

        public void TopicCommented(int id)
        {
            var usersOnTopic = db.Topics.Select(t => new {t.User.UserName, t.Id}).SingleOrDefault(t => t.Id == id);
            if (usersOnTopic != null)
            {
                Clients./*User(usersOnTopic.UserName)#1#All.notify();
            }
        }
*/
    }
}