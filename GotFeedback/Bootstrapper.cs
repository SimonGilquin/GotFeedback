using GotFeedback.Core;
using GotFeedback.Services;

namespace GotFeedback
{
    public class Bootstrapper
    {
              private static readonly object Obj = new object();
        private static bool IsInitialized { get; set; }

        public static void Initialize()
        {
            lock (Obj)
            {
                if (IsInitialized) return;
                IsInitialized = true;
            }

            var uca = new UnityContainerAdapter();
            uca.RegisterType<IUserService, UserService>();
            uca.RegisterType<IGotFeedbackService, GotFeedbackService>();

            DependencyContainer.Set(uca);
        }
    }
}