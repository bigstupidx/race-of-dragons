using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : IState<PlayerController>
{
    private Rigidbody2D body;
    private Vector2 vel;

    public PlayerRunState() : base ()
    {

    }

    public override void Begin()
    {
        context.playerState = PlayerState.Running;
        body = context.body;
        //body.velocity = new Vector2(5, 0);
    }

    public override void Reason()
    {
        if (Input.GetMouseButton(0))
        {
            machine.ChangeState<PlayerFlyState>();
        }
    }

    public override void Update(float deltaTime)
    {

    }

    public override void End()
    {

    }
}
