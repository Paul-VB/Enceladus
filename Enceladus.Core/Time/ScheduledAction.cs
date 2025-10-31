using System.Diagnostics.CodeAnalysis;

namespace Enceladus.Core.Time
{
    public class ScheduledAction
    {
        [SetsRequiredMembers]
        public ScheduledAction(Action action)
        {
            Action = action;
        }

        public required Action Action;
        public bool IsCancelled { get; set; } = false;
        public ActionFailureSeverity FailureSeverity { get; set; } = ActionFailureSeverity.Log;
        public void Cancel() => IsCancelled = true;
    }
}
