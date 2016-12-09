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
        moistASource.volume = 0.17f;
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void PlayDripTrickle()
    {
        if(!ParticleSystemSwapShack._instance.lightningOn)
        {
            moistASource.Stop();
            float moistSwap = Random.Range(0, 7);
            moistASource.volume = 0.17f;

            if (moistSwap >= 4 && moistSwap <= 5)
            {
                moistASource.clip = trickle;
            }
            else if(moistSwap <= 3)
            {
                moistASource.clip = drip;
            }
            else
            {
                moistASource.volume = 0.04f;
                moistASource.clip = technolgicalBeep;

            }       
        
            moistASource.Play();
        }
        
    }
    
}
