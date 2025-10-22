namespace Enceladus.Core.Input
{
    public interface IControllableRegistry
    {
        IReadOnlyList<IControllable> Controllables { get; }

        T Register<T>(T entity) where T : IControllable;
    }
    public class ControllableRegistry : IControllableRegistry
    {
        private readonly List<IControllable> _controllables = new();

        public IReadOnlyList<IControllable> Controllables => _controllables;

        public T Register<T>(T controllable) where T : IControllable
        {
            _controllables.Add(controllable);
            return controllable;
        }
    }
}
