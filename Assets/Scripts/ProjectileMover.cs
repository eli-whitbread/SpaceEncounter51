using UnityEngine;
using System.Collections;

public class ProjectileMover : MonoBehaviour {

    public float moveSpeed, lifeTime, trackTargetSpeed;

    public Transform target;

    //public Transform Target
    //{
    //    get { return target; }
    //    set { target = value; }
    //}

    void OnEnable()
    {
        GunShootScript.OnTargetSet += GunShootScript_OnTargetSet;
        Invoke("DeactivateProjectile", lifeTime);
    }

    private void GunShootScript_OnTargetSet(Transform newTarget)
    {
        target = newTarget;
    }

    void Update () {

        if(target != null)
        {
            Vector3 angleToTarget = target.position - transform.position;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, angleToTarget, trackTargetSpeed * Time.deltaTime, 0.0f);
            transform.localRotation = Quaternion.LookRotation(newDir);
        }

        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
	}

    void DeactivateProjectile()
    {
        target = null;
        gameObject.SetActive(false);
    }
}
