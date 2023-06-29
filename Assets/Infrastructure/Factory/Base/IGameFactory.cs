using System.Collections.Generic;
using Infrastructure.Services.Progress;

namespace Infrastructure.Factory.Base
{
    public interface IGameFactory : IFactory
    {
        List<IProgressWriter> ProgressWriters { get; }
        List<IProgressReader> ProgressReaders { get; }
        void CleanUp();
    }
}