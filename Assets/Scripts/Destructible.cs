using UnityEngine;
using System.Collections;

public class Destructible : MonoBehaviour {

    public int health;
    
	public void ApplyDamage(int val)
    {
        health -= val;
        if(health <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
