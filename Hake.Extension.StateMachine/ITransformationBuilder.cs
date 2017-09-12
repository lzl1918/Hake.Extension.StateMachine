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

    public static class TransformationBuilderExtensions
    {
        public static ITransformationBuilder<TState, TInput> OnCondition<TState, TInput>(this ITransformationBuilder<TState, TInput> builder, TriggerCondition<TState, TInput> condition, TState newState)
            => builder.OnCondition(condition, newState, null);

        public static ITransformationBuilder<TState, TInput> OnValue<TState, TInput>(this ITransformationBuilder<TState, TInput> builder, TInput triggerValue, TState newState)
            => builder.OnValue(triggerValue, newState, null);

        public static ITransformationBuilder<TState, TInput> OnAlways<TState, TInput>(this ITransformationBuilder<TState, TInput> builder, TState newState)
            => builder.OnAlways(newState, null);

        public static ITransformationBuilder<TState, TInput> OnAlways<TState, TInput>(this ITransformationBuilder<TState, TInput> builder, StateEvaluator<TState, TInput> stateEvaluator)
            => builder.OnAlways(stateEvaluator, null);
    }

}
