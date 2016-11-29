using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{

    public static DialogueManager _instance;
    public enum speakingNPC { Adult, Child, AI, None };
    public speakingNPC _speakingNPC;
    public Transform adultAlien, childAlien, player, ai;
    public List<AudioClip> adultDialogueClips, childDialogueClips, aiDialogueClips;
    public GameObject alienLettersEmpty;

    [SerializeField]
    int adultDialogueIndex, childDialogueIndex, aiDialogueIndex;
    AudioSource aSource;

    void Awake()
    {
        _instance = this;
        aSource = GetComponent<AudioSource>();
        //  adultDialogueClips = new List<AudioClip>();
        // childDialogueClips = new List<AudioClip>();
        // aiDialogueClips = new List<AudioClip>();
        _speakingNPC = speakingNPC.AI;
    }

    // Use this for initialization
    void Start()
    {
        //call when drone talking stops.


        aSource.volume = 0.6f;
    }

    // Update is called once per frame
    void Update()
    {
        GameManager._gameManager.canUseDrone = true;

        switch (GameManager._gameManager.gameStates)
            {
                case GameManager.GameStates.Start:
                if(_speakingNPC == speakingNPC.AI && !aSource.isPlaying && aiDialogueIndex < aiDialogueClips.Count)
                {
                    if(aiDialogueIndex != 1)
                    {
                    aSource.PlayOneShot(aiDialogueClips[aiDialogueIndex]);
                    aiDialogueIndex++;
                    }
                    else
                    {
                        if(GameManager._gameManager.podHasLanded)
                        {
                            aSource.PlayOneShot(aiDialogueClips[aiDialogueIndex]);
                            aiDialogueIndex++;
                        }
                    }
                }
                else if(aiDialogueIndex == aiDialogueClips.Count)
                {
                    _speakingNPC = speakingNPC.Adult;
                }
                    break;
                case GameManager.GameStates.End:

                    break;
                case GameManager.GameStates.Drone:

                    if (_speakingNPC == speakingNPC.Adult && !aSource.isPlaying && adultDialogueIndex < adultDialogueClips.Count)
                    {
                        if(adultDialogueIndex == 0)
                        {
                            alienLettersEmpty.SetActive(true);
                        }
                        else
                        {
                            alienLettersEmpty.SetActive(false);
                        }

                        aSource.PlayOneShot(adultDialogueClips[adultDialogueIndex]);
                        adultDialogueIndex++;

                        if (childDialogueIndex != childDialogueClips.Count)
                        {
                            _speakingNPC = speakingNPC.Child;

                        }
                    }
                    if (_speakingNPC == speakingNPC.Child && !aSource.isPlaying && childDialogueIndex <= childDialogueClips.Count)
                    {
                        aSource.PlayOneShot(childDialogueClips[childDialogueIndex]);
                        childDialogueIndex++;
                        _speakingNPC = speakingNPC.Adult;
                    }

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
