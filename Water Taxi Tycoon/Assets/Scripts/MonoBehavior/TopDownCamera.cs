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
        RotateCamera();
    }

    private void FollowPlayer()
    {
        transform.position = player.position + separationDistance;
    }

    private void RotateCamera()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            transform.Rotate(0f, -90f, 0f);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            transform.Rotate(0f, 90f, 0f);
        }
    }
}
