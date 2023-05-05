using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour
{
    public bool hasPlayer = false;
    public Transform playerTransform;
    private Vector3 zAxis = new Vector3(0, 0, -1);

    void Update()
    {
        if(hasPlayer)
        {
            transform.RotateAround(playerTransform.position, zAxis, 50 * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            hasPlayer = true;
            playerTransform = other.transform;
            Vector3 temp = new Vector3(1,0,0);
            Transform petTransform = other.transform;
            // petTransform.position += temp;
            this.gameObject.transform.parent = other.transform;
            this.gameObject.transform.position += temp;
        }
        
    }
}
