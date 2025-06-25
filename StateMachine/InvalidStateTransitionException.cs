namespace StateMachine
{
    /// <summary>
    /// Exception thrown when trying to execute a step that cannot be executed due to invalid state
    /// </summary>
    public class InvalidStateTransitionException : Exception
    {
        public InvalidStateTransitionException(string message) : base(message)
        {
        }

        public InvalidStateTransitionException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
