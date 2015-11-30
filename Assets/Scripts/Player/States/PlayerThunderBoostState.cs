using UnityEngine;
using System.Collections;

public class PlayerThunderBoostState : IState<PlayerController>
{

    private Rigidbody2D body;
    private Vector2 vel;
    private float timer;
    private float maxForce = 50;
    public PlayerThunderBoostState() : base ()
    {

    }

    public override void Begin()
    {
        context.speedAngle = 13;   
        body = context.body;
        
        float alpha = GameUtils.Instance.DegreeToRadian(context.transform.rotation.eulerAngles.z);
        Vector2 force = new Vector2(maxForce * Mathf.Cos(alpha), maxForce * Mathf.Sin(alpha));
        body.velocity = force;

        timer = 0;
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
        timer += deltaTime;
        if (timer >= 1)
        {
            body.velocity = body.velocity * 0.5f;            
            timer = 0;
            machine.ChangeState<PlayerFlyState>();            
        }
    }

    public override void End()
    {

    }
}
