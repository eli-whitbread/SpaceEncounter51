using UnityEngine;
using System.Collections;

public class DripNTrickle : MonoBehaviour
{
    private AudioSource moistASource;

    public AudioClip trickle;
    public AudioClip drip;
    public AudioClip technolgicalBeep;
    public float startTime = 3;
    public float repeatEvery = 5;

    // Use this for initialization
    void Start ()
    {
        moistASource = GetComponent<AudioSource>();
        InvokeRepeating("PlayDripTrickle", startTime, repeatEvery);
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void PlayDripTrickle()
    {
        moistASource.Stop();
        float rnd = Random.Range(0.4f, 0.8f);
        float moistSwap = Random.Range(0, 7);

        if (moistSwap >= 3 && moistSwap <= 5)
        {
            moistASource.clip = trickle;
        }
        else if(moistSwap <= 2)
        {
            moistASource.clip = drip;
        }
        else
        {
            // Technological beep only at '6'
            moistASource.clip = technolgicalBeep;

        }
                
        if(moistASource.clip == technolgicalBeep)
        {
            moistASource.volume = 0.2f;
        }
        else
        {
            moistASource.volume = rnd;
        }       
        
        moistASource.Play();
    }
    
}
