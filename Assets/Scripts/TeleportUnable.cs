using UnityEngine;
using System.Collections;

public class TeleportUnable : MonoBehaviour
{
    public bool cannotTeleport = false;
	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Player" || other.tag != "Ground")
        {
            cannotTeleport = true;
            Debug.Log("Cannot Teleport into " + other.name + "!!");
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("ABLE Teleport into " + other.name + "!!");
        cannotTeleport = false;
    }
}
