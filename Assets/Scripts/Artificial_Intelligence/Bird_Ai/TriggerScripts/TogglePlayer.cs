using UnityEngine;
using System.Collections;

public class TogglePlayer : MonoBehaviour {


    public GameObject drone;
    private VR_CharacterController playerController;
	// Use this for initialization
	void Start ()
    {
	 
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetButtonDown("B") && drone.activeInHierarchy)
        {
            playerController.lockControls = false;
            drone.SetActive(false);
        }
    }
    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if ((Input.GetButtonDown("X") || Input.GetKeyDown(KeyCode.P)) && GameManager._gameManager.canUseDrone)
            {
                collision.gameObject.GetComponent<VR_CharacterController>().lockControls = true;
                playerController = collision.gameObject.GetComponent<VR_CharacterController>();
                drone.SetActive(true);
               
            }
        }
    }
    
}
