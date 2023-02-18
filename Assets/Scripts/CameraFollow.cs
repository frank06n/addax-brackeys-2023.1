using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float CameraFollowSmoothTime;
    private Vector3 cameraVelocity;
    void FixedUpdate()
    {
        Vector3 playerPos = player.position;
        playerPos.z = transform.position.z;
        //transform.position = playerPos;

        Vector3.SmoothDamp(transform.position, playerPos, ref cameraVelocity, CameraFollowSmoothTime);
        transform.position += cameraVelocity * Time.deltaTime;
    }
}
