using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public MovingPlatform door;
    private void OnTriggerEnter(Collider other)
    {
        door.NextPlatform();
    }

    private void OnTriggerExit(Collider other)
    {
        door.NextPlatform();
    }
}
