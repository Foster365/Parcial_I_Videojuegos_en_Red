using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DMGFistTestScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Enemy")
        {
            var healthComponent = collision.GetComponent<HealthManager>();
            if (healthComponent != null)
            {
                healthComponent.TakeDamage(5);
            }
        }
    }
}
