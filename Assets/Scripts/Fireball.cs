using UnityEditor;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private Animator anim;
    private Rigidbody2D rb;

    private bool hit;
    public float speed = 10f;
    public float lifetime = 5f;
    private float direction;
    private bool isActive = false;  // To track if the fireball is actively being used

    private void Awake()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        hit = false;  // Reset hit status when enabled
        isActive = true;  // Mark fireball as active
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);  // Ensure it moves initially
        Invoke("Deactivate", lifetime); // Deactivate after lifetime

        
    }

    public void Launch(float dir)
    {
        // Check if components are properly initialized before proceeding
        if (boxCollider == null || rb == null || anim == null)
        {
            Debug.LogWarning("Fireball components are not properly initialized.");
            return;  // If any component is missing, stop the launch process
        }

        direction = dir;
        hit = false;
        boxCollider.enabled = true;

        // Flip the fireball if needed
        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != dir)
        {
            localScaleX = -localScaleX;
        }
        transform.localScale = new Vector2(localScaleX, transform.localScale.y);
    }

    private void Update()
    {
        if (hit)
        {
            return;
        }

        // Move the fireball if it hasn't hit anything
        rb.velocity = new Vector2(direction * speed, rb.velocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Handle collision and trigger the explode animation
        if (!hit && isActive)
        {
            hit = true;
            boxCollider.enabled = false;  // Disable collider to prevent further collisions
            rb.velocity = Vector2.zero;  // Stop the fireball from moving
            anim.SetTrigger("Explode");
        }
    }

    private void Deactivate()
    {
        if (isActive)
        {
            isActive = false;  // Mark the fireball as inactive
            ObjectPoolManager.Instance.ReturnFireball(gameObject);
        }
    }

    private void OnDisable()
    {
        CancelInvoke();  // Cancel the lifetime invoke if the fireball is disabled before it naturally deactivates
    }
}