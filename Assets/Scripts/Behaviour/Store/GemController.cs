using UnityEngine;
using System.Collections;

public class GemController : Singleton<GemController>
{
    public NumberReduceController gems;
	
	void Start ()
    {
        gems.number = PlayerData.Current.gem;
        gems.tmpNumber = gems.number;
	}
	
	public void SetGems(int gemsValue)
    {
        PlayerData.Current.gem = gemsValue;
        gems.number = gemsValue;
    }
}
