using UnityEngine;
using System.Collections;

public class GunAiming : MonoBehaviour {

    public float rotationSpeed;
    public Transform gunRotGimbal, gunTiltGimbal, coolingDownAimPoint;
    public PlayerReticle playerReticleScript;

    [SerializeField]
    Transform target;

    public bool gunActive = false;
    public bool coolingDown = false;

    public bool GunActive
    {
        get { return gunActive; }
        set { gunActive = value; }
    }
    public bool CoolingDown
    {
        set { coolingDown = value; }
    }

	void Update () {
	
        if(gunActive)
        {
            if (coolingDown == false)
            {
                target = playerReticleScript.ReticleTransform;
            }
            else
            {
                target = coolingDownAimPoint;
            }
            Vector3 angleToTarget = target.position - gunTiltGimbal.position;
            Vector3 newDir = Vector3.zero;
            if (coolingDown == false)
            {
                newDir = Vector3.RotateTowards(gunTiltGimbal.transform.forward, angleToTarget, rotationSpeed * Time.deltaTime, 0.0f);
            }
            else
            {
                newDir = Vector3.RotateTowards(gunTiltGimbal.transform.forward, angleToTarget, (rotationSpeed / 3) * Time.deltaTime, 0.0f);
            }

            newDir.y = Mathf.Clamp(newDir.y, -0.3f, 0.8f);
            
            gunTiltGimbal.transform.localRotation = Quaternion.LookRotation(newDir);
            Quaternion rot = gunTiltGimbal.transform.rotation;
            rot.x = 0;
            rot.z = 0;
            gunRotGimbal.transform.localRotation = rot;
        }
	}
}
