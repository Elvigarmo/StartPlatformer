using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

   public Health playerHealth;
   public Image totalHealthBar;
   public Image currentHealthBar;

   private void Start()
   {
      totalHealthBar.fillAmount = playerHealth.currentHealth / 10;
   }

   private void Update()
   {
      currentHealthBar.fillAmount = playerHealth.currentHealth / 10;
   }
}
