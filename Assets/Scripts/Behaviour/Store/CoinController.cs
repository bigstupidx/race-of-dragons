using UnityEngine;
using System.Collections;

public class CoinController : Singleton<CoinController>
{
    public NumberReduceController coins;

	void Start()
    {
        coins.number = PlayerData.Current.gold;
        coins.tmpNumber = coins.number;
    }

    public void SetCoins(int coinsValue)
    {
        PlayerData.Current.gold = coinsValue;
        coins.number = PlayerData.Current.gold;
    }
}
