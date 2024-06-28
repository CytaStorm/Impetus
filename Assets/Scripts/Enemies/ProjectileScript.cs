using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 10f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed; // Assuming the projectile is facing right initially
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        PlayerScript player = hitInfo.GetComponent<PlayerScript>();
        if (player != null)
        {
            player.TakeDamage(damage, 1f);
        }
        Destroy(gameObject); // Destroy the projectile after it hits something
    }
}
