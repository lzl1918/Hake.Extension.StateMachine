using System;
using System.Collections.Generic;

namespace Hake.Extension.StateMachine
{
    public class StateMachineEndingContext<TState, TInput>
    {
        public EndingReason Reason { get; }
        public IStateMachine<TState, TInput> StateMachine { get; }
        public TriggerType TriggerType { get; }
        public TState State { get; }
        public bool Handled { get; private set; }

        internal StateMachineEndingAction Action { get; private set; }
        internal IReadOnlyList<TInput> FeededInputs { get; private set; }

        internal StateMachineEndingContext(IStateMachine<TState, TInput> stateMachine, EndingReason reason, TriggerType triggerType)
        {
            StateMachine = stateMachine;
            Reason = reason;
            TriggerType = triggerType;
            State = StateMachine.State;
            Handled = false;
            Action = StateMachineEndingAction.End;
        }

        public void FeedInputs(IReadOnlyList<TInput> inputs)
        {
            if (Reason == EndingReason.EarlyStopped)
                throw new Exception("cannot continue executing of state machine stopped in advance");
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
