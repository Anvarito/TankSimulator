using System;
using System.Collections.Generic;
using Infrastructure.Factory.Base;

namespace Infrastructure.Factory.Compose
{
    public class Factories : IFactories
    {
        public Dictionary<Type, IGameFactory> All { get; } = new Dictionary<Type, IGameFactory>();

        public void Add<TFactory>(TFactory factory) where TFactory : IGameFactory =>
            All.Add(typeof(TFactory), factory);

        public void CleanUp()
        {
        }

        public TFactory Single<TFactory>() where TFactory : class, IGameFactory =>
            All[typeof(TFactory)] as TFactory;
    }
}