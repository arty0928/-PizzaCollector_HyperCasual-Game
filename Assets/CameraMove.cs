using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform player;

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = new Vector3(player.position.x, 8f, player.position.z-6);
        transform.position = (playerPos);
    }
}
