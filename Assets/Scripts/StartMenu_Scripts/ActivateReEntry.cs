using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ActivateReEntry : MonoBehaviour {

    public Light point;
    public float WaitBeforeLoad = 0.6f;
    [SerializeField]
    private GameObject _MenuManager;
    public float howLongFadeTakes = 10f;
    bool fadeRunning = false;

    [SerializeField] private AudioSource ShuttleAudioSourceAlarm;
    [SerializeField] private AudioSource ShuttleAudioSourceBurn;
    [SerializeField] private AudioSource ShuttleAudioSourceVoice;
    [SerializeField] private AudioSource ShuttleAudioSourceVoice2;
    [SerializeField] private AudioClip AlarmClip;
    [SerializeField] private AudioClip Voice2Clip;
    [SerializeField] private AudioClip BurnClip;
    [SerializeField] private AudioClip VoiceClip;
    public Image fadeImg;

    void Awake()
    {
        fadeImg.rectTransform.localScale = new Vector2(Screen.width, Screen.height);
    }

    // Use this for initialization
    void Start () {
        ShuttleAudioSourceAlarm.loop = true;
        ShuttleAudioSourceBurn.loop = true;
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if(fadeRunning)
        {
            fadeImg.color = Color.Lerp(fadeImg.color, Color.black, howLongFadeTakes * Time.deltaTime);
        }
	}

    public void StartGame()
    {
        StartCoroutine("ChangeLevel");
        ChangeLevel();
        point.GetComponent<Light>().enabled = true;
        ShuttleAudioSourceAlarm.clip = AlarmClip;
        ShuttleAudioSourceAlarm.Play();
        ShuttleAudioSourceBurn.clip = BurnClip;
        ShuttleAudioSourceBurn.Play();
        ShuttleAudioSourceVoice.clip = VoiceClip;
        ShuttleAudioSourceVoice.Play();        
    }

    public void StopSounds()
    {
        point.GetComponent<Light>().enabled = false;
        ShuttleAudioSourceAlarm.Stop();
        ShuttleAudioSourceBurn.Stop();
        ShuttleAudioSourceVoice.Stop();
        ShuttleAudioSourceVoice2.clip = Voice2Clip;
        ShuttleAudioSourceVoice2.Play();
    }

    IEnumerator ChangeLevel()
    {
        yield return new WaitForSeconds(WaitBeforeLoad);
        StartCoroutine("FadeToBlack");
        //float fadeTime = _MenuManager.GetComponent<FadeScript>().BeginFade(1);
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene(1);
        //Debug.Log("Level should be loading");
    }

    IEnumerator FadeToBlack()
    {
        // Lerp the colour of the image between itself and black.

        //Debug.Log(fadeImg.color.a);

        if (fadeImg.color.a >= 0.85f)
        {
            fadeImg.color = new Color(0, 0, 0, 255);
        }
        else
        {
            fadeRunning = true;
            
        }

        yield return new WaitForSeconds(0);
    }

}
