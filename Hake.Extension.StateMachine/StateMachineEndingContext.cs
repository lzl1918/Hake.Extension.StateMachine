using System;
using System.Collections.Generic;

namespace Hake.Extension.StateMachine
{
    public sealed class StateMachineEndingContext<TState, TInput>
    {
        public IStateMachine<TState, TInput> StateMachine { get; }
        public TState State { get; }
        public bool Handled { get; private set; }

        internal StateMachineEndingAction Action { get; private set; }
        internal IReadOnlyList<TInput> FeededInputs { get; private set; }

        internal StateMachineEndingContext(IStateMachine<TState, TInput> stateMachine)
        {
            StateMachine = stateMachine;
            State = StateMachine.State;
            Handled = false;
            Action = StateMachineEndingAction.End;
        }

        public void FeedInputs(IReadOnlyList<TInput> inputs)
        {
            if (inputs == null)
                throw new ArgumentNullException(nameof(inputs));
            if (inputs.Count <= 0)
                throw new Exception("given input set is empty");

            FeededInputs = inputs;
            Action = StateMachineEndingAction.ContinueWithFeededInputs;
            Handled = true;
        }
        public void MarkEnd()
        {
            FeededInputs = null;
            Action = StateMachineEndingAction.End;
            Handled = true;
        }
    }
}
