using UnityEngine;

public class Fireball : MonoBehaviour
{
	Rigidbody2D rb;

	[SerializeField] float speed = 5.0f;

	[SerializeField] int damage = 3;

	[SerializeField] private ParticleSystem boom;

	private SpriteRenderer sprite;
	private BoxCollider2D col;

	// Start is called before the first frame update
	void Start()
	{
		sprite = GetComponent<SpriteRenderer>();
		col = GetComponent<BoxCollider2D>();

		rb = GetComponent<Rigidbody2D>();
		rb.gravityScale = 1;
		rb.velocity = new Vector2(speed, rb.velocity.y);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		Character player = other.GetComponentInParent<Character>();

        if(player != null)
        {
            player.ModifyHealth(-damage);
            transform.position = player.transform.position;
            Explode();
        }

        if (other.CompareTag("Ground"))
        {
            Invoke(nameof(Explode), 0.08f);
        }

    }

	void Explode()
	{
		sprite.enabled = false;
		rb.gravityScale = 0;
		rb.velocity = Vector2.zero;
		col.enabled = false;
		boom.Play();

        CameraMovement cameraMovement = Camera.main.GetComponent<CameraMovement>();
        cameraMovement.DoCameraShake(0.33f, 0.07f); //expose these

        Destroy(gameObject, boom.main.duration);

	}
}