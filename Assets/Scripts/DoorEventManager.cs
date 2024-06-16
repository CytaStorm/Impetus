using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorEventManager : MonoBehaviour
{
    Collider2D myCollider2D;

    public delegate void DoorAction();
    public static event DoorAction OnDoorEnter;

    void Awake()
    {
        myCollider2D = this.GetComponent<Collider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Called when something enters the attached object's trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (OnDoorEnter != null)
        {
            OnDoorEnter();
        }
    }
}
