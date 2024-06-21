using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickupScript : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Rigidbody2D _rigidbody;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
        if (collision.transform.tag != "Player")
        {
            return;
        }
        PlayerScript _player = collision.gameObject.GetComponent<PlayerScript>();
        if (_player.Health == _player.MaxHealth)
        {
            return;
        }
        _player.Heal(1, 1, false);
        StartCoroutine(PlayHealthPickupSound());
	}

    private IEnumerator PlayHealthPickupSound()
    {
        _spriteRenderer.enabled = false;
        _rigidbody.simulated = false;
        _audioSource.Play();
        yield return new WaitUntil(
            () => _audioSource.time >= _audioSource.clip.length);
        Destroy(this.gameObject);
    }
}
