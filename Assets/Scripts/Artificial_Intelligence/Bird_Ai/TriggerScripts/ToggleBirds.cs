using UnityEngine;
using System.Collections;

public class ToggleBirds : MonoBehaviour {

    public Bird_Controller flockControl;
    private Renderer rend;
    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("B"))
        {
            flockControl.BirdBathing(false);
            rend.material.color = Color.red;
        }
    }
    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (Input.GetButtonDown("X") || Input.GetKeyDown(KeyCode.P))
            {
                flockControl.BirdBathing(true);
                rend.material.color = Color.green;
            }
        }
    }

}
