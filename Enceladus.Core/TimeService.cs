namespace Enceladus.Core
{
    public interface ITimeService
    {
        float GameTime { get; }
        void Update(float deltaTime);
    }

    public class TimeService : ITimeService
    {
        private float _gameTime = 0f;

        public float GameTime => _gameTime;

        public void Update(float deltaTime)
        {
            _gameTime += deltaTime;
        }
    }
}
