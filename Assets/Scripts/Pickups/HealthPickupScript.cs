using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickupScript : MonoBehaviour
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
        collision.gameObject.GetComponent<PlayerScript>().Heal(
            1, 1, false);
        Destroy(this.gameObject);
	}
}
