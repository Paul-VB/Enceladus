namespace Enceladus.Core.Time
{
    public interface ITimeService
    {
        float GameTime { get; }
        void Update(float deltaTime);
    }

    public class TimeService : ITimeService
    {
        private float _gameTime = 0f;

        //seconds since the game started
        public float GameTime => _gameTime;

        public void Update(float deltaTime)
        {
            _gameTime += deltaTime;
        }
    }
}
