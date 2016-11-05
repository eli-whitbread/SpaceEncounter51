using UnityEngine;
using System.Collections;

public class AlienLanguage : MonoBehaviour
{

    [SerializeField]
    private int columnSize; // x (u) Coordinate
    [SerializeField]    
    private int rowSize;    // y (v) Coordinate
    [SerializeField]
    private int colFrameStart = 0;
    [SerializeField]
    private int rowFrameStart = 0;
    [SerializeField]
    private int totalFrames = 0;

    [SerializeField]
    private float framesPerSecond = 4.0f;   // Speed of Animation

    // Use this for initialization
    void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        int index = (int)(Time.time * framesPerSecond); // time control fps
        index = index % totalFrames;

        Vector2 size = new Vector2(1.0f / columnSize, 1.0f / rowSize);

        int u = index % columnSize;
        int v = index / columnSize;

        Vector2 offset = new Vector2((u + colFrameStart) * size.x, (1.0f - size.y) - (v + rowFrameStart) * size.y);

        GetComponent<Renderer>().material.mainTextureOffset = offset; // Texture Offset
        GetComponent<Renderer>().material.mainTextureScale = size;  // Texture Scale

        GetComponent<Renderer>().material.SetTextureOffset("_BumpMap", offset);
        GetComponent<Renderer>().material.SetTextureScale("_BumpMap", size);
    }
}
