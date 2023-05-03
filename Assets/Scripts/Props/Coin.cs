using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Coin : MonoBehaviour
{
    public TMP_Text coinDisplayText;
    public Transform target;
    public bool isMoving = false;
    public GameObject coinShine;

    private void Update()
    {
        if(isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, 6 * Time.deltaTime);
            if(target.position == transform.position){
                isMoving=false;
                if(PlayerPrefs.HasKey("Coins")){
                    int currentCoins = PlayerPrefs.GetInt("Coins");
                    Destroy(this.gameObject);
                    Instantiate(coinShine, transform.position, transform.rotation);
                    currentCoins += 1;
                    PlayerPrefs.SetInt("Coins", currentCoins);
                    coinDisplayText.text = currentCoins.ToString();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Player")
        {
            isMoving=true;
        }
    }
}
