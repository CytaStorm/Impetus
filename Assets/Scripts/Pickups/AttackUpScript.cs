using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUpScript : MonoBehaviour
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
        _player.AttackDamage += 10;
        _player.AttackBuffRoomsLeft = 3;
        StartCoroutine(PlayAttackBuffSound());
	}
    private IEnumerator PlayAttackBuffSound()
    {
        _spriteRenderer.enabled = false;
        _rigidbody.simulated = false;
        _audioSource.Play();
        yield return new WaitUntil(
            () => _audioSource.time >= _audioSource.clip.length);
        Destroy(this.gameObject);
    }

}
