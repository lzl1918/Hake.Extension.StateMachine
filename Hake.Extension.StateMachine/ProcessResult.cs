namespace Hake.Extension.StateMachine
{
    public class ProcessResult<TState>
    {
        public ProcessResult(int position, TState state)
        {
            Position = position;
            State = state;
        }

        public int Position { get; }
        public TState State { get; }

    }

}
