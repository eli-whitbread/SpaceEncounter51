using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InteractiveItemLever : MonoBehaviour {

    //public List<Material> oldMaterials, newHighlightedMaterials;
    //public Material highlightMaterial;

    private bool isLookedAt, highlightEnabled;

    public GameObject planetToResize;   // Set this in the inspector
    //public GameObject leverToRotate;    // Set in the inspector
    public GameObject initialLever;
    public GameObject leverHead;
    public GameObject leverToTurnOn;
    public GameObject LeverHighlight;
    public GameObject aiHead;

    private Quaternion newLeverRotation;
    private Quaternion startingRotation;
    [SerializeField]
    float leverRotationSpeed;

	// Use this for initialization
	void Start ()
    {
        //foreach(Material i in GetComponent<Renderer>().materials)
        //{
        //    oldMaterials.Add(i);
        //    newHighlightedMaterials.Add(i);
        //}

        //newHighlightedMaterials.Add(highlightMaterial);

        //startingRotation = new Quaternion(leverToRotate.transform.rotation.x, leverToRotate.transform.rotation.y, leverToRotate.transform.rotation.z, leverToRotate.transform.rotation.w);
        //newLeverRotation = new Quaternion(600.0f, leverToRotate.transform.rotation.y, leverToRotate.transform.rotation.z, leverToRotate.transform.rotation.w);
        
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(isLookedAt)
        {
            //gameObject.GetComponent<Renderer>().materials = newHighlightedMaterials.ToArray();
            LeverHighlight.SetActive(true);
        }
        else
        {
            //gameObject.GetComponent<Renderer>().materials = oldMaterials.ToArray();
            LeverHighlight.SetActive(false);
        }

        isLookedAt = false;


	}

    public void amOver()
    {
        isLookedAt = true;

        if(!leverToTurnOn.activeInHierarchy)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Fire1") || Input.GetButtonDown("Teleport"))
            {
                Animator anim = aiHead.GetComponent<Animator>();
                initialLever.GetComponent<MeshRenderer>().enabled = false;
                LeverHighlight.GetComponent<MeshRenderer>().enabled = false;
                leverHead.GetComponent<MeshRenderer>().enabled = false;
                anim.SetBool("RunDialogue1", true);                
                StartCoroutine("RotateLever");

                //Debug.Log("Have been interacted with");
                IncreasePlanetSize increaseScript = planetToResize.GetComponent<IncreasePlanetSize>();
                ActivateReEntry startReentry = GetComponent<ActivateReEntry>();
                increaseScript.increasePlanet();
                startReentry.StartGame();
            }
        }
        
    }

    //153.56

    private IEnumerator RotateLever()
    {
        while (true)
        {
            //leverToRotate.transform.rotation = Quaternion.Lerp(leverToRotate.transform.rotation, newLeverRotation, Time.deltaTime * leverRotationSpeed);
            //If object is in new position, stop script
            //if (startingRotation == newLeverRotation)
            //{
            //    yield break;
            //}
            
            leverToTurnOn.SetActive(true);

            // Otherwise, continue next frame
            yield return new WaitForSeconds(0);
        }

    }
}
