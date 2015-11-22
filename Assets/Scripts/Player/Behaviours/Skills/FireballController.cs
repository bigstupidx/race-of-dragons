using UnityEngine;
using System.Collections;

public class FireballController : MonoBehaviour
{
    public Animator animator;
    public ParticleSystem particle;
    public RadarController radar;
    public float maxForce = 10;

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
        if (other.gameObject.GetInstanceID() == radar.parentId)
            return;

        particle.Stop();
        collider.enabled = false;
        force.enabled = false;
        body.velocity = Vector2.zero;
        particle.Stop();

        if (other.gameObject.tag.Equals("Player"))
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            player.GetStateMachine().ChangeState<PlayerBurningState>();           
        }
        else
        {

        }

        animator.SetBool("isExplosive", true);
    }

    public void SetParentId(int parentId)
    {
        radar.parentId = parentId;
    }
}
