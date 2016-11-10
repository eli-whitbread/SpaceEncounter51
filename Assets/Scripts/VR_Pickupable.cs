using UnityEngine;
using System.Collections;
using UnityEngine.VR;

public class VR_Pickupable : MonoBehaviour {

    public float positionOffset;
    public Transform mainCamera;

    public float clampDist;
    bool interacting = false;
    

	// Update is called once per frame
	void Update () {
	
        if(interacting == true)
        {
            transform.position = mainCamera.transform.position + mainCamera.transform.forward * clampDist;
        }
	}

    public void EnableInteraction()
    {
        interacting = true;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        clampDist = Vector3.Distance(mainCamera.transform.position, transform.position);
    }

    public void DisableInteraction()
    {
        interacting = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
    }
}
