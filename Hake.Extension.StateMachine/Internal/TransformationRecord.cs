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
        private IStateMachine<TState, TInput> stateMachine;

        public TransformationRecord(IStateMachine<TState, TInput> stateMachine, TransformationBuilder<TState, TInput> builder)
        {
            State = builder.ConfiguringState;
            transformations = builder.Transformations;
            this.stateMachine = stateMachine;
        }

        public bool Transform(int position, TInput input, IEnumerable<TInput> inputs, StateMachine.TriggerType triggerType, out TState newState, out FollowingAction followingAction, out object shiftContext, out object stateMapper)
        {
            object context = null;
            object mapper = null;
            foreach (TriggerRecord<TState, TInput> triggerRecord in transformations)
            {
                switch (triggerRecord.Type)
                {
                    case TriggerType.Trigger:
                        if (input.Equals(triggerRecord.Trigger))
                        {
                            newState = triggerRecord.NewState;
                            followingAction = FireTriggeringAction(triggerRecord, newState);
                            shiftContext = context;
                            stateMapper = mapper;
                            return true;
                        }
                        break;
                    case TriggerType.Condition:
                        if (triggerRecord.Condition(State, input))
                        {
                            newState = triggerRecord.NewState;
                            followingAction = FireTriggeringAction(triggerRecord, newState);
                            shiftContext = context;
                            stateMapper = mapper;
                            return true;
                        }
                        break;
                    case TriggerType.Always:
                        newState = triggerRecord.NewState;
                        followingAction = FireTriggeringAction(triggerRecord, newState);
                        shiftContext = context;
                        stateMapper = mapper;
                        return true;

                    case TriggerType.AlwaysWithEvaluator:
                        newState = triggerRecord.Evaluator(State, input);
                        followingAction = FireTriggeringAction(triggerRecord, newState);
                        shiftContext = context;
                        stateMapper = mapper;
                        return true;
                }
            }
            newState = default(TState);
            followingAction = FollowingAction.Continue;
            shiftContext = context;
            stateMapper = mapper;
            return false;

            FollowingAction FireTriggeringAction(TriggerRecord<TState, TInput> triggerRecord, TState newstate)
            {
                TriggeringArguments<TState, TInput> arg = new TriggeringArguments<TState, TInput>(stateMachine, inputs, State, newstate, input, position, triggerType);
                triggerRecord.TriggeringAction?.Invoke(arg);
                if (!arg.Handled)
                    arg.FollowingAction = FollowingAction.Continue;
                if (arg.FollowingAction == FollowingAction.Shift)
                {
                    context = arg.ShiftContext;
                    mapper = arg.StateMapper;
                }
                return arg.FollowingAction;
            }
        }
    }
}
