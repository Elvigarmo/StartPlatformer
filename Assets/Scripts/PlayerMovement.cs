using UnityEngine;
using UnityEngine.UI; // For referencing UI elements like the Joystick

public class PlayerMovement : MonoBehaviour
{
    public float speed = 4;
    public LayerMask groundLayer;
    public LayerMask wallLayer;
    public float JumpStrength = 2;
    private Rigidbody2D body;
    private Animator anim;
    private bool falling;
    private BoxCollider2D boxCollider;
    public float wallJumpCooldown = 0.2f;
    private float initialGravityScale;
    private float horizontalInput;
    public float wallJumpPush = 10;
    public float wallJumpUpwardPush = 6;
    private Health health;



    // Joystick reference
    public Joystick joystick;  // Assuming you have a Joystick class for handling joystick input


    private void Awake()
    {
        // Grab references
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        health = GetComponent<Health>();

        // Store initial gravity scale
        initialGravityScale = body.gravityScale;
    }

    private void Update()
    {
        // Replace Input.GetAxis with joystick input
        horizontalInput = joystick.Horizontal; // Use joystick input for horizontal movement

        // Flip Player Horizontal based on the current local scale
        if (horizontalInput > 0.01f)
        {
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector2(-Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }

        // Check if player is falling
        falling = body.velocity.y < 0 && !isGrounded();

        // Set animator parameters
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());
        anim.SetBool("falling", falling);

        // Die if offscreen
        if (transform.position.y < -5){
            health.dead = true;
        }

        // Wall jump and sliding logic
        if (wallJumpCooldown > 0.2f)
        {

            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if (onWall() && !isGrounded())
            {

                body.gravityScale = 0;
                body.velocity = Vector2.zero;  // Stick to the wall before sliding

            }
            else
            {
                // Reset to initial gravity scale when not on the wall and reset timer
                body.gravityScale = initialGravityScale;
            }
            if (joystick.Vertical > 0.5f) // Use the vertical axis of the joystick to trigger jump
            {
                Jump();
            }
        }
        else
        {
            wallJumpCooldown += Time.deltaTime;
        }
    }



    private void Jump()
    {
        if (isGrounded())
        {
            // Regular ground jump
            body.velocity = new Vector2(body.velocity.x, JumpStrength);
            anim.SetTrigger("Jump");
        }
        else if (onWall() && !isGrounded())
        {
            // Wall jump logic
            float wallDirection = -Mathf.Sign(transform.localScale.x);  // Get the direction to jump away from the wall

            // Apply horizontal push and upward force
            body.velocity = new Vector2(wallDirection * wallJumpPush, wallJumpUpwardPush);

            // Flip the player's x-scale to face away from the wall
            transform.localScale = new Vector2(wallDirection, transform.localScale.y);

            // Reset the wall jump cooldown after jumping off the wall
            wallJumpCooldown = 0;


        }
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            boxCollider.bounds.center,
            boxCollider.bounds.size,
            0,
            Vector2.down,
            0.1f,
            groundLayer
        );

        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            boxCollider.bounds.center,
            boxCollider.bounds.size,
            0,
            new Vector2(transform.localScale.x, 0),
            0.1f,
            wallLayer
        );


        return raycastHit.collider != null;
    }

    public bool canAttack()
    {
        return !onWall();
    }

}