using Infrastructure.Services.StaticData.Level;
using Infrastructure.Services.StaticData.Tank;

namespace Infrastructure.Services.StaticData
{
    public interface IStaticDataService : IService
    {
        void LoadAllStaticData();
        TankConfig ForTank(TankId id);
        LevelConfig ForLevel(LevelId id);
    }
}