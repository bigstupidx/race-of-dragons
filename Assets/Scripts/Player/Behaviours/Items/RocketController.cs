using UnityEngine;
using System.Collections;

public class RocketController : Photon.PunBehaviour
{
    public float maxForce = 10;
    public GameObject[] listParticle;
    private ParticleSystem particle;

    private Rigidbody2D body;
    private ConstantForce2D force;
    private float timer;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();       
        force = GetComponent<ConstantForce2D>();

        float alpha = GameUtils.Instance.DegreeToRadian(transform.rotation.eulerAngles.z);
        force.force = new Vector2(maxForce * Mathf.Cos(alpha), maxForce * Mathf.Sin(alpha));

        body.velocity = force.force;

        int rand = Random.Range(0, listParticle.Length);
        listParticle[rand].SetActive(true);
        particle = listParticle[rand].GetComponent<ParticleSystem>();
        particle.Play();

        SoundManager.Instance.playSound(ESound.Rocket);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 20)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Player"))
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            if (player.PlayerId == photonView.ownerId)
                return;

            if (player.hasShield)
            {

            }
            else
            {
                player.speedAngle = 0;
            }
        }
        else
        {

        }
    }
}
