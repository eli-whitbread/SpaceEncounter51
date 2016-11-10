using UnityEngine;
using System.Collections;

public class FadeScript : MonoBehaviour {

    public Texture2D fadeOutTexture;
    public float fadeSpeed = 0.0f;
    public bool FadeIn = false;

    private int drawDepth = -1000;
    private float alpha = 0.0f;
    private int fadeDir = -1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        // Fade out/In the alpha value using a direction, a speed and Time.deltaTime to convert the operation to seconds.
        alpha += fadeDir * fadeSpeed * Time.deltaTime;

        // Force (clamp) the number between 0 and 1, because GUI.Colour uses alpha values between 0 and 1;
        alpha = Mathf.Clamp01(alpha);

        //Set colour of the GUI (the texture). All colour values reamin thew same and the alpha is set to the alpha variable
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = drawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
    }

    public float BeginFade (int direction)
    {
        fadeDir = direction;
        return (fadeSpeed);     // returns the fadespeed variable, to make it easy to time the fade
    }

    void OnLevelWasLoaded()
    {
        alpha = 1;
        BeginFade(-1);        
    }
}
