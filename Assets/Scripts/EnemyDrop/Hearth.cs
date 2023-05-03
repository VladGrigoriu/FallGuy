using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hearth : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();

        if(other.tag == "Player"){
            var player = GameObject.FindGameObjectWithTag("Player");
            var playerHealth = player.GetComponent<PlayerHealth>();
            playerHealth.Heal(20);
            Destroy(this.gameObject); 

        }
    }
}
