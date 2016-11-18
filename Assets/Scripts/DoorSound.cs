using UnityEngine;
using System.Collections;

public class DoorSound : MonoBehaviour {

    public AudioClip[] doorHitSounds;

    public DoorHitPoint[] HitPoints;

    private int[] indexID;

    private AudioSource audioSource;


	// Use this for initialization
	void Start ()
    {
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        PlayOnHit();
	}

   
    public void PlayOnHit()
    {
        foreach (DoorHitPoint point in HitPoints)
        {
            if (point.hit)
            {
                audioSource.PlayOneShot(doorHitSounds[0]);
            }
        }
    }
  
}
