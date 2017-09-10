using Hake.Extension.StateMachine.Internal;
using System;
using System.Collections.Generic;

namespace Hake.Extension.StateMachine
{
    public sealed class StateMachine<TState, TInput> : IStateMachine<TState, TInput>
    {
        private TState state;
        public TState State => state;

        private IDictionary<TState, TransformationRecord<TState, TInput>> transformationTable;
        private IDictionary<TState, TransformationBuilder<TState, TInput>> transformationBuilders;
        private IList<Action<IStateMachine<TState, TInput>>> startingActions;
        private IList<Action<StateMachineEndingContext<TState, TInput>>> endingActions;
        private bool configurationLocked;

        public StateMachine()
        {
            state = default(TState);
            transformationBuilders = new Dictionary<TState, TransformationBuilder<TState, TInput>>();
            startingActions = new List<Action<IStateMachine<TState, TInput>>>();
            endingActions = new List<Action<StateMachineEndingContext<TState, TInput>>>();
            configurationLocked = false;
        }

        public ITransformationBuilder<TState, TInput> Configure(TState state)
        {
            TransformationBuilder<TState, TInput> builder;
            if (transformationBuilders.TryGetValue(state, out builder))
                return builder;
            builder = new TransformationBuilder<TState, TInput>(state);
            transformationBuilders[state] = builder;
            configurationLocked = false;
            return builder;
        }

        public TState Invoke(TState initialState, IEnumerable<TInput> inputs)
        {
            if (!configurationLocked)
                Rebuild();
            state = initialState;
            foreach (Action<IStateMachine<TState, TInput>> action in startingActions)
                action(this);

            TransformationRecord<TState, TInput> record;
            TState newState;
            foreach (TInput input in inputs)
            {
                if (transformationTable.TryGetValue(state, out record))
                {
                    if (record.Transform(input, out newState))
                        state = newState;
                    else
                        throw new Exception($"no transform information given.\r\ncurrent state: {state}\r\ninput: {input}");
                }
                else
                    throw new Exception($"no transform information given.\r\ncurrent state: {state}");
            }
            StateMachineEndingContext<TState, TInput> context;
            do
            {
                context = new StateMachineEndingContext<TState, TInput>(this);
                foreach (Action<StateMachineEndingContext<TState, TInput>> action in endingActions)
                    action.Invoke(context);
                if (context.Handled && context.Action == StateMachineEndingAction.ContinueWithFeededInputs)
                {
                    foreach (TInput input in context.FeededInputs)
                    {
                        if (transformationTable.TryGetValue(state, out record))
                        {
                            if (record.Transform(input, out newState))
                                state = newState;
                            else
                                throw new Exception($"no transform information given.\r\ncurrent state: {state}\r\ninput: {input}");
                        }
                        else
                            throw new Exception($"no transform information given.\r\ncurrent state: {state}");
                    }
                }
                else
                    break;
            } while (true);
            return state;
        }

        public TState InvokeOneShot(TState initialState, TInput input)
        {
            if (!configurationLocked)
                Rebuild();

            state = initialState;
            foreach (Action<IStateMachine<TState, TInput>> action in startingActions)
                action(this);
            TransformationRecord<TState, TInput> record;
            TState newState;
            if (transformationTable.TryGetValue(state, out record))
            {
                if (record.Transform(input, out newState))
                    state = newState;
                else
                    throw new Exception($"no transform information given.\r\ncurrent state: {state}\r\ninput: {input}");
            }
            else
                throw new Exception($"no transform information given.\r\ncurrent state: {state}");
            return state;
        }

        public TState InvokeOneShot(TInput input)
        {
            if (!configurationLocked)
                throw new Exception("initial state must be provided");

            TransformationRecord<TState, TInput> record;
            TState newState;
            if (transformationTable.TryGetValue(state, out record))
            {
                if (record.Transform(input, out newState))
                    state = newState;
                else
                    throw new Exception($"no transform information given.\r\ncurrent state: {state}\r\ninput: {input}");
            }
            else
                throw new Exception($"no transform information given.\r\ncurrent state: {state}");
            return state;
        }

        public IStateMachine<TState, TInput> OnEnding(Action<StateMachineEndingContext<TState, TInput>> context)
        {
            if (context != null)
                endingActions.Add(context);
            return this;
        }

        public IStateMachine<TState, TInput> OnStarting(Action<IStateMachine<TState, TInput>> action)
        {
            if (action != null)
                startingActions.Add(action);
            return this;
        }

        private void Rebuild()
        {
            transformationTable = new Dictionary<TState, TransformationRecord<TState, TInput>>();
            foreach (var pair in transformationBuilders)
                transformationTable[pair.Key] = pair.Value.Build();
            configurationLocked = true;
        }
    }

}
