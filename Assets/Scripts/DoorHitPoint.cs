using UnityEngine;
using System.Collections;

public class DoorHitPoint : MonoBehaviour {

    private AudioSource audioSource;

    public bool hit;

	// Use this for initialization
	void Start ()
    {
        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update () {
	
	}
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
            audioSource.Play();
        }

    }
    private void OnTriggerExit()
    {
    }
}
