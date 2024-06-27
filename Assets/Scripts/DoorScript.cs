using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorScript : MonoBehaviour
{
    [SerializeField] private Collider2D _collider;
    [SerializeField] private GameObject _player;
    public GameObject LinkedDoor;
    public Directions Direction;


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
