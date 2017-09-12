using System;
using System.Collections.Generic;
using System.Text;

namespace Hake.Extension.StateMachine
{
    public class TriggeringArguments<TState, TInput>
    {
        internal TriggeringArguments(TState oldState, TState newState, TInput input, int inputPosition)
        {
            OldState = oldState;
            NewState = newState;
            Input = input;
            InputPosition = inputPosition;
            Handled = false;
            FollowingAction = FollowingAction.Continue;
        }

        public TState OldState { get; }
        public TState NewState { get; }
        public TInput Input { get; }
        public int InputPosition { get; }

        public FollowingAction FollowingAction { get; set; }
        public bool Handled { get; set; }


    }

}
