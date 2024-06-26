using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorScript : MonoBehaviour
{
    private GameObject _doorToLinkTo;
    public Directions Direction;

    [SerializeField] private Collider2D _collider;

    public static UnityEvent ExitRoom;

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
        //Transfer room
    }
}
