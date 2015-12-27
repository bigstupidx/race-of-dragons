using UnityEngine;
using System.Collections;
using Prime31.ZestKit;

public class PlayerFinishRaceState : IState<PlayerController>
{
    private Rigidbody2D body;
    private float timer;
    private float maxTimeToReset = 3;

    public PlayerFinishRaceState() : base()
    {

    }

    public override void Begin()
    {
        context.playerState = PlayerState.FinishRaceState;
        body = context.body;
        body.velocity = Vector2.zero;
        body.gravityScale = 0;

        Vector3 newPos = context.victoryPos + (4 - context.EndPosition + 1) * Vector3.right * 3;
        context.transform.ZKpositionTo(newPos, 1.5f).start();
    }

    public override void Reason()
    {

    }

    public override void Update(float deltaTime)
    {

    }

    public override void End()
    {

    }
}
