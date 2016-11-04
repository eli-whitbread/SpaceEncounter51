using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GunShootScript : MonoBehaviour {

    public delegate void NewTargetSet(Transform newTarget);
    public static event NewTargetSet OnTargetSet;
    public GunAiming gunAimScript;
    public Animator topBarrelAnimation, BottomBarrelAnimation;
    public GunBarrelShootSender gunBarrelScript;

    public GameObject projectile;
        
    public Transform projectileSpawnPoint_TopL, projectileSpawnPoint_BottomL, projectileSpawnPoint_TopR, projectileSpawnPoint_BottomR;
    public Transform gunBarrel_TopL, gunBarrel_BottomL, gunBarrel_TopR, gunBarrel_BottomR;

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
	}
	
	// Update is called once per frame
	void Update () {

        //used for testing only - BEGIN
        if (Input.GetKeyDown(KeyCode.F) || Input.GetButtonDown("Teleport"))
        {
            ActivateGun();
            topBarrelAnimation.SetBool("Fire", true);
        }
        if (Input.GetKeyUp(KeyCode.F) || Input.GetButtonUp("Teleport"))
        {
            topBarrelAnimation.SetBool("Fire", false);
            BottomBarrelAnimation.SetBool("Fire", false);
            DeactivateGun();
        }
        //used for testing only - END

        if (gunEnabled)
        {
            if (canShoot == false && coolingDown == false)
            {
                gunEnabled = false;
                topBarrelAnimation.SetBool("Fire", false);
                BottomBarrelAnimation.SetBool("Fire", false);
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
                topBarrelAnimation.SetBool("Fire", false);
                BottomBarrelAnimation.SetBool("Fire", false);
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
            gunAimScript.CoolingDown = true;
            coolingTime = coolDownTime;
        }

    }

    //void AnimateGunBarrels()
    //{
        


    //    //float barrelMoveTop = Mathf.Lerp(131.0f, 143.0f, barrelMoveTime * Time.deltaTime);
    //    //float barrelMoveBottom = Mathf.Lerp(143.0f, 131.0f, barrelMoveTime * Time.deltaTime);
    //    gunBarrel_TopL.transform.localPosition = new Vector3(gunBarrel_TopL.transform.localPosition.x, gunBarrel_TopL.transform.localPosition.y, gunBarrel_TopL.transform.localPosition.z + gunBarrelMoveAmount);
    //    gunBarrel_TopR.transform.localPosition = new Vector3(gunBarrel_TopR.transform.localPosition.x, gunBarrel_TopR.transform.localPosition.y, gunBarrel_TopR.transform.localPosition.z + gunBarrelMoveAmount);
    //    //gunBarrel_BottomL.transform.localPosition = new Vector3(gunBarrel_BottomL.transform.localPosition.x, gunBarrel_BottomL.transform.localPosition.y, barrelMoveBottom);
    //    //gunBarrel_BottomR.transform.localPosition = new Vector3(gunBarrel_BottomR.transform.localPosition.x, gunBarrel_BottomR.transform.localPosition.y, barrelMoveBottom);
        
    //    CycleProjectileSpawnPoint();
    //}

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
