namespace Hake.Extension.StateMachine
{
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

        public static ITransformationBuilder<TState, TInput> OnConditionKeep<TState, TInput>(this ITransformationBuilder<TState, TInput> builder, TriggerCondition<TState, TInput> condition, TriggeringAction<TState, TInput> triggeringAction)
            => builder.OnCondition(condition, builder.ConfiguringState, triggeringAction);
        public static ITransformationBuilder<TState, TInput> OnConditionKeep<TState, TInput>(this ITransformationBuilder<TState, TInput> builder, TriggerCondition<TState, TInput> condition)
            => builder.OnCondition(condition, builder.ConfiguringState, null);

        public static ITransformationBuilder<TState, TInput> OnValueKeep<TState, TInput>(this ITransformationBuilder<TState, TInput> builder, TInput triggerValue, TriggeringAction<TState, TInput> triggeringAction)
            => builder.OnValue(triggerValue, builder.ConfiguringState, triggeringAction);
        public static ITransformationBuilder<TState, TInput> OnValueKeep<TState, TInput>(this ITransformationBuilder<TState, TInput> builder, TInput triggerValue)
            => builder.OnValue(triggerValue, builder.ConfiguringState, null);

        public static ITransformationBuilder<TState, TInput> OnAlwaysKeep<TState, TInput>(this ITransformationBuilder<TState, TInput> builder, TriggeringAction<TState, TInput> triggeringAction)
            => builder.OnAlways(builder.ConfiguringState, triggeringAction);
        public static ITransformationBuilder<TState, TInput> OnAlwaysKeep<TState, TInput>(this ITransformationBuilder<TState, TInput> builder)
            => builder.OnAlways(builder.ConfiguringState, null);
    }

}
