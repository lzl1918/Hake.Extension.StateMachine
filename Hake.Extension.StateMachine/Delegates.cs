using System;
using System.Collections.Generic;
using System.Text;

namespace Hake.Extension.StateMachine
{
    public delegate bool TriggerCondition<TState, TInput>(TState currentState, TInput input);
    public delegate TState StateEvaluator<TState, TInput>(TState currentState, TInput input);
    public delegate void TriggeringAction<TState, TInput>(TState oldState, TState newState, TInput input);

}
