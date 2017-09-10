using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hake.Extension.StateMachine.Internal
{
    internal enum TriggerType
    {
        Trigger,
        Condition,
        Always,
        AlwaysWithEvaluator
    }

    internal sealed class TriggerRecord<TState, TInput>
    {
        public TriggerType Type { get; }
        public TState NewState { get; }
        public TInput Trigger { get; }
        public TriggerCondition<TState, TInput> Condition { get; }
        public StateEvaluator<TState, TInput> Evaluator { get; }
        public TriggeringAction<TState, TInput> TriggeringAction { get; }

        public TriggerRecord(TState newState, TInput trigger, TriggeringAction<TState, TInput> triggeringAction)
            : this(TriggerType.Trigger, newState, trigger, null, null, triggeringAction) { }
        public TriggerRecord(TState newState, TriggerCondition<TState, TInput> condition, TriggeringAction<TState, TInput> triggeringAction)
            : this(TriggerType.Condition, newState, default(TInput), condition, null, triggeringAction) { }
        public TriggerRecord(TState newState, TriggeringAction<TState, TInput> triggeringAction)
            : this(TriggerType.Always, newState, default(TInput), null, null, triggeringAction) { }
        public TriggerRecord(StateEvaluator<TState, TInput> evaluator, TriggeringAction<TState, TInput> triggeringAction)
            : this(TriggerType.AlwaysWithEvaluator, default(TState), default(TInput), null, evaluator, triggeringAction) { }

        private TriggerRecord(TriggerType type, TState newState, TInput trigger, TriggerCondition<TState, TInput> condition, StateEvaluator<TState, TInput> evaluator, TriggeringAction<TState, TInput> triggeringAction)
        {
            Type = type;
            NewState = newState;
            Trigger = trigger;
            Condition = condition;
            Evaluator = evaluator;
            TriggeringAction = triggeringAction;
        }
    }

    internal sealed class TransformationRecord<TState, TInput>
    {
        public TState State { get; }

        private IReadOnlyList<TriggerRecord<TState, TInput>> transformations;

        public TransformationRecord(TransformationBuilder<TState, TInput> builder)
        {
            State = builder.ConfiguringState;
            transformations = builder.Transformations;
        }

        public bool Transform(TInput input, out TState newState)
        {
            foreach (TriggerRecord<TState, TInput> triggerRecord in transformations)
            {
                switch (triggerRecord.Type)
                {
                    case TriggerType.Trigger:
                        if (input.Equals(triggerRecord.Trigger))
                        {
                            newState = triggerRecord.NewState;
                            triggerRecord.TriggeringAction?.Invoke(State, newState, input);
                            return true;
                        }
                        break;
                    case TriggerType.Condition:
                        if (triggerRecord.Condition(State, input))
                        {
                            newState = triggerRecord.NewState;
                            triggerRecord.TriggeringAction?.Invoke(State, newState, input);
                            return true;
                        }
                        break;
                    case TriggerType.Always:
                        newState = triggerRecord.NewState;
                        triggerRecord.TriggeringAction?.Invoke(State, newState, input);
                        return true;

                    case TriggerType.AlwaysWithEvaluator:
                        newState = triggerRecord.Evaluator(State, input);
                        triggerRecord.TriggeringAction?.Invoke(State, newState, input);
                        return true;
                }
            }
            newState = default(TState);
            return false;
        }
    }
}
