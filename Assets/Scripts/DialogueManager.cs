using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour {

    public static DialogueManager _instance;
    public enum speakingNPC { Adult, Child, None };
    public speakingNPC _speakingNPC;
    public Transform adultAlien, childAlien,player;
    public List<AudioClip> adultDialogueClips, childDialogueClips;

    [SerializeField]
    int adultDialogueIndex, childDialogueIndex;
    AudioSource aSource;

    void Awake()
    {
        _instance = this;
        aSource = GetComponent<AudioSource>();
        adultDialogueClips = new List<AudioClip>();
        childDialogueClips = new List<AudioClip>();
        _speakingNPC = speakingNPC.Adult;
    }

	// Use this for initialization
	void Start ()
    {
        //call when drone talking stops.
        GameManager._gameManager.canUseDrone = true;
       
    }

    // Update is called once per frame
    void Update ()
    {
        switch (GameManager._gameManager.gameStates)
        {
            case GameManager.GameStates.Start:

                break;
            case GameManager.GameStates.End:

                break;
            case GameManager.GameStates.Drone:

                break;
            case GameManager.GameStates.Cannon:

                break;
            case GameManager.GameStates.Free:

                break;
            default:
                break;
        }



    }
   
  
}
