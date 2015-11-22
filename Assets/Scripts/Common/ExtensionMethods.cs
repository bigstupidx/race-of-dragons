using UnityEngine;
using System.Collections;

public static class ExtensionMethods
{
    public static void ResetTransformation(this Transform trans)
    {
        trans.position = Vector3.zero;
        trans.localRotation = Quaternion.identity;
        trans.localScale = new Vector3(1, 1, 1);
    }

    public static Vector3 ToVector3(this Vector2 vector2)
    {
        return new Vector3(vector2.x, vector2.y, 0);
    }
}
