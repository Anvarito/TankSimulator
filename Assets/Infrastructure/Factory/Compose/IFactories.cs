using System;
using System.Collections.Generic;
using Infrastructure.Factory.Base;
using Infrastructure.Services;

namespace Infrastructure.Factory.Compose
{
    public interface IFactories : IService
    {
        Dictionary<Type, IGameFactory> All { get; }
        void Add<TFactory>(TFactory factory) where TFactory : IGameFactory;
        TFactory Single<TFactory>() where TFactory : class, IGameFactory;
    }
}