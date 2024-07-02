using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour
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

    public IEnumerator PlayPickupSound()
    {
        _spriteRenderer.enabled = false;
        _rigidbody.simulated = false;
        _audioSource.Play();
        yield return new WaitUntil(
            () => _audioSource.time >= _audioSource.clip.length);
        Destroy(this.gameObject);
    }
}
