using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] public int maxHealth, currentHealth;

    public GameObject coin;
    
    private int counter = 1;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }


    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if(currentHealth <= 0)
        {
            //DEAD
            //Play dead animation 
            Destroy(this.gameObject);

            while (counter < 5)
            {
                counter++;
                System.Random rand = new System.Random();
                // coinRb.AddForce(Vector3.up * rand.Next(500));

                Vector3 coinPosition = transform.position;
                coinPosition.y += rand.Next(2);
                coinPosition.x += rand.Next(2);

                Instantiate(coin, coinPosition, transform.rotation);        
            }

        }
    }
}
