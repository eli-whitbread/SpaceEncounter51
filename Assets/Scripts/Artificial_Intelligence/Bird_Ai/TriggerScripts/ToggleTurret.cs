using UnityEngine;
using System.Collections;

public class ToggleTurret : MonoBehaviour {

    public Transform turretLoc;
    public Transform hutLoc;
    public VR_CharacterController player;
   
	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (player.lockControls)
        {
            if (Input.GetButtonDown("B"))
            {
                player.lockControls = false;
                player.MovePlayerWithBlink(hutLoc);
            }
        }
	}
   

    public void TeleportPlayer()
    {
        player.MovePlayerWithBlink(turretLoc);
        player.lockControls = true;
    }
}
