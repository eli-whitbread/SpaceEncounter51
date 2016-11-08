using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GunShootScript : MonoBehaviour {

    public delegate void NewTargetSet(Transform newTarget);
    public static event NewTargetSet OnTargetSet;
    public GunAiming gunAimScript;
    public GunBarrelMover gunBarrelMover;

    public GameObject projectile;
        
    public Transform projectileSpawnPoint_TopL, projectileSpawnPoint_BottomL, projectileSpawnPoint_TopR, projectileSpawnPoint_BottomR;
    public Transform gunBarrelsAnchor_Top, gunBarrelsAnchor_Bottom;

    public float projectilePoolSize, rateOfFire, warmupTime, coolDownTime, shotsBeforeOverheat, barrelMoveTime;
    public bool canShoot;

    float myTime, shotTime, coolingTime, shotsFired;
    bool coolingDown, gunEnabled, fireTopBarrels, gunBarrelSwitchDir = false;
    List<GameObject> projectilePool;
    Transform target;
    public float gunBarrelMoveAmount = 0;

    public Transform Target
    {
        get { return target; }
        set { target = value;
            if(OnTargetSet != null)
             OnTargetSet(target);
        }
    }
   
	// Use this for initialization
	void Start () {

        canShoot = false;
        shotsFired = 0;
        gunEnabled = false;
        fireTopBarrels = true;

        projectilePool = new List<GameObject>();
        for (int i = 0; i < projectilePoolSize; i++)
        {
            GameObject obj = (GameObject)Instantiate(projectile);
            obj.SetActive(false);
            projectilePool.Add(obj);
        }

        //for testing
        ActivateGun();
	}
	
	// Update is called once per frame
	void Update () {

        //used for testing only - BEGIN
        if (Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("Teleport"))
        {
            ActivateGun();
        }
        if (Input.GetKeyUp(KeyCode.F) || Input.GetButtonUp("Teleport"))
        {
            DeactivateGun();
        }
        //used for testing only - END

        if (gunEnabled)
        {
            if (canShoot == false && coolingDown == false)
            {
                gunEnabled = false;
                return;
            }

            myTime += Time.deltaTime;
            shotTime += Time.deltaTime;

            if (coolingDown == true)
            {
                coolingTime -= Time.deltaTime;
                if (coolingTime <= 0)
                {
                    coolingDown = false;
                    gunAimScript.CoolingDown = false;
                    shotsFired = 0;
                    myTime = 0;
                }
            }

            if (canShoot == true && shotTime >= rateOfFire && myTime >= warmupTime && coolingDown == false)
            {
                //AnimateGunBarrels();
                //CycleProjectileSpawnPoint();
                shotTime = 0;
            }

            if (shotsFired >= shotsBeforeOverheat && coolingDown == false)
            {
                canShoot = false;
                coolingDown = true;
                gunAimScript.CoolingDown = true;
                coolingTime = coolDownTime * 1.5f;
            }

        }
	}

    public void ActivateGun()
    {
        if (canShoot == false)
        {
            myTime = 0;
        }
        canShoot = true;
        gunEnabled = true;
        gunBarrelMover.Activated = true;
    }

    public void DeactivateGun()
    {
        canShoot = false;

        gunBarrelMover.Activated = false;
        if (shotsFired > 0 && coolingDown == false)
        {
            coolingDown = true;
            gunAimScript.CoolingDown = true;
            coolingTime = coolDownTime;
        }

    }

    
    public void CycleProjectileSpawnPoint(bool top)
    {
        //if(fireTopBarrels == true)
        if(top == true)
        {
            Shoot(projectileSpawnPoint_TopL);
            Shoot(projectileSpawnPoint_TopR);
            //fireTopBarrels = false;
        }
        else
        {
            Shoot(projectileSpawnPoint_BottomL);
            Shoot(projectileSpawnPoint_BottomR);
            //fireTopBarrels = true;
        }
       
    }
    

    void Shoot(Transform projectileSpawnPoint)
    {
        
        for (int i = 0; i < projectilePool.Count; i++)
        {
            if (!projectilePool[i].activeInHierarchy)
            {
                projectilePool[i].transform.position = projectileSpawnPoint.position;
                projectilePool[i].transform.rotation = projectileSpawnPoint.rotation;
                projectilePool[i].SetActive(true);
                shotsFired++;
                break;
            }
        }
    }

    
}
