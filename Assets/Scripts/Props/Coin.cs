using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Coin : MonoBehaviour
{
    public TMP_Text coinDisplayText;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(PlayerPrefs.HasKey("Coins")){
            int currentCoins = PlayerPrefs.GetInt("Coins");
            if(collider.tag == "Player")
            {
                Destroy(this.gameObject);
                currentCoins += 1;
                PlayerPrefs.SetInt("Coins", currentCoins);
                coinDisplayText.text = currentCoins.ToString();
            }
        }
    }
}
