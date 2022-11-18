using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    public Transform player;
    // Update is called once per frame
    void Update()
    {
        Vector3 forwardDirection = transform.position - player.position;
        forwardDirection.y = 0;
        Quaternion newRotation = Quaternion.LookRotation(forwardDirection, Vector3.up);
        transform.rotation = newRotation;
    }
}
