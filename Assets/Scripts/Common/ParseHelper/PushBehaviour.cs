using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Parse;
using System;

public class PushBehaviour : MonoBehaviour {
    private IDictionary<string, object> dictMsg;
    string msg;

    // Use this for initialization
    void Awake()
    {
#if UNITY_IOS
    NotificationServices.RegisterForRemoteNotificationTypes(RemoteNotificationType.Alert |
                                                            RemoteNotificationType.Badge |
                                                            RemoteNotificationType.Sound);
#endif

        ParsePush.ParsePushNotificationReceived += (sender, args) =>
        {
#if UNITY_ANDROID
            AndroidJavaClass parseUnityHelper = new AndroidJavaClass("com.parse.ParsePushUnityHelper");
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            // Debug
            Debug.Log("Calling Parse from Unity and Payload is : " + args.StringPayload);

            msg = args.StringPayload;
            dictMsg = args.Payload;
            string from = dictMsg["from"].ToString();
            string roomName = dictMsg["room"].ToString();

            if (!string.IsNullOrEmpty(roomName) && !string.IsNullOrEmpty(from))
            {
                ShowInviteDialog(from, roomName);
            }

            // Call default behavior.
            parseUnityHelper.CallStatic("handleParsePushNotificationReceived", currentActivity, args.StringPayload);
#elif UNITY_IOS
      IDictionary<string, object> payload = args.Payload;

      foreach (var key in payload.Keys) {
        Debug.Log("Payload: " + key + ": " + payload[key]);
      }
#endif
        };
    }

    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 500, 500), "Message: " + msg);
    }

    void ShowInviteDialog(string from, string roomName)
    {
        GameObject inviteDialog = Instantiate(Resources.Load("Prefabs/UI/InviteDialog")) as GameObject;
        var inviteBehaviour = inviteDialog.GetComponentInChildren<InviteDialogBehaviour>();
        inviteBehaviour.SetInfo(from, roomName);
    }
}
