using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorScript : MonoBehaviour
{
    [SerializeField] private Collider2D _collider;
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
        if (collision.gameObject.tag != "Player")
        {
            return;
        }
        Vector3 DirectionToSendPlayer = Vector3.zero;
        float nextDoorOffset = 1.25f;
        switch (Direction)
        {
            case Directions.Top:
                DirectionToSendPlayer = Vector3.up * nextDoorOffset;
                break;
            case Directions.Bottom:
                DirectionToSendPlayer = Vector3.down * nextDoorOffset;
                break;
            case Directions.Left:
                DirectionToSendPlayer = Vector3.left * nextDoorOffset;
                break;
            case Directions.Right:
                DirectionToSendPlayer = Vector3.right * nextDoorOffset;
                break;
            default:
                throw new UnityException("" +
                    "Linked Door Script has no direction assigned to it!");
        }
        collision.gameObject.transform.position = 
            LinkedDoor.transform.position + DirectionToSendPlayer;
    }
}
