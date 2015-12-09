using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;

public class GameUtils {

    private static GameUtils mInstance = null;

    public static GameUtils Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new GameUtils();
            }

            return mInstance;
        }
    }

    public List<Dictionary<string, string>> DecodeStringAsListOfDic(string data)
    {
        List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
        string[] dicarr = data.Split(';');

        string[] arrtemp;
        string[] arrtemp2;
        Dictionary<string, string> tempdic;
        for (int i = 1; i < dicarr.Length; i++)
        {
            arrtemp = dicarr[i].Split(',');

            tempdic = new Dictionary<string, string>();
            for (int j = 0; j < arrtemp.Length; j++)
            {
                arrtemp2 = arrtemp[j].Split(':');
                tempdic.Add(arrtemp2[0], arrtemp2[1]);
            }
            result.Add(tempdic);
        }
        return result;
    }

    public int[] DecodeStringAsArrayInt(string data)
    {
        string[] temp = data.Split(',');
        return convertStringArrToIntArr(temp);
    }

    public string[] DecodeStringAsArrayStr(string data)
    {
        return data.Split(',');
    }

    public Dictionary<string, int> DecodeStringAsDictionaryStringInt(string data)
    {
        Dictionary<string, int> result = new Dictionary<string, int>();
        string[] temp = data.Split(',');

        string str;
        string[] strarr;
        for (int i = 0; i < temp.Length; i++)
        {
            str = temp[i];
            strarr = str.Split(':');
            result.Add(strarr[0], int.Parse(strarr[1]));
        }
        return result;
    }

    public int[] convertStringArrToIntArr(string[] strarr)
    {
        int[] results = new int[strarr.Length];

        for (int i = 0; i < strarr.Length; i++)
        {
            results[i] = int.Parse(strarr[i]);
        }

        return results;
    }

    public bool randomPercent(float percent)
    {
        if (percent == 0) return false;

        if (percent == 100) return true;

        int tempPercent = (int)(percent * 10f);
        return (Random.Range(0, 1000) < tempPercent);
    }

    public void Shuffle<T>(List<T> arr)
    {
        if (arr == null || arr.Count == 0)
            return;

        T t;
        int n;
        for (int i = 0; i < arr.Count; i++)
        {
            n = Random.Range(0, arr.Count);
            t = arr[i];
            arr[i] = arr[n];
            arr[n] = t;
        }
    }

    public void Shuffle<T>(T[] arr)
    {
        if (arr == null || arr.Length == 0)
            return;

        int n;
        T t;
        for (int i = 0; i < arr.Length; i++)
        {
            n = Random.Range(0, arr.Length);
            t = arr[i];
            arr[i] = arr[n];
            arr[n] = t;
        }
    }

    public void sortArrayListAtFirst(List<ArrayList> list)
    {
        ArrayList tempArrayList;
        for (int tempIndex = 0; tempIndex < list.Count - 1; tempIndex++)
        {
            for (int tempIndex2 = tempIndex + 1; tempIndex2 < list.Count; tempIndex2++)
            {
                if ((int)list[tempIndex][0] < (int)list[tempIndex2][0])
                {
                    tempArrayList = list[tempIndex];
                    list[tempIndex] = list[tempIndex2];
                    list[tempIndex2] = tempArrayList;
                }
            }
        }
    }

    public bool IsIntersectListInt(List<int> list1, int fromlist1, List<int> list2, int fromlist2)
    {
        for (int tempIndex = fromlist1; tempIndex < list1.Count; tempIndex++)
        {
            for (int tempIndex2 = fromlist2; tempIndex2 < list2.Count; tempIndex2++)
            {
                if (list1[tempIndex] == list2[tempIndex2])
                    return true;
            }
        }
        return false;
    }

    public float DegreeToRadian(float d)
    {
        return d * Mathf.PI / 180.0f;
    }

    public float RadianToDegree(float r)
    {
        return r * 180.0f / Mathf.PI;
    }

    public float CalculateAlpha(float x, float y)
    {
        float result = Mathf.Atan2(y, x);
        result = RadianToDegree(result);

        return result;
    }

    public static T GetCustomProperty<T>(PhotonView view, string property, T defaultValue)
    {
        if (view != null && view.owner != null && view.owner.customProperties.ContainsKey(property) == true)
        {
            return (T)view.owner.customProperties[property];
        }

        return defaultValue;
    }

    public static void SetCustomProperty<T>(PhotonView view, string property, T value)
    {
        if (view != null && view.owner != null)
        {
            ExitGames.Client.Photon.Hashtable prop = new ExitGames.Client.Photon.Hashtable();
            prop.Add(property, value);

            view.owner.SetCustomProperties(prop);
        }
    }

    public static void SetRoomCustomProperty<T>(string property, T value)
    {
        ExitGames.Client.Photon.Hashtable prop = new ExitGames.Client.Photon.Hashtable();
        prop.Add(property, value);

        PhotonNetwork.room.SetCustomProperties(prop);
    }

    public static T GetRoomCustomProperty<T>(string property, T defaultValue)
    {
        if (PhotonNetwork.room.customProperties.ContainsKey(property))
        {
            return (T)PhotonNetwork.room.customProperties[property];
        }

        return defaultValue;
    }
}
