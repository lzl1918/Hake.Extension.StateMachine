using System;

namespace Hake.Extension.StateMachine
{
    public class ShiftResult<TSubState, TState, TInput>
    {
        internal ShiftResult(object data, Exception exception, IStateMachine<TSubState, TInput> processor, TSubState newState, TState oldState, Func<TSubState, TState> stateMapper, int startPosition, int endPosition)
        {
            Data = data;
            Exception = exception;
            Processor = processor;
            NewState = newState;
            OldState = oldState;
            StateMapper = stateMapper;
            StartPosition = startPosition;
            EndPosition = endPosition;
        }

        public object Data { get; }
        public Exception Exception { get; }
        public IStateMachine<TSubState, TInput> Processor { get; }
        public TSubState NewState { get; }
        public TState OldState { get; }
        public Func<TSubState, TState> StateMapper { get; }
        public int StartPosition { get; }
        public int EndPosition { get; }

        public bool IsSucceeded => Exception == null;

        // for reflection usage
        internal static ShiftResult<TSubState, TState, TInput> Create(object data, Exception exception, IStateMachine<TSubState, TInput> processor, TSubState newState, TState oldState, Func<TSubState, TState> stateMapper, int startPosition, int endPosition)
        {
            return new ShiftResult<TSubState, TState, TInput>(data, exception, processor, newState, oldState, stateMapper, startPosition, endPosition);
        }
    }

}
