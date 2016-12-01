using UnityEngine;
using System.Collections;

public class PlayLightningSound : MonoBehaviour
{
    AudioSource lightningSource;
    public AudioClip thunderSmall;
    public AudioClip thunderLarge;
    public float startTime = 3;
    public float repeatEvery = 8;
    public UnityEngine.UI.Image lightningFlashCanvas;

	// Use this for initialization
	void Start ()
    {
        lightningSource = GetComponent<AudioSource>();
        lightningFlashCanvas.rectTransform.localScale = new Vector2(Screen.width, Screen.height);
        InvokeRepeating("PlayThunderSound", startTime, repeatEvery);
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void PlayThunderSound()
    {
        lightningSource.Stop();
        float rnd = Random.Range(0.75f, 0.9f);
        float thunderSwap = Random.Range(0, 10);
        Debug.Log(thunderSwap);
        if (thunderSwap > 6)
        {
            if(thunderSwap % 2 == 0)
            {
                lightningSource.clip = thunderLarge;
                if (rnd >= 0.75f)
                {
                    lightningFlashCanvas.gameObject.SetActive(true);
                    StartCoroutine("FlashCanvasOff");
                }
            }
            else
            {
                lightningSource.clip = thunderSmall;
            }
            
            
        }
        else
        {
            lightningSource.clip = thunderSmall;
        }

        if(GameManager._gameManager.startTimer == false)
        {
            lightningSource.volume = 0.1f;
        }
        else
        {
            lightningSource.volume = rnd;
        }
        

        lightningSource.Play();
    }

    IEnumerator FlashCanvasOff()
    {
        yield return new WaitForSeconds(0.35f);
        lightningFlashCanvas.gameObject.SetActive(false);
    }
}
