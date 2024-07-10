using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorScript : MonoBehaviour
{
    [SerializeField] private Collider2D _collider;
    public GameObject LinkedDoor;
    [HideInInspector] public bool IsEntrance;
    [HideInInspector] public bool IsExit;


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
        if (collision.gameObject.tag != "Player" ||
            LinkedDoor == null)
        {
            return;
        }

        //Set player in new room.
        Vector3 DirectionToSendPlayer;
        float nextDoorOffset = 1.25f;
        switch (this.gameObject.tag)
        {
            case "Top Door":
                DirectionToSendPlayer = Vector3.up;
                break;
            case "Bottom Door":
                DirectionToSendPlayer = Vector3.down;
                break;
            case "Left Door":
                DirectionToSendPlayer = Vector3.left;
                break;
            case "Right Door":
                DirectionToSendPlayer = Vector3.right;
                break;
            default:
                throw new UnityException("Untagged Door!");
        }
        collision.gameObject.transform.position = 
            LinkedDoor.transform.position + DirectionToSendPlayer * nextDoorOffset;

        //Load new rooms.
        if (IsEntrance)
        {
            LevelManagerScript.Instance.ChangeRoom.Invoke(-1);
        }
        else if (IsExit)
        {
            LevelManagerScript.Instance.ChangeRoom.Invoke(1);
        }

    }
}
