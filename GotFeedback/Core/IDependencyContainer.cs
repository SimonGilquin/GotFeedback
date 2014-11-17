using System;
using Microsoft.Practices.Unity;

namespace GotFeedback.Core
{
    public interface IDependencyContainer
    {
        T Resolve<T>();
        T Resolve<T>(Type type, string name) where T : class;

        IUnityContainer Register<T>(T instance) where T : class;
        IUnityContainer RegisterType<T, T1>(LifetimeManager lifetimeManager)
            where T : class
            where T1 : T;

        IUnityContainer RegisterType<T, T1>(LifetimeManager lifetimeManager,
                                            params InjectionMember[] injectionMembers)
            where T : class
            where T1 : T;

        IUnityContainer RegisterType<T, T1>()
            where T : class
            where T1 : T; 
    }
}