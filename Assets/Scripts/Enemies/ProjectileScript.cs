using System.Collections;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    #region Variables
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _damage = 10f;
    private Rigidbody2D _rb;
    #endregion

    #region Properties
    public float Speed => _speed;
    #endregion

    #region Start Method
    void Start()
    {
        //_rb = GetComponent<Rigidbody2D>();
        //if (_rb != null)
        //{
        //    // Assuming the projectile is facing right initially
        //    _rb.velocity = transform.right * _speed;
        //}
        //else
        //{
        //    Debug.LogError("Rigidbody2D component missing from the projectile!");
        //}

        //Start a timer to delete the object
        StartCoroutine("SelfDestruct");
    }
    #endregion

    #region Collision Handling
    void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerScript player = collision.gameObject.GetComponent<PlayerScript>();
        if (player != null)
        {
            player.TakeDamage(_damage, 1f);
            Debug.Log("Player hit! Damage applied: " + _damage);
        }
        else
        {
            Debug.Log("Hit object is not a player: " + collision.gameObject.name);
        }
        Destroy(this.gameObject); // Destroy the projectile after it hits something
    }
    #endregion

    #region Self Destruct
    private IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }
    #endregion 
}
