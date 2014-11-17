using GotFeedback.Core;

namespace GotFeedback
{
    public static class Bootstrapper
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

            DependencyContainer.Set(uca);
        }
    }
}