using Infrastructure.Data;

namespace Infrastructure.Services.Progress
{
    public class ProgressService : IProgressService
    {
        public PlayerProgress Progress { get; set; }

        public void CleanUp()
        {
            Progress.WorldData.StartedLevel = false;
        }
    }
}