using System;
using System.Collections.Generic;

namespace Infrastructure.Services
{
    public class ServiceLocator
    {
        private readonly Dictionary<Type, IService> _services;
        private static ServiceLocator _instance;

        public static ServiceLocator Container => _instance ??= new ServiceLocator();

        private ServiceLocator()
        {
            _services = new Dictionary<Type, IService>();
        }

        public void RegisterSingle<TService>(TService service) where TService : IService
        {
            _services.Add(typeof(TService), service);
        }

        public TService Single<TService>() where TService : class, IService
        {
            return _services[typeof(TService)] as TService;
        }
    }
}