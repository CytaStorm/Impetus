using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AetherMaxUpScript : PickupScript
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
        _player.MaxAether += 10;
        StartCoroutine(PlayPickupSound());
	}
}
