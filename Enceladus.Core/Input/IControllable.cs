namespace Enceladus.Core.Input
{
    public interface IControllable
    {
        void HandleInputs(float deltaTime, IInputManager inputManager);
    }
}
