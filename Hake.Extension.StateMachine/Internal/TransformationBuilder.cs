using System;
using System.Collections.Generic;
using System.Text;

namespace Hake.Extension.StateMachine.Internal
{
    internal sealed class TransformationBuilder<TState, TInput> : ITransformationBuilder<TState, TInput>
    {
        private List<TriggerRecord<TState, TInput>> transformations;
        public IReadOnlyList<TriggerRecord<TState, TInput>> Transformations => transformations;

        public TState ConfiguringState { get; }

        public TransformationBuilder(TState configuringState)
        {
            transformations = new List<TriggerRecord<TState, TInput>>();
            ConfiguringState = configuringState;
        }


        public ITransformationBuilder<TState, TInput> OnCondition(TState newState, TriggerCondition<TState, TInput> condition, TriggeringAction<TState, TInput> triggeringAction)
        {
            transformations.Add(new TriggerRecord<TState, TInput>(newState, condition, triggeringAction));
            return this;
        }

        public ITransformationBuilder<TState, TInput> OnValue(TState newState, TInput triggerValue, TriggeringAction<TState, TInput> triggeringAction)
        {
            transformations.Add(new TriggerRecord<TState, TInput>(newState, triggerValue, triggeringAction));
            return this;
        }

        public ITransformationBuilder<TState, TInput> OnAlways(TState newState, TriggeringAction<TState, TInput> triggeringAction)
        {
            transformations.Add(new TriggerRecord<TState, TInput>(newState, triggeringAction));
            return this;
        }

        public ITransformationBuilder<TState, TInput> OnAlways(StateEvaluator<TState, TInput> stateEvaluator, TriggeringAction<TState, TInput> triggeringAction)
        {
            transformations.Add(new TriggerRecord<TState, TInput>(stateEvaluator, triggeringAction));
            return this;
        }

        public TransformationRecord<TState, TInput> Build() => new TransformationRecord<TState, TInput>(this);
    }
}
