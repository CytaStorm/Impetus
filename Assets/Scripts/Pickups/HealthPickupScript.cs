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
        if (PlayerScript.Player.Health == PlayerScript.Player.MaxHealth)
        {
            return;
        }
        PlayerScript.Player.Heal(1, 1, false);
        StartCoroutine(PlayPickupSound());
	}
}
