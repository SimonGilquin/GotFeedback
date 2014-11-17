using System;
using Microsoft.Practices.Unity;

namespace GotFeedback.Core
{
    public class UnityContainerAdapter : IDependencyContainer, IDisposable
    {
        private readonly IUnityContainer _container;

        public UnityContainerAdapter()
        {
            _container = new UnityContainer();
        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        public T Resolve<T>(Type type, string name) where T : class
        {
            return _container.Resolve(type, name) as T;
        }

        public IUnityContainer Register<T>(T instance) where T : class
        {
            return _container.RegisterInstance(typeof(T), instance);
        }

        public IUnityContainer RegisterType<T, T1>(LifetimeManager lifetimeManager)
            where T : class
            where T1 : T
        {
            return _container.RegisterType<T, T1>(lifetimeManager);
        }

        public IUnityContainer RegisterType<T, T1>(LifetimeManager lifetimeManager, params  InjectionMember[] injectionMembers)
            where T : class
            where T1 : T
        {
            return _container.RegisterType<T, T1>(lifetimeManager, injectionMembers);
        }

        public IUnityContainer RegisterType<T, T1>()
            where T : class
            where T1 : T
        {
            return _container.RegisterType<T, T1>();
        }
        public void Dispose()
        {
            _container.Dispose();
        }
    }
}