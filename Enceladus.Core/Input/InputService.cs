namespace Enceladus.Core.Input
{
    public interface IInputService
    {
        void Update(float deltaTime);
    }
    public class InputService : IInputService
    {

        private readonly IControllableRegistry _controllableRegistry;
        private readonly IInputManager _inputReader;
        public InputService(IControllableRegistry controllableRegistry, IInputManager inputReader)
        {
            _controllableRegistry = controllableRegistry;
            _inputReader = inputReader;
        }


        public void Update(float deltaTime)
        {
            foreach(var controllable in _controllableRegistry.Controllables)
            {
                controllable.HandleInputs(deltaTime, _inputReader);
            }
        }
    }
}
