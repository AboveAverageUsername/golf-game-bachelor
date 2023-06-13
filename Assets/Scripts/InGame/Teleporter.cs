using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public GameObject destination;


    public Transform getDestination()
    {
        return destination.transform;
    }
}
