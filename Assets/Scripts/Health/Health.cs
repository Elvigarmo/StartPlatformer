using UnityEngine;
using UnityEngine.Scripting;

public class Health : MonoBehaviour
{
    public float startingHealth = 3f;
    public float currentHealth { get; private set; }
    private Animator anim;
    public bool dead;


    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
    }
    public void TakeDamage(float _damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
        if (currentHealth > 0)
        {
            Debug.Log("hurt");
            anim.SetTrigger("Hurt");
        }
        else
        {
            if (!dead)
            {
                Debug.Log("die");
                anim.SetTrigger("Die");
                dead = true;
            }

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
