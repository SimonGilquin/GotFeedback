using System;

namespace GotFeedback.Core
{
    public class DependencyContainer
    {
        private static IDependencyContainer Container { get; set; }
        public static void Set(IDependencyContainer container)
        {
            Container = container;
        }

        public static IDependencyContainer Get()
        {
            if (Container == null)
                throw new NullReferenceException("DependencyContainer is null!");
            return Container;
        } 
    }
}