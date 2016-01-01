using UnityEngine;
using System.Collections;

public class FireballController : Photon.PunBehaviour
{
    public Animator animator;
    public ParticleSystem particle;
    public RadarController radar;
    public float maxForce = 10;
    public int playerId = 0;

    private Rigidbody2D body;
    private Collider2D collider;
    private ConstantForce2D force;
    private float timer;
	
	void Start ()
    {
        body = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        force = GetComponent<ConstantForce2D>();

        float alpha = GameUtils.Instance.DegreeToRadian(transform.rotation.eulerAngles.z);
        force.force = new Vector2(maxForce * Mathf.Cos(alpha), maxForce * Mathf.Sin(alpha));

        body.velocity = force.force;

        // Network
        playerId = photonView.ownerId;
        radar.playerId = playerId;

        SoundManager.Instance.playSound(ESound.Fire);
    }
		
	void Update ()
    {
        timer += Time.deltaTime;
        if (timer >= 20)
        {
            Destroy(this.gameObject);
        }

	    if (radar.target != null)
        {
            var targetSpeed = radar.target.GetComponent<Rigidbody2D>().velocity.ToVector3();
            var targetFuturePos = radar.target.transform.position + targetSpeed * Time.deltaTime;

            var direction = targetFuturePos - transform.position;            
            body.velocity = direction.normalized * body.velocity.magnitude;

            float alpha = GameUtils.Instance.CalculateAlpha(body.velocity.x, body.velocity.y);
            transform.rotation = Quaternion.Euler(0, 0, alpha);
        }
	}

    void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.tag.Equals("Player"))
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            if (player.PlayerId == playerId)
                return;
            
            if (player.hasShield)
            {

            }
            else
            {
                player.GetStateMachine().ChangeState<PlayerBurningState>();
            }
        }
        else
        {

        }

        particle.Stop();
        collider.enabled = false;
        force.enabled = false;
        body.velocity = Vector2.zero;
        particle.Stop();

        animator.SetBool("isExplosive", true);
    }
}
