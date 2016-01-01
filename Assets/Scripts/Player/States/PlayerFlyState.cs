using UnityEngine;
using System.Collections;
using System;

public class PlayerFlyState : IState<PlayerController>
{  
    private float speedAlpha = 180;
    private Rigidbody2D body;
    private Vector2 vel;
    private float alpha;
    private float timer;

    public PlayerFlyState() : base()
    {
        
    }

    private Vector2 CalculateForce(float speed, float a)
    {
        a = GameUtils.Instance.DegreeToRadian(a);
        Vector2 result = new Vector2();
        result.x = speed * Mathf.Cos(a);
        result.y = speed * Mathf.Sin(a);

        return result;
    }

    public override void Begin()
    {
        timer = 0;
        SoundManager.Instance.playSound(ESound.Fly);
        context.textName.text = "";
        context.playerState = PlayerState.Flying;
        body = context.body;
        body.gravityScale = 0;
        context.animator.SetBool("isFlying", true);       
    }

    public override void Reason()
    {
        if (!Input.GetMouseButton(0))
        {
            machine.ChangeState<PlayerFallState>();
        }
    }

    public override void Update(float deltaTime)
    {
        if (body.velocity.normalized.x != -1)
            alpha = GameUtils.Instance.CalculateAlpha(body.velocity.x, body.velocity.y);
        else
            alpha = 0;

        vel = CalculateForce(context.speedAngle, alpha + speedAlpha * deltaTime);
        body.velocity = vel;
        context.speedAngle += 2 * deltaTime;

        timer += deltaTime;
        if (timer >= 2)
        {
            timer = 0;
            SoundManager.Instance.playSound(ESound.Eagle);
        }  
    }

    public override void End()
    {
        
    }
}
