using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float speed = 10f;
    public float damage = 10f;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = transform.right * speed; // Assuming the projectile is facing right initially
        }
        else
        {
            Debug.LogError("Rigidbody2D component missing from the projectile!");
        }
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        PlayerScript player = hitInfo.GetComponent<PlayerScript>();
        if (player != null)
        {
            player.TakeDamage(damage, 1f);
            Debug.Log("Player hit! Damage applied: " + damage);
        }
        else
        {
            Debug.Log("Hit object is not a player: " + hitInfo.name);
        }
        Destroy(gameObject); // Destroy the projectile after it hits something
    }
}
