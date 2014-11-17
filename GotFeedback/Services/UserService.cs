using GotFeedback.Domain;

namespace GotFeedback.Services
{
    public interface IUserService
    {
        User Get(string userId);
        User GetByEmail(string email);
    }

    public class UserService : IUserService
    {
        public User Get(string userId)
        {
            return null;
        }

        public User GetByEmail(string email)
        {
            return null;
        }
    }
}