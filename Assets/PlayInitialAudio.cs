using UnityEngine;
using System.Collections;

public class PlayInitialAudio : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip[] A_initialDialogue;
    private int trackNo = 0;

	// Use this for initialization
	void Start ()
    {
        audioSource = GetComponent<AudioSource>();
        
        audioSource.PlayOneShot(A_initialDialogue[trackNo]);
        trackNo++;

	}
	
	// Update is called once per frame
	void Update ()
    {
        if(trackNo < 2)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(A_initialDialogue[trackNo]);
                trackNo++;
            }	
        }


	}
}
