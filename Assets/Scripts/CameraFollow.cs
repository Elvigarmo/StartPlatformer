using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class CameraFollow : MonoBehaviour
{
    public Transform player;  // Reference to the player's transform
    public float smoothSpeed = 100f;  // How smooth the camera follows the player
    public Vector3 offset = new Vector3(2, 1, -10);  // Offset from the player's position
    public float lookAheadDistance = 0.2f;  // Distance the camera looks ahead of the player
    private float lookAhead;

    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        if (player != null)
        {
            // Get the player's velocity to determine the direction they are moving
            float playerDirection = player.localScale.x;

            // Calculate the target position: player's position + offset + look-ahead in the moving direction
            //Vector3 desiredPosition = player.position + offset + new Vector3(player.position.x + lookAheadDistance , transform.position.y, transform.position.z);
            transform.position = new Vector3(player.position.x + lookAheadDistance , transform.position.y, transform.position.z);
            lookAhead = Mathf.Lerp(lookAhead, (lookAheadDistance*player.localScale.x), Time.deltaTime* smoothSpeed);

            // // Smoothly move the camera towards the desired position
            // Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed * Time.deltaTime);

           // transform.position = smoothedPosition;
        }
    }

}