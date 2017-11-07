using Hake.Extension.StateMachine.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hake.Extension.StateMachine
{
    public interface IStateMachine<TState, TInput>
    {
        TState State { get; }

        ITransformationBuilder<TState, TInput> Configure(TState state);
        IStateMachine<TState, TInput> OnStarting(Action<IStateMachine<TState, TInput>, TriggerType> action);
        IStateMachine<TState, TInput> OnEnding(Action<StateMachineEndingContext<TState, TInput>> context);

        InvokeResult<TState> Invoke(TState initialState, IEnumerable<TInput> inputs);
        InvokeResult<TState> InvokeProcess(TState initialState, IEnumerable<TInput> inputs, int position);
        TState InvokeOneShot(TState state, TInput input);
        TState InvokeOneShot(TInput input);
    }

}
