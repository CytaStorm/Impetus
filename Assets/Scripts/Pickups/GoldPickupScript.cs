using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldPickupScript : PickupScript
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collider)
	{
        if (collider.transform.tag != "Player")
        {
            return;
        }
        collider.gameObject.GetComponent<PlayerScript>().Gold++;
        StartCoroutine(PlayPickupSound());
	}
}
