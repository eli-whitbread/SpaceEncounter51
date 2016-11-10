using UnityEngine;
using System.Collections;

public class ButtonColourChanger : MonoBehaviour {

    //public float colourChangeTime;
    public float minEmissiveValue, maxEmissiveValue;
    

		
	public void ChangeState(bool on)
    {
        Renderer rnd = GetComponent<Renderer>();
        
        if(on == true)
        {
            Color col = new Color(maxEmissiveValue, 0, 0);
            rnd.material.SetColor("_EmissionColor", col);
        }
        else
        {
            Color col = new Color(minEmissiveValue, 0, 0);
            rnd.material.SetColor("_EmissionColor", col);
        }
    }
}
