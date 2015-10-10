using UnityEngine;
using System.Collections;
using System;

public class PlayerFallState : IState<PlayerController>
{
    private Rigidbody2D body; 

    public PlayerFallState() : base()
    {

    }

    public override void Begin()
    {
        body = context.GetComponent<Rigidbody2D>();        
        body.gravityScale = 1f;
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
        if (body.velocity.x > 0)
            body.velocity = body.velocity - new Vector2(2 * Time.deltaTime, 0);
        if (context.speedAngle > GameConsts.Instance.PLAYER_SPEED_ANGLE_DEFAULT)
        {
            context.speedAngle -= deltaTime;
        }
    }

    public override void End()
    {

    }
}
