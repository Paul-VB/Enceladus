namespace Enceladus.Core
{
    public interface IScheduledActionService
    {
        ScheduledAction ScheduleAction(float delaySeconds, Action action, ActionFailureSeverity failureSeverity = ActionFailureSeverity.Log);
        void TriggerScheduledActions();
    }

    public class ScheduledActionService : IScheduledActionService
    {
        private readonly PriorityQueue<ScheduledAction, float> _scheduledActions = new();
        private readonly ITimeService _timeService;

        public ScheduledActionService(ITimeService timeService)
        {
            _timeService = timeService;
        }

        public ScheduledAction ScheduleAction(float delaySeconds, Action action, ActionFailureSeverity failureSeverity = ActionFailureSeverity.Log)
        {
            var scheduledAction = new ScheduledAction(action)
            {
                FailureSeverity = failureSeverity
            };
            var triggerTime = _timeService.GameTime + delaySeconds;

            _scheduledActions.Enqueue(scheduledAction, triggerTime);

            return scheduledAction;
        }

        public void TriggerScheduledActions()
        {
            while (NextActionIsReady())
            {
                var action = _scheduledActions.Dequeue();
                Execute(action);
            }
        }

        private bool NextActionIsReady() =>
            _scheduledActions.TryPeek(out _, out float triggerTime) && _timeService.GameTime >= triggerTime;

        private void Execute(ScheduledAction scheduledAction)
        {
            if (scheduledAction.IsCancelled) return;

            try
            {
                scheduledAction.Action.Invoke();
            }
            catch (Exception ex)
            {
                switch (scheduledAction.FailureSeverity)
                {
                    case ActionFailureSeverity.Silent:
                        break;
                    case ActionFailureSeverity.Log:
                        Console.WriteLine($"Scheduled action failed: {ex}");
                        break;
                    case ActionFailureSeverity.Throw:
                        throw;
                }
            }
        }
    }
}
