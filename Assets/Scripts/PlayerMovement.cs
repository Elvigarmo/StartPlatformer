using UnityEngine;
using UnityEngine.SceneManagement; // For restarting the scene
using UnityEngine.UI; // For handling UI elements like buttons

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
    public Joystick joystick;

    // UI elements
    public GameObject restartScreen;  // The restart UI screen
    public Button restartButton;      // Reference to the restart button
    public Button quitButton;         // Reference to the quit button

    private Vector3 initialPosition;  // Store the initial player position

    private void Awake()
    {
        // Grab references
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        health = GetComponent<Health>();

        // Store initial gravity scale
        initialGravityScale = body.gravityScale;

        // Store the initial position of the player
        initialPosition = transform.position;

        // Attach listeners to the buttons
        restartButton.onClick.AddListener(RestartGame);
        quitButton.onClick.AddListener(QuitGame);
    }

    private void Update()
    {
        // Off boundaries
        // if (body.position.y < -5)
        // {
        //     body.bodyType = RigidbodyType2D.Static;
        //     health.dead = true;
        // }
        // else
        // {
        //     body.bodyType = RigidbodyType2D.Dynamic;
        // }

        if (health.dead)
        {
            restartScreen.SetActive(true); // Show the restart screen
        }

        // Replace Input.GetAxis with joystick input
        horizontalInput = joystick.Horizontal;

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

        // Wall jump and sliding logic
        if (wallJumpCooldown > 0.2f && !health.dead)
        {
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if (onWall() && !isGrounded())
            {
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
            }
            else
            {
                body.gravityScale = initialGravityScale;
            }
            if (joystick.Vertical > 0.5f)
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
            body.velocity = new Vector2(body.velocity.x, JumpStrength);
            anim.SetTrigger("Jump");
        }
        else if (onWall() && !isGrounded())
        {
            float wallDirection = -Mathf.Sign(transform.localScale.x);
            body.velocity = new Vector2(wallDirection * wallJumpPush, wallJumpUpwardPush);
            transform.localScale = new Vector2(wallDirection, transform.localScale.y);
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

    // Method to restart the game
    private void RestartGame()
    {
        health.dead = false;                   // Revive the player
        restartScreen.SetActive(false);        // Hide the restart screen
        transform.position = initialPosition;   // Move player to the initial position
        
    }

    // Method to quit the game
    private void QuitGame()
    {
        Application.Quit();   // Quits the game
        // Note: Quitting will only work in a built version of the game, not in the editor.
    }
}
