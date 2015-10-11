using UnityEngine;

public class TextMeshHelper : MonoBehaviour {

    public string layerToPushTo;

    // Use this for initialization
    void Start () {
        GetComponent<Renderer>().sortingLayerName = layerToPushTo;
    }
	
}
