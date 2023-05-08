using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombyCollector : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if(other.tag == "Player")
        {
            Shoot shoot = other.GetComponent<Shoot>();
            shoot.hasBomby = true;
            Destroy(this.gameObject);    
        }

       
    }
}
