using Hake.Extension.StateMachine.Internal;
using System;

namespace Hake.Extension.StateMachine
{
    public interface ITransformationBuilder<TState, TInput>
    {
        TState ConfiguringState { get; }

        ITransformationBuilder<TState, TInput> OnCondition(TriggerCondition<TState, TInput> condition, TState newState, TriggeringAction<TState, TInput> triggeringAction);
        ITransformationBuilder<TState, TInput> OnValue(TInput triggerValue, TState newState, TriggeringAction<TState, TInput> triggeringAction);
        ITransformationBuilder<TState, TInput> OnAlways(TState newState, TriggeringAction<TState, TInput> triggeringAction);
        ITransformationBuilder<TState, TInput> OnAlways(StateEvaluator<TState, TInput> stateEvaluator, TriggeringAction<TState, TInput> triggeringAction);
    }

}
