using UnityEngine;
using System.Collections;

public class GameConsts {

    private static GameConsts mInstance = null;

    public static GameConsts Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new GameConsts();
            }

            return mInstance;
        }
    }

    public readonly string STRING_LOGIN_FAIL = "Your username or password incorrect!";
    public readonly string STRING_SIGN_UP_FAIL = "Sign up fail!";
    public readonly string STRING_SIGN_UP_SUCCESS = "Sign up success!";

    public readonly float TIME_COUNT_DOWN_TO_PLAY = 3.0f;

    public readonly float PLAYER_SPEED_ANGLE_DEFAULT = 7.0f;
}
