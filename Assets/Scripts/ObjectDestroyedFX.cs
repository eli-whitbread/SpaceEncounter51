using UnityEngine;
using System.Collections;

public class ObjectDestroyedFX : MonoBehaviour {

    public float lifeTime;

	void OnEnable()
    {
        Invoke("DisableFX", lifeTime);
    }

    void DisableFX()
    {
        gameObject.SetActive(false);
    }
}
