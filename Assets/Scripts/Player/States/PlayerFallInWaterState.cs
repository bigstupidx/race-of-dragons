using UnityEngine;
using System.Collections;

public class PlayerFallInWaterState : IState<PlayerController>
{
    private Rigidbody2D body;
    private float timer;
    private float maxTimeToReset = 3;

    public PlayerFallInWaterState() : base()
    {

    }

    public override void Begin()
    {
        context.playerState = PlayerState.FallInWater;
        body = context.body;
        body.velocity = body.velocity.normalized;
        body.gravityScale = 0.7f;
    }

    public override void Reason()
    {
        
    }

    public override void Update(float deltaTime)
    {
        timer += deltaTime;
        if (timer >= maxTimeToReset)
        {
            context.ResetPosition();
            timer = 0;
        }
    }

    public override void End()
    {

    }
}
