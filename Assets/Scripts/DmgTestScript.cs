using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DmgTestScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Player")
        {
            var healthComponent = collision.GetComponent<HealthManager>();
            if (healthComponent != null)
            {
                healthComponent.TakeDamage(1);
            }
        }
    }
}
