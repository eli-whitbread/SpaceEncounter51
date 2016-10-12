using UnityEngine;
using System.Collections;

public class PickupObject : MonoBehaviour
{

    bool carryingObject;
    GameObject carriedObject;
    public float distance;
    public float smooth;
	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(carryingObject)
        {
            carry(carriedObject);
            checkDrop();
        }
        else
        {
            pickup();
        }
	}

    void carry(GameObject o)
    {
        o.transform.position = Vector3.Lerp(o.transform.position, Camera.main.transform.position + Camera.main.transform.forward * distance, Time.deltaTime * smooth);
        o.transform.rotation = Quaternion.identity;
    }

    void pickup()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            int x = Screen.width / 2;
            int y = Screen.height / 2;

            Ray ray = Camera.main.ScreenPointToRay(new Vector3(x, y));
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                Pickupable p = hit.collider.GetComponent<Pickupable>();
                if(p != null)
                {
                    carryingObject = true;
                    carriedObject = p.gameObject;
                    p.gameObject.GetComponent<Rigidbody>().useGravity = true;
                }
            }
        }

    }

    void checkDrop()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            dropObject();
        }
    }

    void dropObject()
    {
        carryingObject = false;
        carriedObject.gameObject.GetComponent<Rigidbody>().useGravity = false;
        carriedObject = null;
        
    }
}
