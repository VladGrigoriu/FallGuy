using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    [SerializeField] float health, maxHealth = 50;

    [SerializeField] float moveSpeed = 5f;
    Rigidbody2D rigidBody;

    Vector2 moveDirection;
    
    [Range(1, 15)]
    [SerializeField]
    private float viewRadius;

    [SerializeField]
    private float detectionCheckDelay = 0.1f;

    [SerializeField]
    private LayerMask playerLayerMask;

    [SerializeField]
    private LayerMask visibilityLayer;

    [SerializeField]
    private Transform target = null;
    
    [field: SerializeField]

    public bool TargetVisible { get; private set; }


    public Transform Target
    {
        get => target;
        set
        {
            target = value;
            TargetVisible = false;
        }
    }

    public PlayerHealth playerHealth;

    private void Awake() 
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }


    void Start()
    {
        health = maxHealth;
        // target = GameObject.Find("Player").transform;
        StartCoroutine(DetectionCoroutine());
    }

    private void Update()
    {
        if(Target != null) TargetVisible = CheckTargetVisible();

        if(Target != null && TargetVisible == true)
        {
            Vector3 direction = (Target.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rigidBody.rotation = angle;
            moveDirection = direction;
        }
    }

    private void FixedUpdate() {

        if(Target != null && TargetVisible == true)
        {
            rigidBody.velocity = new Vector2(moveDirection.x, moveDirection.y) * moveSpeed;
        }
        
    }

    private bool CheckTargetVisible()
    {
        var result = Physics2D.Raycast(transform.position, Target.position - transform.position, viewRadius, visibilityLayer);

        if(result.collider != null)
        {
            return (playerLayerMask & (1 << result.collider.gameObject.layer)) != 0;
        }
        return false;
    }

    private void DetectTarget()
    {
        if(Target == null)
        {
            CheckIfPlayerInRange();
        }
        else if(Target != null)
        {
            DetectIfOutOfRange();
        }
    }

    IEnumerator DetectionCoroutine()
    {
        yield return new WaitForSeconds(detectionCheckDelay);
        DetectTarget();
        StartCoroutine(DetectionCoroutine());
    }

    private void DetectIfOutOfRange()
    {
        if(Target == null || Target.gameObject.activeSelf == false || Vector2.Distance(transform.position, Target.position) > viewRadius)
        {
            Target = null;
        }
    }

    private void CheckIfPlayerInRange()
    {
        Collider2D collision = Physics2D.OverlapCircle(transform.position, viewRadius, playerLayerMask);
        if(collision != null)
        {
            Target = collision.transform;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, viewRadius);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerHealth.TakeDamage(5);
        }
    }

}
