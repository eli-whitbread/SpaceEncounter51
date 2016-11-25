using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VR_NPCInteractableObject : MonoBehaviour {

    public GameObject AlienTextTranslate;
    public List<AudioClip> free;
    public List<AudioClip> drone;
    public List<AudioClip> cannon;
    public List<AudioClip> end;

    private AudioSource audioSource;

    public int freeIndex = 0;
    public int droneIndex = 0;
    public int cannonIndex = 0;
    public int endIndex = 0;

    // Use this for initialization
    void Start ()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    public void NPCInteracted()
    {
        switch (GameManager._gameManager.gameStates)
        {
            case GameManager.GameStates.Start:

                break;
            case GameManager.GameStates.End:
                if (!audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(end[endIndex]);
                    endIndex++;
                }
                break;
            case GameManager.GameStates.Drone:
                if (!audioSource.isPlaying && droneIndex < drone.Count)
                {
                    audioSource.PlayOneShot(drone[droneIndex]);
                    droneIndex++;

                    if(droneIndex == 1)
                    {
                        AlienTextTranslate.SetActive(true);
                    }
                    else if(droneIndex == 2)
                    {
                        AlienTextTranslate.SetActive(false);
                    }
                    if (droneIndex == drone.Count)
                    {
                        GameManager._gameManager.canUseDrone = true;
                    }
                }
                break;
            case GameManager.GameStates.Cannon:
                if (!audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(cannon[cannonIndex]);
                    cannonIndex++;
                }
                break;
            case GameManager.GameStates.Free:
                if (!audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(free[freeIndex]);
                    freeIndex++;
                }
                break;
            default:
                Debug.Log("Error..");
                break;
        }
    }
}
