namespace Infrastructure.Services.Score
{
    public interface IScoreCounter : IService
    {
        public float Score { get; }

        public void LoadData();
    }
}