using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorEventManager : MonoBehaviour
{
    public bool isExit = false;
    private Collider2D myCollider2D;

    public delegate void DoorAction();
    public static event DoorAction OnDoorEnter;
    public static event DoorAction OnDoorExit;

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
        if (!isExit) // trigger entrance door events
        {
            if (OnDoorEnter != null)
            {
                OnDoorEnter();
            }
        }
        else // trigger exit door events
        {
            if (OnDoorExit != null)
            {
                OnDoorExit();
            }
        }
        
    }
}
