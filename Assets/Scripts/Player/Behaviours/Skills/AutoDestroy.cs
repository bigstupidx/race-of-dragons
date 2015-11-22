using UnityEngine;
using System.Collections;

public class AutoDestroy : MonoBehaviour
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
}
