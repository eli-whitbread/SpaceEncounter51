using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InteractiveItemLever : MonoBehaviour {

    public List<Material> oldMaterials, newHighlightedMaterials;
    public Material highlightMaterial;

    private bool isLookedAt, highlightEnabled;

    public GameObject planetToResize;   // Set this in the inspector
    public GameObject leverToRotate;    // Set in the inspector
    private Quaternion newLeverRotation;
    private Quaternion startingRotation;
    [SerializeField]
    float leverRotationSpeed;

	// Use this for initialization
	void Start ()
    {
        foreach(Material i in GetComponent<Renderer>().materials)
        {
            oldMaterials.Add(i);
            newHighlightedMaterials.Add(i);
        }

        newHighlightedMaterials.Add(highlightMaterial);

        startingRotation = new Quaternion(leverToRotate.transform.rotation.x, leverToRotate.transform.rotation.y, leverToRotate.transform.rotation.z, leverToRotate.transform.rotation.w);
        newLeverRotation = new Quaternion(600.0f, leverToRotate.transform.rotation.y, leverToRotate.transform.rotation.z, leverToRotate.transform.rotation.w);
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(isLookedAt)
        {
            gameObject.GetComponent<Renderer>().materials = newHighlightedMaterials.ToArray();
        }
        else
        {
            gameObject.GetComponent<Renderer>().materials = oldMaterials.ToArray();
        }

        isLookedAt = false;
	}

    public void amOver()
    {
        isLookedAt = true;

        if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Fire1"))
        {
            StartCoroutine("RotateLever");

            //Debug.Log("Have been interacted with");
            IncreasePlanetSize increaseScript = planetToResize.GetComponent<IncreasePlanetSize>();
            ActivateReEntry startReentry = GetComponent<ActivateReEntry>();
            increaseScript.increasePlanet();
            startReentry.StartGame();
        }
    }

    //153.56

    private IEnumerator RotateLever()
    {
        while (true)
        {
            leverToRotate.transform.rotation = Quaternion.Lerp(leverToRotate.transform.rotation, newLeverRotation, Time.deltaTime * leverRotationSpeed);
            //If object is in new position, stop script
            if (startingRotation == newLeverRotation)
            {
                yield break;
            }

            // Otherwise, continue next frame
            yield return null;
        }

    }
}
