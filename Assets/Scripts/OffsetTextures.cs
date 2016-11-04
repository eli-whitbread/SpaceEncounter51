using UnityEngine;
using System.Collections;

public class OffsetTextures : MonoBehaviour {
       
    public float XscrollSpeed = 0.05f;
    public float YscrollSpeed = 0f;
    public float XrotateObject;
    public float YrotateObject;
    public bool rotateObject = false, displacementTransform = false, edgeLengthTransform = false;
    [SerializeField]
    private float minDisplacement = 0.818f;
    [SerializeField]
    private float maxDisplacement = 0.95f;
    [SerializeField]
    private float minEdgeLength = 18.8f;
    [SerializeField]
    private float maxEdgeLength = 35f;
    [SerializeField]
    private bool expandingDisplacement = false, expandingEdgeLength = false;
    [SerializeField]
    private float currentDisplacement;
    [SerializeField]
    private float incrementDispValue = 0.003f;
    [SerializeField]
    private float currentEdgeLength;
    [SerializeField]
    private float incrementEdgeValue = 0.05f;


    // Use this for initialization
    void Start ()
    {
        currentDisplacement = minDisplacement;
        currentEdgeLength = minEdgeLength;
    }
	
	// Update is called once per frame
	void Update ()
    {
        GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2((Time.time * XscrollSpeed), (Time.time * YscrollSpeed)));

        if(rotateObject)
        {
            transform.Rotate(Vector3.right * XrotateObject);
            transform.Rotate(Vector3.up * YrotateObject);
        }

        if(displacementTransform)
        {
            EaseIn_OutDisplacement();
        }

        if(edgeLengthTransform)
        {
            Ease_EdgeLength();
        }
       
    }

    void EaseIn_OutDisplacement()
    {
        if(currentDisplacement == minDisplacement)
        {
            expandingDisplacement = true;
        }
        else if(currentDisplacement == maxDisplacement)
        {
            expandingDisplacement = false;
        }

        if(expandingDisplacement)
        {
            if(currentDisplacement < maxDisplacement)
            {
                currentDisplacement += incrementDispValue;
            }
            else
            {
                currentDisplacement = maxDisplacement;
            }
        }
        else
        {
            if(currentDisplacement > minDisplacement)
            {
                currentDisplacement -= incrementDispValue;
            }
            else
            {
                currentDisplacement = minDisplacement;
            }
        }

        GetComponent<Renderer>().material.SetFloat("_Displacement", currentDisplacement);
    }

    void Ease_EdgeLength()
    {
        if (currentEdgeLength == minEdgeLength)
        {
            expandingEdgeLength = true;
        }
        else if (currentEdgeLength == maxEdgeLength)
        {
            expandingEdgeLength = false;
        }

        if (expandingEdgeLength)
        {
            if (currentEdgeLength < maxEdgeLength)
            {
                currentEdgeLength += incrementEdgeValue;
            }
            else
            {
                currentEdgeLength = maxEdgeLength;
            }
        }
        else
        {
            if (currentEdgeLength > minEdgeLength)
            {
                currentEdgeLength -= incrementEdgeValue;
            }
            else
            {
                currentEdgeLength = minEdgeLength;
            }
        }

        GetComponent<Renderer>().material.SetFloat("_EdgeLength", currentEdgeLength);
    }
}
