using UnityEngine;
using UnityEngine.UI;


public class PlayerAttack : MonoBehaviour
{
    private Animator anim;
    private PlayerMovement playerMovement;
    public float cooldownTimer = Mathf.Infinity;
    public float attackCooldown = 2f; // Example cooldown time
    public bool canAttack = true;
    public Vector3 fireballOffset = new Vector3(1f, -0.7f, 0f); // Adjust as needed

    // Fire button and cooldown ring UI
    public Button fireButton;
    public Image cooldownRing; // Reference to the UI Image for the cooldown ring

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();

        // Add the button listener to call Attack when pressed
        fireButton.onClick.AddListener(OnFireButtonPressed);

    }

    private void Update()
    {
        fireButton.image.color = Color.white;
        cooldownTimer += Time.deltaTime;

        // Update the cooldown ring fill amount (0 = full cooldown, 1 = ready to fire)
        cooldownRing.fillAmount = Mathf.Clamp01(cooldownTimer / attackCooldown);
        if (cooldownTimer <= attackCooldown)
        {
            fireButton.image.color = Color.red;
        }
        else
        {
            fireButton.image.color = Color.white;
        }


    }

    private void OnFireButtonPressed()
    {
        // Check if the player can attack (cooldown, player state)
        if (cooldownTimer > attackCooldown && canAttack && playerMovement.canAttack())
        {
            Attack();

        }
    }

    private void Attack()
{
    anim.SetTrigger("Attack");
    cooldownTimer = 0;

    // Get a fireball and set its position and direction
    GameObject fireball = ObjectPoolManager.Instance.GetFireball();

    // Calculate the adjusted offset for the fireball based on player direction
    float xDirection = Mathf.Sign(transform.localScale.x); // Get the direction player is facing (1 for right, -1 for left)
    
    // Flip the X offset but keep the Y offset the same regardless of direction
    Vector3 adjustedOffset = new Vector3(fireballOffset.x * xDirection, fireballOffset.y, fireballOffset.z);

    // Set fireball position with adjusted offset
    Vector3 spawnPosition = transform.position + adjustedOffset;
    fireball.transform.position = spawnPosition;

    // Launch the fireball in the direction the player is facing
    fireball.GetComponent<Fireball>().Launch(xDirection);
}
}