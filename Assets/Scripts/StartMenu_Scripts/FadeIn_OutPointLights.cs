using UnityEngine;
using System.Collections;

public class FadeIn_OutPointLights : MonoBehaviour {

    //public float lightIntensity;
    public float incrementValue = 0.05f;
    private Light lightComponent;
    private float intensityLow;
    private bool goingUp = false;
    public bool changingColor = false;
	// Use this for initialization
	void Start ()
    {
        lightComponent = GetComponent<Light>();
        intensityLow = 1.0f;
        lightComponent.intensity = intensityLow;
    }
	
	// Update is called once per frame
	void Update ()
    {
        IntensityChanging();

    }

    void IntensityChanging()
    {     
        if(lightComponent.intensity == 1.0f)
        {
            goingUp = true;
            if(changingColor)
            {
                lightComponent.color = new Color(1, 0, 0);
                lightComponent.intensity = 5.74f;
            }
        }
        else if(lightComponent.intensity == 6)
        {
            goingUp = false;
            if(changingColor)
            {
                lightComponent.color = new Color(0, 0, 1);
                lightComponent.intensity = 3.35f;
            }            
        }

        if (goingUp)
        {
            if(lightComponent.intensity < 6)
            {
                lightComponent.intensity += incrementValue;
            }
            else
            {
                lightComponent.intensity = 6;
            }            
        }
        else
        {
            if (lightComponent.intensity > 1.0f)
            {
                lightComponent.intensity -= incrementValue;
            }
            else
            {
                lightComponent.intensity = 1.0f;
            }
        }

    }
}
