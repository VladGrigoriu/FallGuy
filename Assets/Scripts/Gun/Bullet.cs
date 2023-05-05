using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    public GameObject hitParticles;
    public GameObject enemy;
    public bool hasTarget;
    public Vector2 target;
    public int damage;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Debug.Log("no enemy");
        if(hasTarget && this.gameObject != null && enemy != null)
        {
            Debug.Log("enemy");
            // rb = GetComponent<Rigidbody2D>();
            // rb.velocity = new Vector2(enemy.transform.position.x * 1, enemy.transform.position.y * 1);
            this.transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        }
        else
        {
            rb.velocity = transform.right * speed;
        }
    }

    public void MoveTowardsEnemy(GameObject enemyToShoot)
    {
        enemy = enemyToShoot;
        hasTarget = true;
        target = new Vector2(enemy.transform.position.x, enemy.transform.position.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        Boss boss = other.GetComponent<Boss>();

        if(other.tag == "Wall" || other.tag == "Enemy")
        {
            Destroy(this.gameObject);    
        }

        if(other.tag == "Chest")
        {
            Destroy(this.gameObject); 
            Chest chest = other.GetComponent<Chest>();
            chest.TakeDamage(damage/2);
        }

        if(enemy != null)
        {
            enemy.TakeDamage(damage);
            Instantiate(hitParticles, transform.position, transform.rotation);
        }
        else if(enemy == null && boss != null)
        {
            boss.TakeDamage(damage);
            Instantiate(hitParticles, transform.position, transform.rotation);
        }
    }

}
