using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : MonoBehaviour
{
    [SerializeField]
    private Transform target = null;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // this.transform.parent.gameObject.GetComponent<Rigidbody2D>().AddForce((collision.gameObject.transform.position - transform.position) * (16));
    }
}
