using UnityEngine;
using System.Collections;

public class GunBarrelShootSender : MonoBehaviour {

    public GunShootScript shootScript;
    public Animator bottomBarrelAnim;

    public bool isTopBarrels;

	public void Fire()
    {
        shootScript.CycleProjectileSpawnPoint(isTopBarrels);
    }
    public void TriggerBottomBarrel()
    {
        bottomBarrelAnim.SetBool("Fire", true);
        //bottomBarrelAnim.SetTrigger("Shoot");
        
    }
}
