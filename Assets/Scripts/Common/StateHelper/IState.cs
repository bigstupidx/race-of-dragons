using UnityEngine;
using System.Collections;

public abstract class IState<T>
{
    protected StateMachine<T> machine;
    protected T context;

    public IState() { }

    internal void SetMachineAndContext(StateMachine<T> machine, T context)
    {
        this.machine = machine;
        this.context = context;
        onInitialized();
    }

    public virtual void onInitialized()
    { }

    public virtual void Begin()
    { }

    public virtual void Reason()
    { }

    public abstract void Update(float deltaTime);

    public virtual void End()
    { }
}
