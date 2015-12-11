using UnityEngine;
using System.Collections;

public class GemController : Singleton<GemController>
{
    public NumberReduceController gems;
	
	void Start ()
    {
        gems.number = PlayerData.Current.gems;
        gems.tmpNumber = gems.number;
	}
	
	public void SetGems(int gemsValue)
    {
        PlayerData.Current.gems = gemsValue;
        gems.number = gemsValue;
    }
}
