using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class StateMachine
{
    Stack<IState> _stack;

    public StateMachine()
    {
        _stack = new Stack<IState>();
    }

    public void PushState(IState state)
    {
        if (CurrentState != state)
        {
            if (CurrentState != null)
                CurrentState.OnLeaveState();

            _stack.Push(state);
            state.OnEnterState();
        }
    }

    public void PopState()
    {
        _stack.Pop().OnLeaveState();

        if (CurrentState != null)
            CurrentState.OnEnterState();
    }

    public void SetState(IState state)
    {
        while(_stack.Count > 0)
        {
            _stack.Pop().OnLeaveState();
        }
        _stack.Push(state);
        CurrentState.OnEnterState();
    }

    public IState CurrentState
    {
        get
        {
            if (_stack.Count > 0)
                return _stack.Peek();
            else
                return null;
        }
    }
}

public interface IState
{
    void OnEnterState();
    void OnLeaveState();
}
