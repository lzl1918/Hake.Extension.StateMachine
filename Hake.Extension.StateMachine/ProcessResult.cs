namespace Hake.Extension.StateMachine
{
    public class InvokeResult<TState>
    {
        public InvokeResult(int startPosition, TState startState, int endPosition, TState endState)
        {
            StartPosition = startPosition;
            StartState = startState;
            EndPosition = endPosition;
            EndState = endState;
        }

        public int StartPosition { get; }
        public TState StartState { get; }
        public int EndPosition { get; }
        public TState EndState { get; }

    }

}
