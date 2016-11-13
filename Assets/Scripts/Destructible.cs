using UnityEngine;
using System.Collections;

public class Destructible : MonoBehaviour {

    public int health;
    public GameObject deathFX;
    
    
	public void ApplyDamage(int val)
    {
        health -= val;
        if(health <= 0)
        {
            GameManager._gameManager.DestroyedObject();
            if(deathFX != null)
            {
                GameObject fxObj = (GameObject)Instantiate(deathFX);
                fxObj.transform.position = transform.position;
                fxObj.transform.rotation = Quaternion.identity;
                fxObj.SetActive(true);
            }
            gameObject.SetActive(false);
        }
    }
}
