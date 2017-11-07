using System.Collections.Generic;

namespace Hake.Extension.StateMachine.Internal
{
    internal class ShiftProcessorContext<TState, TInput>
    {
        public ShiftProcessorContext(int position, IEnumerable<TInput> inputs, TState initialState, IStateMachine<TState, TInput> processor)
        {
            Position = position;
            Inputs = inputs;
            InitialState = initialState;
            Processor = processor;
        }

        public int Position { get; }
        public IEnumerable<TInput> Inputs { get; }
        public TState InitialState { get; }
        public IStateMachine<TState, TInput> Processor { get; }

        public InvokeResult<TState> Invoke()
        {
            return Processor.InvokeProcess(InitialState, Inputs, Position);
        }
        
    }
}
