using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset;

    void Start()
    {
        offset = new Vector3(0,0,0);
    }

    void LateUpdate()
    {
        float newXPosition = player.transform.position.x - offset.x;
        float newZPosition = player.transform.position.z - offset.z;

        transform.position = new Vector3(newXPosition, transform.position.y, newXPosition);
    }
}
