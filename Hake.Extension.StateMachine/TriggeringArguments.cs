using Hake.Extension.StateMachine.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hake.Extension.StateMachine
{
    public class TriggeringArguments<TState, TInput>
    {
        internal TriggeringArguments(IStateMachine<TState, TInput> stateMachine, IEnumerable<TInput> inputs, TState oldState, TState newState, TInput input, int inputPosition, TriggerType triggerType)
        {
            StateMachine = stateMachine;
            this.inputs = inputs;
            OldState = oldState;
            NewState = newState;
            Input = input;
            InputPosition = inputPosition;
            TriggerType = triggerType;
            Handled = false;
            FollowingAction = FollowingAction.Continue;
        }

        public IStateMachine<TState, TInput> StateMachine { get; }
        public TState OldState { get; }
        public TState NewState { get; }
        public TInput Input { get; }
        public int InputPosition { get; }
        public TriggerType TriggerType { get; }

        private FollowingAction followingAction;
        private IEnumerable<TInput> inputs;

        public FollowingAction FollowingAction
        {
            get => followingAction;
            set
            {
                if (value == FollowingAction.Shift)
                    throw new Exception("the shift action can only be set by calling ShiftProcess");

                Handled = true;
                followingAction = value;
            }
        }
        public bool Handled { get; private set; }

        internal object ShiftContext { get; private set; }
        internal object StateMapper { get; private set; }
        public void SetShift(IStateMachine<TState, TInput> processor, TState initialState)
        {
            if (processor == StateMachine)
                throw new Exception("cannot set the processor same as the current state machine");
            ShiftProcessorContext<TState, TInput> context = new ShiftProcessorContext<TState, TInput>(InputPosition + 1, inputs, initialState, processor);
            ShiftContext = context;
            StateMapper = null;
            Handled = true;
            followingAction = FollowingAction.Shift;
        }
        public void SetShift(IStateMachine<TState, TInput> processor, TState initialState, Func<TState, TState> stateMapper)
        {
            if (processor == StateMachine)
                throw new Exception("cannot set the processor same as the current state machine");
            ShiftProcessorContext<TState, TInput> context = new ShiftProcessorContext<TState, TInput>(InputPosition + 1, inputs, initialState, processor);
            ShiftContext = context;
            StateMapper = stateMapper;
            Handled = true;
            followingAction = FollowingAction.Shift;
        }
        public void SetShift<TSubState>(IStateMachine<TSubState, TInput> processor, TSubState initialState, Func<TSubState, TState> stateMapper)
        {
            if (processor == StateMachine)
                throw new Exception("cannot set the processor same as the current state machine");
            ShiftProcessorContext<TSubState, TInput> context = new ShiftProcessorContext<TSubState, TInput>(InputPosition + 1, inputs, initialState, processor);
            ShiftContext = context;
            StateMapper = stateMapper;
            Handled = true;
            followingAction = FollowingAction.Shift;
        }
    }

}
