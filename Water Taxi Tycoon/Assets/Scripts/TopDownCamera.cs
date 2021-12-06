using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    Transform player;
    Vector3 separationDistance;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        separationDistance = transform.position - player.position;
    }

    void LateUpdate()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        transform.position = player.position + separationDistance;
    }
}
