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
    public int minDroneIndex = 0, maxDroneIndex = 9, minCannonIndex = 10, maxCannonIndex = 13;//, minEndIndex = 14, maxEndIndex = 17; 

    [SerializeField]
    int animationIndex, adultDialogueIndex, childDialogueIndex, aiDialogueIndex;
    AudioSource aSource;
    public AudioSource dropPodDialogue;
    bool animationPlaying = false, playerInShack = false, hasPlayedAiClip2 = false;

    public bool AnimationPlaying
    {
        set { animationPlaying = value; }
    }

    public bool PlayerInShack
    {
        set { playerInShack = value; }
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
                    transform.position = player.position;

                    if (aiDialogueIndex != 1)
                    {
                        if(!hasPlayedAiClip2)
                        {                            
                            dropPodDialogue.PlayOneShot(aiDialogueClips[aiDialogueIndex]);
                            aiDialogueIndex++;
                            hasPlayedAiClip2 = true;
                        }
                        
                    }
                    else
                    {
                        if(GameManager._gameManager.podHasLanded)
                        {
                            Animator anim = aiHead.GetComponent<Animator>();
                            anim.SetInteger("GroundPodAudio", 1);
                            aSource.PlayOneShot(aiDialogueClips[aiDialogueIndex]);
                            aiDialogueIndex++;
                        }
                    }
                }
                else if (aiDialogueIndex == aiDialogueClips.Count)
                {
                    aiHead.SetActive(false);
                    _speakingNPC = speakingNPC.Adult;
                }
                break;
            case GameManager.GameStates.End:
                if (playerInShack == true)
                {
                    if (_speakingNPC == speakingNPC.Adult && !aSource.isPlaying && adultDialogueIndex < adultDialogueClips.Count && animationPlaying == false)// && animationIndex >= minEndIndex && animationIndex <= maxEndIndex)
                    {
                        Animator anim = adultAlien.GetComponent<Animator>();

                        
                        transform.position = adultAlienAudioSourcePoint.position;
                        aSource.PlayOneShot(adultDialogueClips[adultDialogueIndex]);

                        animationPlaying = true;
                        anim.SetInteger("AudioIndex", animationIndex);
                        anim.SetTrigger("canPlayNextClip");

                        adultDialogueIndex++;
                        animationIndex++;

                        if (animationIndex == 17)
                        {
                            _speakingNPC = speakingNPC.Child;

                        }

                        break;
                    }
                    if (_speakingNPC == speakingNPC.Child && !aSource.isPlaying && childDialogueIndex < childDialogueClips.Count && animationPlaying == false) // && animationIndex >= minEndIndex && animationIndex <= maxEndIndex)
                    {
                        // Turns the Alien Letters and Translation text OFF
                        Animator anim = childAlien.GetComponent<Animator>();
                        anim.SetInteger("AudioIndex", animationIndex);
                        anim.SetTrigger("canPlayNextClip");
                        animationPlaying = true;
                        transform.position = childAlienAudioSourcePoint.position;
                        aSource.PlayOneShot(childDialogueClips[childDialogueIndex]);
                        childDialogueIndex++;
                        //animationIndex++;
                        //_speakingNPC = speakingNPC.Adult;


                        break;
                    }
                }
                break;
            case GameManager.GameStates.Drone:

                if (playerInShack == true)
                {
                    if (_speakingNPC == speakingNPC.Adult && !aSource.isPlaying && adultDialogueIndex < adultDialogueClips.Count && animationPlaying == false && animationIndex <= maxDroneIndex)
                    {
                        Animator anim = adultAlien.GetComponent<Animator>();

                        // Turns the Alien Letters and Translation text ON
                        if (adultDialogueIndex == 0)
                        {
                            alienLettersEmpty.SetActive(true);
                            translatingText.SetActive(true);
                        }

                        transform.position = adultAlienAudioSourcePoint.position;
                        aSource.PlayOneShot(adultDialogueClips[adultDialogueIndex]);

                        if (adultDialogueIndex >= 1)
                        {
                            animationPlaying = true;
                            anim.SetInteger("AudioIndex", animationIndex);
                            anim.SetTrigger("canPlayNextClip");
                        }



                        adultDialogueIndex++;
                        animationIndex++;

                        //if (childDialogueIndex <= childDialogueClips.Count && adultDialogueIndex > 1)
                        if (animationIndex == 2 || animationIndex == 4 || animationIndex == 7 || animationIndex == 10 || animationIndex == 13 || animationIndex == 17)
                        {
                            _speakingNPC = speakingNPC.Child;

                        }
                        //StartCoroutine("Waiter");

                        break;
                    }
                    if (_speakingNPC == speakingNPC.Child && !aSource.isPlaying && childDialogueIndex <= childDialogueClips.Count && animationPlaying == false && animationIndex <= maxDroneIndex)
                    {
                        // Turns the Alien Letters and Translation text OFF
                        Animator anim = childAlien.GetComponent<Animator>();
                        anim.SetInteger("AudioIndex", animationIndex);
                        anim.SetTrigger("canPlayNextClip");
                        animationPlaying = true;
                        transform.position = childAlienAudioSourcePoint.position;
                        aSource.PlayOneShot(childDialogueClips[childDialogueIndex]);
                        childDialogueIndex++;
                        animationIndex++;
                        _speakingNPC = speakingNPC.Adult;

                        //StartCoroutine("Waiter");
                        break;
                    }
                }
                break;
                
            case GameManager.GameStates.Cannon:

                if (playerInShack == true)
                {
                    if (_speakingNPC == speakingNPC.Adult && !aSource.isPlaying && adultDialogueIndex < adultDialogueClips.Count && animationPlaying == false && animationIndex <= maxCannonIndex)
                    {
                        Animator anim = adultAlien.GetComponent<Animator>();

                        // Turns the Alien Letters and Translation text ON
                        if (adultDialogueIndex == 0)
                        {
                            alienLettersEmpty.SetActive(true);
                            translatingText.SetActive(true);
                        }

                        transform.position = adultAlienAudioSourcePoint.position;
                        aSource.PlayOneShot(adultDialogueClips[adultDialogueIndex]);

                        if (adultDialogueIndex >= 1)
                        {
                            animationPlaying = true;
                            anim.SetInteger("AudioIndex", animationIndex);
                            anim.SetTrigger("canPlayNextClip");
                        }



                        adultDialogueIndex++;
                        animationIndex++;

                        if (animationIndex == 13)
                        {
                            _speakingNPC = speakingNPC.Child;

                        }

                        break;
                    }
                    if (_speakingNPC == speakingNPC.Child && !aSource.isPlaying && childDialogueIndex < childDialogueClips.Count && animationPlaying == false)
                    {
                        // Turns the Alien Letters and Translation text OFF
                        Animator anim = childAlien.GetComponent<Animator>();
                        anim.SetInteger("AudioIndex", animationIndex);
                        anim.SetTrigger("canPlayNextClip");
                        animationPlaying = true;
                        transform.position = childAlienAudioSourcePoint.position;
                        aSource.PlayOneShot(childDialogueClips[childDialogueIndex]);
                        childDialogueIndex++;
                        animationIndex++;
                        _speakingNPC = speakingNPC.Adult;


                        break;
                    }
                }
                break;
            case GameManager.GameStates.Free:
                
                break;
            default:
                break;


               


        }
        if (alienLettersEmpty.activeInHierarchy && animationIndex == 3)
        {
            alienLettersEmpty.SetActive(false);
            translatingText.SetActive(false);
        }
    }


    IEnumerator Waiter(float waitTime)
    {

        yield return new WaitForSeconds(waitTime);
    }

}
