using UnityEngine;
using System.Collections;

public class NameInPlaceHolderController : Singleton<NameInPlaceHolderController>
{
    public TextMesh[] text;
	
	void Start ()
    {
        
	}
		
	void Update ()
    {
        UpdateNameList();
    }

    public void UpdateNameList()
    {
        if (PhotonNetwork.connected && PhotonNetwork.room != null)
        {
            int playerCount = PhotonNetwork.room.playerCount;
            string listName = GameUtils.GetRoomCustomProperty<string>("LIST_NAME", "");
            if (listName != "")
            {
                var names = listName.Split(',');
                if (names.Length == playerCount)
                {
                    for (int i = 0; i < names.Length; i++)
                    {
                        text[i].text = names[i].ToShortString();
                    }
                }
                else
                {
                    listName = "";
                    for (int i = 0; i < playerCount; i++)
                    {
                        string name = (string)PhotonNetwork.playerList[i].customProperties["NAME"];
                        if (!string.IsNullOrEmpty(name))
                        {
                            if (i < playerCount - 1)
                                listName += name + ",";
                            else
                                listName += name;
                        }                        
                    }
                    GameUtils.SetRoomCustomProperty<string>("LIST_NAME", listName);
                }                
            }                       
        }
    }
}
