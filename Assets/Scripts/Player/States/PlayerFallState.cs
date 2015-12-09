using UnityEngine;
using System.Collections;
using System;

public class PlayerFallState : IState<PlayerController>
{
    private Rigidbody2D body;
    private float speedDegree = 1.5f;
    public PlayerFallState() : base()
    {

    }

    public override void Begin()
    {
        context.playerState = PlayerState.Failing;
        body = context.body;       
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
            context.speedAngle -= deltaTime * speedDegree;
        }
    }

    public override void End()
    {

    }
}
