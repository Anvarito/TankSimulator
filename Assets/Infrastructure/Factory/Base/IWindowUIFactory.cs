using Infrastructure.Factory.Base;

namespace Infrastructure.Factory
{
    public interface IWindowUIFactory : IGameFactory
    {
        PauseMenu PauseMenu { get; }
        PauseMenu CreatePauseMenu();
        void DestroyPauseMenu();
    }
}