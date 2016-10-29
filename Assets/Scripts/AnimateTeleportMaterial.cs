using UnityEngine;
using System.Collections;

public class AnimateTeleportMaterial : MonoBehaviour
{

    public float scrollSpeedX = 0.1f;
    public float scrollSpeedY = 0.1f;


    // Use this for initialization
    void Start ()
    {
	
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        float offsetX = Time.time * scrollSpeedX;
        float offsetY = Time.time * scrollSpeedY;

        GetComponent<Renderer>().material.mainTextureOffset = new Vector2(offsetX, offsetY);

    }
}
