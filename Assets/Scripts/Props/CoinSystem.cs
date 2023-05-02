using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinSystem : MonoBehaviour
{

    public TMP_Text coinDisplayText;

    public int currentCoins;

    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.HasKey("Coins"))
        {
            currentCoins = PlayerPrefs.GetInt("Coins");
        }
        else
        {
            PlayerPrefs.SetInt("Coins", 0);
        }
        
        coinDisplayText.text = currentCoins.ToString();
    }

}
