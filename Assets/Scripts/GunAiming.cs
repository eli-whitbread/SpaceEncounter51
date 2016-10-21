﻿using UnityEngine;
using System.Collections;

public class GunAiming : MonoBehaviour {

    public float rotationSpeed;
    public Transform gunGimbal;
    public PlayerReticle playerReticleScript;

    [SerializeField]
    Transform target;

    public bool gunActive = false;

    public bool GunActive
    {
        get { return gunActive; }
        set { gunActive = value; }
    }

    	
	void Update () {
	
        if(gunActive)
        {
            target = playerReticleScript.ReticleTransform;
            Vector3 angleToTarget = target.position - gunGimbal.position;
            Vector3 newDir = Vector3.RotateTowards(gunGimbal.transform.forward, angleToTarget, rotationSpeed * Time.deltaTime, 0.0f);

            gunGimbal.transform.rotation = Quaternion.LookRotation(newDir);
        }
	}
}