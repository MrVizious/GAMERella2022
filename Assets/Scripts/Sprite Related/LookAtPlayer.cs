using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    public Transform player;
    public bool inverted = false;

    void Update()
    {
        Vector3 forwardDirection = inverted ? transform.position - player.position : player.position - transform.position;
        forwardDirection.y = 0;
        Quaternion newRotation = Quaternion.LookRotation(forwardDirection, Vector3.up);
        transform.rotation = newRotation;
    }
}
