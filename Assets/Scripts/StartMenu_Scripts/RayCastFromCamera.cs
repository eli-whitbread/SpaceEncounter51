using UnityEngine;
using System.Collections;

public class RayCastFromCamera : MonoBehaviour {
       
    [SerializeField]
    private float m_RayLength = 500f;
    [SerializeField]
    private LayerMask interactiveLayer;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void LateUpdate()
    {
        // Create a ray that points forwards from the camera.
        Ray ray = new Ray(this.transform.position, this.transform.forward);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, m_RayLength, interactiveLayer))
        {
            InteractiveItemLever interactible = hit.collider.GetComponent<InteractiveItemLever>();

            if(interactible)
            {
                interactible.amOver();
            }
        }

    }
}
