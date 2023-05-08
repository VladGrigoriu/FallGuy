using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUp : MonoBehaviour
{   
    public PlayerHealth playerHealth;

    private void OnTriggerEnter2D(Collider2D other)
    {

        if(other.tag == "Player")
        {
            playerHealth = other.GetComponent<PlayerHealth>();
            playerHealth.maxHealth = playerHealth.maxHealth + 40;
            playerHealth.Heal(playerHealth.maxHealth);
            Destroy(this.gameObject);
        }
        
    }
}
