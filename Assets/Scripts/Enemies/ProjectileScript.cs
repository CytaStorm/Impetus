using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    #region Variables
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _damage = 10f;
    private Rigidbody2D _rb;
    #endregion

    #region Start Method
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (_rb != null)
        {
            // Assuming the projectile is facing right initially
            _rb.velocity = transform.right * _speed;
        }
        else
        {
            Debug.LogError("Rigidbody2D component missing from the projectile!");
        }
    }
    #endregion

    #region Collision Handling
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        PlayerScript player = hitInfo.GetComponent<PlayerScript>();
        if (player != null)
        {
            player.TakeDamage(_damage, 1f);
            Debug.Log("Player hit! Damage applied: " + _damage);
        }
        else
        {
            Debug.Log("Hit object is not a player: " + hitInfo.name);
        }
        Destroy(gameObject); // Destroy the projectile after it hits something
    }
    #endregion
}
