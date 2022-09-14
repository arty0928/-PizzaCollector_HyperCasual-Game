using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalArrow : MonoBehaviour
{
    public Transform target;

    private void Update()
    {
        Vector3 targetPosition = target.transform.position;
        targetPosition.y = transform.position.y;
        transform.LookAt(targetPosition);
    }
}
