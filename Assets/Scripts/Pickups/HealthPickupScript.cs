using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickupScript : PickupScript
{
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
        StartCoroutine(PlayPickupSound());
	}
}
