using UnityEngine;
 
// Attach this script to your fox (the player object).
// The fox needs a Rigidbody and a Collider on it (a Capsule or Box Collider works well).
[RequireComponent(typeof(Rigidbody))]
public class FoxMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;       // How fast the fox walks left and right
    public float jumpForce = 7f;       // How high the fox jumps
 
    [Header("Ground check")]
    public LayerMask groundLayers = ~0; // Which layers count as "ground" (default: everything)
    public float groundCheckExtra = 0.1f; // Extra reach below the fox's feet
 
    private Rigidbody rb;
    private Collider col;
    private float moveInput;           // -1 = left, 0 = standing still, 1 = right
    private bool jumpRequested;
 
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
 
        // Keep the fox on the 2D plane and stop it from tipping over.
        // (This does the same as ticking "Freeze Position Z" and "Freeze Rotation" in the Inspector.)
        rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
    }
 
    // Update runs every frame - best place to read the keyboard
    void Update()
    {
        // A/D or Left/Right arrow keys
        moveInput = Input.GetAxisRaw("Horizontal");
 
        // Space bar - we remember the press and do the actual jump in FixedUpdate
        if (Input.GetButtonDown("Jump"))
        {
            jumpRequested = true;
        }
 
        FlipToFaceDirection();
    }
 
    // FixedUpdate runs on the physics timer - best place to move Rigidbodies
    void FixedUpdate()
    {
        // Set horizontal speed, but keep the current vertical speed (gravity, jumping)
        Vector3 velocity = rb.linearVelocity;
        velocity.x = moveInput * moveSpeed;
        rb.linearVelocity = velocity;
 
        // Jump only if standing on the ground
        if (jumpRequested && IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        jumpRequested = false;
    }
 
    // Shoots a short invisible line down from the fox's centre.
    // If it hits something, the fox is standing on the ground.
    bool IsGrounded()
    {
        float distance = col.bounds.extents.y + groundCheckExtra;
        return Physics.Raycast(col.bounds.center, Vector3.down, distance, groundLayers, QueryTriggerInteraction.Ignore);
    }
 
    // Mirrors the fox so it looks left or right depending on where it walks
    void FlipToFaceDirection()
    {
        if (moveInput > 0.01f)
        {
            SetFacing(-1f);
        }
        else if (moveInput < -0.01f)
        {
            SetFacing(1f);
        }
    }
 
    void SetFacing(float direction)
    {
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;
    }
}