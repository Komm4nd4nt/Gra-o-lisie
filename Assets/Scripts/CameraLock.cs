using UnityEngine;
 
// Attach this script to your Main Camera.
// The camera follows the player left and right (X axis),
// while its height (Y) and distance (Z) stay locked in place.
public class CameraLock : MonoBehaviour
{
    [Header("What the camera follows")]
    public Transform target;          // Drag your fox (player) here in the Inspector
 
    [Header("Follow settings")]
    public float smoothSpeed = 5f;    // Higher = camera catches up faster. Set to 0 for no smoothing.
    public float offsetX = 0f;        // Shift the camera left/right relative to the player
 
    [Header("Optional level limits")]
    public bool useLimits = false;    // Tick this to stop the camera at the level edges
    public float minX = -20f;         // Leftmost position of the camera
    public float maxX = 20f;          // Rightmost position of the camera
 
    // These store the camera's starting height and depth, so they never change
    private float lockedY;
    private float lockedZ;
 
    void Start()
    {
        // Remember where the camera is when the game starts
        lockedY = transform.position.y;
        lockedZ = transform.position.z;
    }
 
    // LateUpdate runs after all movement for the frame is done,
    // which prevents the camera from jittering behind the player
    void LateUpdate()
    {
        if (target == null) return; // Nothing to follow, do nothing
 
        // Where we want the camera to be on the X axis
        float desiredX = target.position.x + offsetX;
 
        // Keep it inside the level limits, if enabled
        if (useLimits)
        {
            desiredX = Mathf.Clamp(desiredX, minX, maxX);
        }
 
        // Smoothly move toward the desired X (or jump straight there if smoothSpeed is 0)
        float newX = desiredX;
        if (smoothSpeed > 0f)
        {
            newX = Mathf.Lerp(transform.position.x, desiredX, smoothSpeed * Time.deltaTime);
        }
 
        // Apply the new position: X changes, Y and Z stay locked
        transform.position = new Vector3(newX, lockedY, lockedZ);
    }
}