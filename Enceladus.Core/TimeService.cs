namespace Enceladus.Core
{
    public interface ITimeService
    {
        float GameTime { get; }
        void Update(float deltaTime);
        ScheduledAction ScheduleAction(float delaySeconds, Action action);
    }

    public class TimeService : ITimeService
    {
        private float _gameTime = 0f;
        private readonly PriorityQueue<ScheduledAction, float> _scheduledActions = new();

        //seconds since the game started
        public float GameTime => _gameTime;

        public void Update(float deltaTime)
        {
            _gameTime += deltaTime;

            TriggerScheduledActions();
        }

        private void TriggerScheduledActions()
        {
            // Process scheduled actions (only check earliest action)

            while (NextActionIsReady())
            {
                var action = _scheduledActions.Dequeue();
                action.Invoke();
            }
        }

        private bool NextActionIsReady() => _scheduledActions.TryPeek(out _, out float triggerTime) && _gameTime >= triggerTime;

        public ScheduledAction ScheduleAction(float delaySeconds, Action action)
        {
            var scheduledAction = new ScheduledAction(action);
            var triggerTime = _gameTime + delaySeconds;

            _scheduledActions.Enqueue(scheduledAction, triggerTime);

            return scheduledAction;
        }
    }
}
