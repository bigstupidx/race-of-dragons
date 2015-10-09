using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public sealed class StateMachine<T>
{
    protected T context;
#pragma warning disable
    public event Action onStateChanged;
#pragma warning restore

    private IState<T> currentState;
    private IState<T> previousState;
    private Dictionary<System.Type, IState<T>> dicStates = new Dictionary<Type, IState<T>>();

    public IState<T> CurrentState
    {
        get
        {
            return currentState;
        }
    }

    public IState<T> PreviousState
    {
        get
        {
            return previousState;
        }
    }

    public float elapsedTimeInState = 0f;

    public StateMachine(T context, IState<T> initialState)
    {
        this.context = context;

        AddState(initialState);
        currentState = initialState;
        currentState.begin();
    }

    public void AddState(IState<T> state)
    {
        state.setMachineAndContext(this, context);
        dicStates[state.GetType()] = state;
    }

    public void update(float deltaTime)
    {
        elapsedTimeInState += deltaTime;
        currentState.reason();
        currentState.update(deltaTime);
    }

    public R ChangeState<R>() where R : IState<T>
    {
        // avoid changing to the same state
        var newType = typeof(R);
        if (currentState.GetType() == newType)
            return currentState as R;

        if (currentState != null)
            currentState.end();

#if UNITY_EDITOR
        // do a sanity check while in the editor to ensure we have the given state in our state list
        if (!dicStates.ContainsKey(newType))
        {
            var error = GetType() + ": state " + newType + " does not exist. Did you forget to add it by calling addState?";
            Debug.LogError(error);
            throw new Exception(error);
        }
#endif

        previousState = currentState;
        currentState = dicStates[newType];
        currentState.begin();
        elapsedTimeInState = 0.0f;

        if (onStateChanged != null)
            onStateChanged();

        return currentState as R;
    }
}
