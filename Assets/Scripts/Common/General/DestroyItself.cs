using UnityEngine;
using System.Collections;

public class DestroyItself : MonoBehaviour
{
    public void Destroy()
    {
        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void PhotonDestroy()
    {
        if (transform.parent != null)
        {
            PhotonNetwork.Destroy(transform.parent.gameObject);
        }
        else
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
