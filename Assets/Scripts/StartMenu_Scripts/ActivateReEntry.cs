using UnityEngine;
using System.Collections;

public class ActivateReEntry : MonoBehaviour {

    public Light point;
    [SerializeField] private AudioSource ShuttleAudioSourceAlarm;
    [SerializeField] private AudioSource ShuttleAudioSourceBurn;
    [SerializeField] private AudioSource ShuttleAudioSourceVoice;
    [SerializeField] private AudioSource ShuttleAudioSourceVoice2;
    [SerializeField] private AudioClip AlarmClip;
    [SerializeField] private AudioClip Voice2Clip;
    [SerializeField] private AudioClip BurnClip;
    [SerializeField] private AudioClip VoiceClip;

    // Use this for initialization
    void Start () {
        ShuttleAudioSourceAlarm.loop = true;
        ShuttleAudioSourceBurn.loop = true;
        ShuttleAudioSourceVoice.loop = true;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void StartGame()
    {
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
}
