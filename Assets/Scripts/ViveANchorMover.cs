using UnityEngine;
using System.Collections;
using UnityEngine.VR;

public class ViveANchorMover : MonoBehaviour {

    public Transform player;
    bool mouseLook;

	void Start()
    {
        if(!VRDevice.isPresent)
        {
            mouseLook = true;
        }
        else
        {
            mouseLook = false;
        }
    }
	// Update is called once per frame
	void Update () {

        transform.position = player.transform.position;

        if(mouseLook)
        {
            Quaternion lookDirection = player.rotation;
            lookDirection.x = 0;
            lookDirection.z = 0;
            transform.localRotation = lookDirection;
        }

    }
}
