using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager _gameManager;
    public int deadBirdsRequired;
    public int birdsDestroyed;
    public float waitTime;

    public enum GameStates { Start, End, Drone, Cannon, Free }
    public GameStates gameStates;
    public float passoutTime, wakeUpTime;
    public Transform wakeUpPosition;
    //public AudioClip BodyDragSound;
    public AudioClip BodyFallSound;
    private AudioSource managerAudioSource;
    public UnityEngine.UI.Image fadeImg;

    public bool playerPassedOut = false, playerWoozie = true;
    public bool isInGun = false;
    public bool canUseGun = false;
    public bool canUseDrone = false;
    public bool startTimer = false;
    public bool podHasLanded = false;
    public bool playerHasWokenUp = false;
    public bool droneIsActive = false;

    //private bool bodyDragHasPlayed = false;
    private bool bodyFallHasPlayed = false;

    float curTime;
    bool countDown = false, hasWokenUp = false;

    void Awake()
    {
        _gameManager = this;
        curTime = passoutTime;
    }

	// Use this for initialization
	void Start ()
    {
        canUseGun = false;
        managerAudioSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {

        switch (gameStates)
        {
            case GameStates.Start:
                Bird_Controller._curBirdController.BirdBathing(true);

                if (countDown == true)
                {
                    passoutTime -= Time.deltaTime;
                }

                if (podHasLanded)
                {                        
                    FadeInDropPod();
                }

                if (startTimer)
                {
                    if(countDown == false)
                    {
                        countDown = true;
                    }
                    if(passoutTime <= 5.0f && playerWoozie == true)
                    {
                        StartCoroutine("FadeToBlack");
                        waitTime = 1.3f;


                        TurnPodMeshColliderOnOff._instance.SwitchCollider(true);

                    }
                }
                if (passoutTime <= 0)
                {
                    if (!bodyFallHasPlayed)
                    {
                        managerAudioSource.loop = false;
                        managerAudioSource.volume = 0.5f;
                        managerAudioSource.clip = BodyFallSound;
                        managerAudioSource.Play();
                        bodyFallHasPlayed = true;
                    }
                    if(!playerPassedOut)
                    {
                        StartCoroutine("PlayerPassOut");                    
                    }                    
                }
                break;

            case GameStates.End:

                break;

            case GameStates.Drone:

                break;

            case GameStates.Cannon:
                canUseGun = true;
                
                if (isInGun != true && birdsDestroyed >= deadBirdsRequired)
                {
                    Bird_Controller._curBirdController.BirdBathing(false);

                    gameStates = GameStates.End;
                }
                break;

            case GameStates.Free:
                if(VR_CharacterController._charController.teleportIsOn == false)
                {
                    VR_CharacterController._charController.teleportIsOn = true;
                }
                break;

            default:
                break;
        }

        if (playerPassedOut == true)
        {

            if(passoutTime < -5.0f)
            {
                passoutTime = curTime;
                if(!playerHasWokenUp)
                {
                    StartCoroutine("PlayerWakeUp");
                }
                gameStates = GameStates.Drone;
            }
            //playerPassedOut = false;
        }

    }

    public void DestroyedObject()
    {
        birdsDestroyed++;
    }

    float t = 0.0f;

    IEnumerator FadeToBlack()
    {
        // Lerp the colour of the image between itself and black.  
        t += 0.2f * Time.deltaTime;      
        fadeImg.color = Color.Lerp(fadeImg.color, new Color(0,0,0,1), t);        

        yield return new WaitForSeconds(1.0f);
        playerWoozie = false;
        StartCoroutine("FadeInFromBlack");
    }

    IEnumerator FadeInFromBlack()
    {
        t = 0.0f;
        t += 4f * Time.deltaTime;
        fadeImg.color = Color.Lerp(fadeImg.color, new Color(0, 0, 0, 0), t);
        yield return new WaitForSeconds(0);        
    }

    float j = 0.0f;

    IEnumerator PlayerPassOut()
    {
        t = 0.0f;
        j += 0.2f * Time.deltaTime;
        fadeImg.color = Color.Lerp(fadeImg.color, new Color(0, 0, 0, 1), j);        

        yield return new WaitForSeconds(wakeUpTime);
        if (hasWokenUp == false)
        {
            VR_CharacterController._charController.MovePlayer(wakeUpPosition);
            hasWokenUp = true;
        }
        playerPassedOut = true;
    }
    
    IEnumerator PlayerWakeUp()
    {
        t += 4f * Time.deltaTime;
        fadeImg.color = Color.Lerp(fadeImg.color, new Color(0, 0, 0, 0), t);
        yield return new WaitForSeconds(1.5f);
        fadeImg.color = new Color(0, 0, 0, 0);
        playerHasWokenUp = true;
    }

    void FadeInDropPod()
    {
        if (fadeImg.color.a <= 0.5f)
        {
            //fadeImg.gameObject.SetActive(false);
            fadeImg.color = new Color(0, 0, 0, 0);
            podHasLanded = false;
        }
        else
        {
            Color tmpColor = new Color(0, 0, 0, 0);
            fadeImg.color = Color.Lerp(fadeImg.color, tmpColor, 5f * Time.deltaTime);
            //fadeInImage.gameObject.SetActive(false);
        }
    }
    
}
