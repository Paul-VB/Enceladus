namespace Enceladus.Core
{
    public enum ActionFailureSeverity
    {
        Silent,  // Swallow exception, continue execution
        Log,     // Log exception to console, continue execution
        Throw    // Rethrow exception (will crash game loop if unhandled)
    }
}
