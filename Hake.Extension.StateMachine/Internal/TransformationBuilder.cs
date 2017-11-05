using System;
using System.Collections.Generic;
using System.Text;

namespace Hake.Extension.StateMachine.Internal
{
    internal sealed class TransformationBuilder<TState, TInput> : ITransformationBuilder<TState, TInput>
    {
        private List<TriggerRecord<TState, TInput>> transformations;
        private IStateMachine<TState, TInput> stateMachine;

        public IReadOnlyList<TriggerRecord<TState, TInput>> Transformations => transformations;

        public TState ConfiguringState { get; }

        public TransformationBuilder(IStateMachine<TState, TInput> stateMachine, TState configuringState)
        {
            transformations = new List<TriggerRecord<TState, TInput>>();
            this.stateMachine = stateMachine;
            ConfiguringState = configuringState;
        }


        public ITransformationBuilder<TState, TInput> OnCondition(TriggerCondition<TState, TInput> condition, TState newState, TriggeringAction<TState, TInput> triggeringAction)
        {
            transformations.Add(new TriggerRecord<TState, TInput>(newState, condition, triggeringAction));
            return this;
        }

        public ITransformationBuilder<TState, TInput> OnValue(TInput triggerValue, TState newState, TriggeringAction<TState, TInput> triggeringAction)
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

        public TransformationRecord<TState, TInput> Build() => new TransformationRecord<TState, TInput>(stateMachine, this);
    }
}
