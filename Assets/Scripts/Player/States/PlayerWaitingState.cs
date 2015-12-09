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
        if (context.body != null)
        {
            context.body.gravityScale = 0;
            context.body.velocity = Vector2.zero;
        }
        
        if (context.animator != null)
            context.animator.Play("player_anim_idle", -1, 0);
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
