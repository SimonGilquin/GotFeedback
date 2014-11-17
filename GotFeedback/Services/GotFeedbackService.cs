using System.Collections.Generic;
using GotFeedback.Domain;

namespace GotFeedback.Services
{
    public interface IGotFeedbackService
    {
        Topic Get(int id);
        List<Topic> GetByUser(string userName);
    }

    public class GotFeedbackService : IGotFeedbackService
    {
        public Topic Get(int id)
        {
              return null;
        }

        public List<Topic> GetByUser(string userName)
        {
            return null;
        }
    }
}