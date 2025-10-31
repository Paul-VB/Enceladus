using System.Diagnostics.CodeAnalysis;

namespace Enceladus.Core
{
    public class ScheduledAction 
    {
        [SetsRequiredMembers]
        public ScheduledAction(Action action)
        {
           Action = () => { if (!IsCancelled) action.Invoke(); };
        }

        public required Action Action;
        public bool IsCancelled { get; set; } = false;

        public void Invoke() => Action.Invoke();
        public void Cancel() => IsCancelled = true;
    }
}
