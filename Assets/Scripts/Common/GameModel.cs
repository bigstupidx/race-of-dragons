using UnityEngine;
using System.Collections;

public class GameModel {

    private static GameModel mInstance = null;

    public static GameModel Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new GameModel();
            }

            return mInstance;
        }
    }
}
