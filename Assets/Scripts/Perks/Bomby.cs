using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomby : MonoBehaviour
{
    public int damage;
    public GameObject explosion;


    private void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        Boss boss = other.GetComponent<Boss>();

        if(other.tag == "Wall" || other.tag == "Enemy")
        {
            Destroy(this.gameObject);    
        }

        if(enemy != null)
        {
            enemy.TakeDamage(damage);
            Instantiate(explosion, transform.position, transform.rotation);
        }
        else if(enemy == null && boss != null)
        {
            boss.TakeDamage(damage);
            Instantiate(explosion, transform.position, transform.rotation);
        }
    }
}
