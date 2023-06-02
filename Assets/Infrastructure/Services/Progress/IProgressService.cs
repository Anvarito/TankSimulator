using Infrastructure.Data;

namespace Infrastructure.Services.Progress
{
    public interface IProgressService : IService
    {
        PlayerProgress Progress { get; set; }
    }
}