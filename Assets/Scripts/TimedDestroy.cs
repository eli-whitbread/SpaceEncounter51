using UnityEngine;
using System.Collections;

public class TimedDestroy : MonoBehaviour {

    public float lifeTime;

    void OnEnable()
    {
        Invoke("Destroy", lifeTime);
    }

    void Destroy()
    {
        gameObject.SetActive(false);
    }
}
