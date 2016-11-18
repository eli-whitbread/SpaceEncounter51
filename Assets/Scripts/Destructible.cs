using UnityEngine;
using System.Collections;

public class Destructible : MonoBehaviour {

    public int health;
    public GameObject deathFX;

    private Bird_Agent bird;
   
	public void ApplyDamage(int val)
    {
        
           
        health -= val;
        if(health <= 0)
        {
            if (GetComponent<Bird_Agent>())
            {
                bird = GetComponent<Bird_Agent>();
                bird.IsDead();
            }
            GameManager._gameManager.DestroyedObject();
            if(deathFX != null)
            {
                GameObject fxObj = (GameObject)Instantiate(deathFX);
                fxObj.transform.position = transform.position;
                fxObj.transform.rotation = Quaternion.identity;
                fxObj.SetActive(true);
            }
            //gameObject.SetActive(false);
        }
    }
}
