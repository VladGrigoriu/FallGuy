using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    float timer = 0f;
    public float waitingTime = 0.5f;
    public Transform shootingPoint;
    public GameObject bullet;
    public GameObject player;
    public float searchRadius = 15f;
    private ITarget closestEnemy;
    private GameObject enemyToShoot;

    [SerializeField]
    private LayerMask visibilityLayer;

    [SerializeField]
    private LayerMask playerLayerMask;

    

    void Update()
    {
        FindInArea();
        Quaternion rotationAmount = Quaternion.Euler(0, 0, 90);
        timer += Time.deltaTime;
        if(enemyToShoot != null && CheckTargetVisible() && Vector3.Distance(enemyToShoot.transform.position, shootingPoint.position) < searchRadius)
        {
            if(timer > waitingTime){
                Vector2 direction = enemyToShoot.transform.position - shootingPoint.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                Instantiate(bullet, shootingPoint.position, rotation);
                var bulletScript = bullet.GetComponent<Bullet>();
                bulletScript.MoveTowardsEnemy(enemyToShoot);
                
                timer = 0;
            }
        }
        else
        {
            enemyToShoot = null;
            if(timer > waitingTime)
            {
                Instantiate(bullet, shootingPoint.position, shootingPoint.rotation);
                timer = 0;
            }
        }

    }

    public float GetWaitingTime()
    {
        return waitingTime;
    }

    public void SetWaitingTime(float newWaitingTime)
    {
        waitingTime = newWaitingTime;
    }

    private bool CheckTargetVisible()
    {
        var result = Physics2D.Raycast(transform.position, enemyToShoot.transform.position - transform.position, searchRadius, visibilityLayer);

        if(result.collider != null)
        {
            return (playerLayerMask & (1 << result.collider.gameObject.layer)) != 0;
        }
        enemyToShoot=null;
        return false;
    }

    private void FindInArea()
    {
        int maxColliders = 5;
        Collider2D[] hitColliders = new Collider2D[maxColliders];
        int numColliders = Physics2D.OverlapCircleNonAlloc(player.transform.position, searchRadius, hitColliders);
        float closestDistance = Mathf.Infinity;
     
        for (int i = 0; i < numColliders; i++)
        {
            ITarget target;
            hitColliders[i].TryGetComponent(out target);

            if(target != null)
            {
                float distance = Vector3.Distance(player.transform.position, hitColliders[i].transform.position);
                if(distance < closestDistance)
                {
                    if(closestEnemy != null)
                    {
                        closestEnemy = null;
                        enemyToShoot = null;
                    }
                    closestEnemy = target;
                    closestDistance = distance;
                    enemyToShoot = closestEnemy.PlayerTarget();
                }
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(player.transform.position, searchRadius);
        
    }

}
