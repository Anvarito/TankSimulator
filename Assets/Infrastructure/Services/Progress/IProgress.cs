using Infrastructure.Data;

namespace Infrastructure.Services.Progress
{
    public interface IProgressReader
    {
        void LoadProgress(PlayerProgress progress);
    }
    
    public interface IProgressWriter : IProgressReader
    {
        void UpdateProgress(PlayerProgress progress);
    }
}