using UnityEngine;
using System.Collections;

public class PlayerBurningState : IState<PlayerController>
{
    private Rigidbody2D body;
    private float speedDegree = 1.5f;
    private float timer;
    private float waitTime = 2;
    private bool canControl;
    public PlayerBurningState() : base()
    {

    }

    public override void Begin()
    {
        context.playerState = PlayerState.Burning;
        body = context.body;
        body.gravityScale = 1f;
        canControl = false;
    }

    public override void Reason()
    {
        if (Input.GetMouseButton(0) && canControl == true)
        {
            machine.ChangeState<PlayerFlyState>();
        }
    }

    public override void Update(float deltaTime)
    {
        timer += deltaTime;
        if (timer > waitTime)
        {
            canControl = true;
            timer = 0;
        }

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
