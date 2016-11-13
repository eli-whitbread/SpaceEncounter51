using UnityEngine;
using System.Collections;
using UnityEngine.VR;

public class VR_Pickupable : MonoBehaviour {

    public Vector3 positionOffset;
    public Transform mainCamera;
    public bool turnOffPhysicsOnPickUp;

    float clampDist;
    bool interacting = false;
    

	// Update is called once per frame
	void Update () {
	
        if(interacting == true)
        {
            transform.position = mainCamera.transform.position + mainCamera.transform.forward * clampDist;
        }
	}

    void OnCollisionEnter(Collision col)
    {
        if(interacting == true)
        {
            DisableInteraction();
        }
    }

    public void EnableInteraction(Vector3 offSet)
    {
        interacting = true;
        //positionOffset = offSet;
        if (turnOffPhysicsOnPickUp == true)
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
        clampDist = Vector3.Distance(mainCamera.transform.position, transform.position);
    }

    public void DisableInteraction()
    {
        interacting = false;
        if (turnOffPhysicsOnPickUp == true)
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}
