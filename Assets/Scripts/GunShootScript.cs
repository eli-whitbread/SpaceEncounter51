using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GunShootScript : MonoBehaviour {

    public delegate void NewTargetSet(Transform newTarget);
    public static event NewTargetSet OnTargetSet;

    public GameObject projectile;
    public Transform projectileSpawnPoint;
    public float projectilePoolSize, rateOfFire, warmupTime, coolDownTime, shotsBeforeOverheat;
    public bool canShoot;

    float myTime, shotTime, coolingTime, shotsFired;
    bool coolingDown, gunEnabled;
    List<GameObject> projectilePool;
    Transform target;

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

        projectilePool = new List<GameObject>();
        for (int i = 0; i < projectilePoolSize; i++)
        {
            GameObject obj = (GameObject)Instantiate(projectile);
            obj.SetActive(false);
            projectilePool.Add(obj);
        }
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
                    shotsFired = 0;
                    myTime = 0;
                }
            }

            if (canShoot == true && shotTime >= rateOfFire && myTime >= warmupTime && coolingDown == false)
            {
                Shoot();
                shotTime = 0;
            }

            if (shotsFired >= shotsBeforeOverheat && coolingDown == false)
            {
                canShoot = false;
                coolingDown = true;
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
    }

    public void DeactivateGun()
    {
        canShoot = false;
        if (shotsFired > 0 && coolingDown == false)
        {
            coolingDown = true;
            coolingTime = coolDownTime;
        }

    }

    void Shoot()
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
