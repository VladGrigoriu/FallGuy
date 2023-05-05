using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleShot : MonoBehaviour
{
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            var shoot = player.GetComponent<Shoot>();
            Destroy(this.gameObject);
            float waitingTime = shoot.GetWaitingTime();
            shoot.SetWaitingTime(waitingTime / 2);
        }
    }

}
