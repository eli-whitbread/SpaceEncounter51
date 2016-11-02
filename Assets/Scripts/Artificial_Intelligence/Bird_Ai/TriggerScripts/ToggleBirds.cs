using UnityEngine;
using System.Collections;

public class ToggleBirds : MonoBehaviour {

    public Flock_Controller flockControl;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("B"))
        {
            flockControl.BirdBathing(false);
        }
    }
    private void OnTriggerStay(Collider collision)
    {
        flockControl.BirdBathing(true);
        if (collision.gameObject.tag == "Player")
        {
            if (Input.GetButtonDown("X") || Input.GetKeyDown(KeyCode.P))
            {
                
            }
        }
    }

}
