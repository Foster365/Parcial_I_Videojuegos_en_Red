using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUpScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag != "Enemy")
        {
            var healthComponent = collision.GetComponent<HealthManager>();
            if (healthComponent != null)
            {
                healthComponent.TakeDamage(-1);
                Destroy(gameObject);
            }
        }
    }
}
