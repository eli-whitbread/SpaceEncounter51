using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{

    public static DialogueManager _instance;
    public enum speakingNPC { Adult, Child, AI, None };
    public speakingNPC _speakingNPC;
    public Transform adultAlienAudioSourcePoint, childAlienAudioSourcePoint, player, aiAudioSourcePoint;
    public GameObject adultAlien, childAlien, aiHead;
    public List<AudioClip> adultDialogueClips, childDialogueClips, aiDialogueClips;
    public GameObject alienLettersEmpty;
    public GameObject translatingText;

    [SerializeField]
    int adultDialogueIndex, childDialogueIndex, aiDialogueIndex;
    AudioSource aSource;
    bool animationPlaying = false;

    public bool AnimationPlaying
    {
        set { animationPlaying = value; }
    }

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

                if (_speakingNPC == speakingNPC.Adult && !aSource.isPlaying && adultDialogueIndex < adultDialogueClips.Count && animationPlaying == false)
                {
                    Animator anim = adultAlien.GetComponent<Animator>();

                    // Turns the Alien Letters and Translation text ON
                    if (adultDialogueIndex == 0)
                    {
                        alienLettersEmpty.SetActive(true);
                        translatingText.SetActive(true);
                    }

                    if (adultDialogueIndex >= 1)
                    {
                        animationPlaying = true;
                    }
                    transform.position = adultAlienAudioSourcePoint.position;
                    aSource.PlayOneShot(adultDialogueClips[adultDialogueIndex]);

                    //anim.SetBool("PlayDialogueAnim", true);
                    anim.SetInteger("AudioIndex", adultDialogueIndex);
                    anim.SetTrigger("canPlayNextClip");
                    //anim.SetBool("PlayDialogueAnim", false);


                    adultDialogueIndex++;
                    
                    if (childDialogueIndex <= childDialogueClips.Count && adultDialogueIndex > 2)
                    {
                        _speakingNPC = speakingNPC.Child;

                    }
                    break;
                }
                if (_speakingNPC == speakingNPC.Child && !aSource.isPlaying && childDialogueIndex <= childDialogueClips.Count && animationPlaying == false)
                {
                    // Turns the Alien Letters and Translation text OFF
                    if (alienLettersEmpty.activeInHierarchy)
                    {
                        alienLettersEmpty.SetActive(false);
                        translatingText.SetActive(false);
                    }

                    
                    aSource.PlayOneShot(childDialogueClips[childDialogueIndex]);
                    childDialogueIndex++;
                    _speakingNPC = speakingNPC.Adult;
                    break;
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
