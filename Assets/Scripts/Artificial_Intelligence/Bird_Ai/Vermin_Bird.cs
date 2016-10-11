using UnityEngine;
using System.Collections;

public class Vermin_Bird : Vermin_AI
{

    private GameManager gameManager;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
    private void OnTriggerEnter(Collider collided)
    {
        if (collided.tag == "Missles")
        {
            gameObject.SetActive(false);
        }
    }
}
