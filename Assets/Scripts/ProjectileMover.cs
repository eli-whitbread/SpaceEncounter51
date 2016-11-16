using UnityEngine;
using System.Collections;

public class ProjectileMover : MonoBehaviour {

    public float moveSpeed, lifeTime, trackTargetSpeed;
    public int damage;
    public Transform target;
    public bool trackTargets;

    
    void OnEnable()
    {
        GunShootScript.OnTargetSet += GunShootScript_OnTargetSet;
        target = null;
        Invoke("DeactivateProjectile", lifeTime);
    }

    private void GunShootScript_OnTargetSet(Transform newTarget)
    {
        target = newTarget;
    }

    void Update () {

        if(target != null && trackTargets == true)
        {
            Vector3 angleToTarget = target.position - transform.position;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, angleToTarget, trackTargetSpeed * Time.deltaTime, 1.0f);
            //transform.localRotation = Quaternion.LookRotation(newDir);
            if (newDir != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(newDir);
            }
        }

        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Destructible") || other.CompareTag("Bird_AI"))
        {
            Destructible destObj = other.GetComponent<Destructible>();
            destObj.ApplyDamage(damage);
           // Bird_Agent destBird = other.GetComponent<Bird_Agent>();
           // destObj.ApplyDamage(damage);
        }

        CancelInvoke();
        gameObject.SetActive(false);
    }

    void DeactivateProjectile()
    {
        target = null;
        gameObject.SetActive(false);
    }
}
