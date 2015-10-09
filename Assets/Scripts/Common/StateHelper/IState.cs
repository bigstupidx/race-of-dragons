using UnityEngine;
using System.Collections;

public abstract class IState<T>
{
    protected StateMachine<T> machine;
    protected T context;

    public IState() { }

    internal void setMachineAndContext(StateMachine<T> machine, T context)
    {
        this.machine = machine;
        this.context = context;
        onInitialized();
    }

    public virtual void onInitialized()
    { }

    public virtual void begin()
    { }

    public virtual void reason()
    { }

    public abstract void update(float deltaTime);

    public virtual void end()
    { }
}
