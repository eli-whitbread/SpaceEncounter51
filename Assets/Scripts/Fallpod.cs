using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Fallpod : MonoBehaviour
{
    private float fraction = 0, currentPos;
    public float lerpPodDropSpeed = 0.5f;
    public GameObject FallingdropPod, groundDropPod;
    public GameObject cameraPlayer, whereHeadShouldBe;
    public GameObject startPosEmpty, endPosEmpty;
    public Image fadeImg;
    public Transform playerStartPOS;
    public bool hasLanded, startFade;
    private AudioSource fallpodAudioSource;
    public AudioSource fallpodParticleEffectAudioSource;
    public AudioSource fallPodDescentSource;
    public AudioClip reentryBurnSound;
    public AudioClip reentryAlarmSound;
    public AudioClip shuttleDescentWindHissSound;
    public GameObject reentryBurnParticleEffect;
    public GameObject windParticleEffect;

    static float t = 0.0f;
    public float howLongFadeTakes = 4.0f;
    private float currentTime = 0.0f, distance;


    void Awake()
    {
        fadeImg.rectTransform.localScale = new Vector2(Screen.width, Screen.height);
        fallpodAudioSource = GetComponent<AudioSource>();
    }
    // Use this for initialization
    void Start()
    {
        // Start reentry burn sound
        fallpodParticleEffectAudioSource.clip = reentryBurnSound;
        fallPodDescentSource.clip = shuttleDescentWindHissSound;
        //fallpodParticleEffectAudioSource.volume = 0.05f;
        fallpodParticleEffectAudioSource.Play();

        // Play Reentry Alarm Sound
        fallpodAudioSource.clip = reentryAlarmSound;
        fallpodAudioSource.volume = 0.05f;
        fallpodAudioSource.Play();

        currentPos = startPosEmpty.transform.position.y;
        distance = startPosEmpty.transform.position.y - endPosEmpty.transform.position.y;
        //Debug.Log("Distance Equals = " + distance);
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasLanded)
        {
            // Determine whether start fade should begin
            if (currentPos < (distance / 4) + 1f && currentPos > (distance / 4) - 1f)
            {
                //Debug.Log("Distance Midway Equals = " + currentPos);
                //UnityEditor.EditorApplication.isPaused = true;
                fallpodParticleEffectAudioSource.Stop();
                startFade = true;
            }

            if(currentPos < (distance * 0.7f))
            {
                if(fallPodDescentSource.isPlaying)
                {
                    fallpodParticleEffectAudioSource.Stop();
                }
                //StartCoroutine("PlayHissDescentSound");

                reentryBurnParticleEffect.GetComponent<ParticleSystem>().Stop();                
            }

            if (currentPos < (distance * 0.8f) && !fallPodDescentSource.isPlaying)
            {
                fallPodDescentSource.Play();
                windParticleEffect.SetActive(true);
            }

            if (fallPodDescentSource.isPlaying)
            {
                t += 0.05f * Time.deltaTime;

                fallPodDescentSource.volume = Mathf.Lerp(0.75f, 0.0f, t);
                fallpodAudioSource.volume = Mathf.Lerp(0.05f, 0.003f, t);

                if(fallPodDescentSource.volume <= 0.2)
                {
                    windParticleEffect.SetActive(false);
                }
            }

            if (currentPos > 0.1f)
            {
                // Continue moving Pod
                fraction += Time.deltaTime * (lerpPodDropSpeed / 100);

                currentPos = Mathf.Lerp(currentPos, endPosEmpty.transform.position.y, fraction);

                FallingdropPod.transform.position = new Vector3(FallingdropPod.transform.position.x, currentPos, FallingdropPod.transform.position.z);

                cameraPlayer.transform.position = whereHeadShouldBe.transform.position;
            }

            if (startFade)
            {
                // Check whether counted seconds equals to fade time - if it does, start fade
                FadeToBlack();
            }
        }
        else
        {

            LastFunction();
        }


    }

    void FadeToBlack()
    {
        // Lerp the colour of the image between itself and black.

        //Debug.Log(fadeImg.color.a);

        if (fadeImg.color.a >= 0.85f)
        {
            fadeImg.color = new Color(0, 0, 0, 255);
            hasLanded = true;
        }
        else
        {
            fadeImg.color = Color.Lerp(fadeImg.color, Color.black, howLongFadeTakes * Time.deltaTime);
        }
    }

    void LastFunction()
    {
        fallpodAudioSource.Stop();
        
        // Start Dust Storm Audio
        VR_CharacterController._charController.playerAudioSource.volume = 0.01f;

        // Place camera where it's meant to be
        //cameraPlayer.transform.position = endHeadPosition.transform.position;

        // Position Pod how it's meant to be (rotation and position)
        //FallingdropPod.transform.position = endPosEmpty.transform.position;
        //FallingdropPod.transform.rotation = endPosEmpty.transform.rotation;
        cameraPlayer.transform.position = playerStartPOS.position;
        //cameraPlayer.transform.rotation = playerStartPOS.rotation;
        UnityEngine.VR.InputTracking.Recenter();
        FallingdropPod.SetActive(false);
        groundDropPod.SetActive(true);

        GameManager._gameManager.podHasLanded = true;
        //fadeImg.gameObject.SetActive(false);
        

        // Turn this script off! (Nothing will run following this)
        gameObject.GetComponent<Fallpod>().enabled = false;
    }
    
}
