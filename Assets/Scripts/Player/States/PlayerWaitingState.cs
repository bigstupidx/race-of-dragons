using UnityEngine;
using System.Collections;
using System;

public class PlayerWaitingState : IState<PlayerController>
{
    
    public PlayerWaitingState() : base ()
    {

    }

    public override void Begin()
    {

    }

    public override void Reason()
    {
        if (Input.GetMouseButtonDown(0))
            machine.ChangeState<PlayerFlyState>();
    }

    public override void Update(float deltaTime)
    {
        
    }

    public override void End()
    {
        
    }
}
