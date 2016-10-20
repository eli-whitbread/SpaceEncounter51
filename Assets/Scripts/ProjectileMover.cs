using UnityEngine;
using System.Collections;

public class ProjectileMover : MonoBehaviour {

    public float moveSpeed, lifeTime;

    void OnEnable()
    {
        Invoke("DeactivateProjectile", lifeTime);
    }
	
	void Update () {

        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
	}

    void DeactivateProjectile()
    {
        gameObject.SetActive(false);
    }
}
