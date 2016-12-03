using UnityEngine;
using System.Collections;

public class ParticleSystemSwapShack : MonoBehaviour
{
    public static ParticleSystemSwapShack _instance;

    public GameObject levelDustParticle;
    public GameObject Player;

    public bool inTrigger = false;
    float distance;
    public float closeDistance = 4.0f;
    // Use this for initialization

    void Start ()
    {
        if(_instance != null)
        {
            _instance = null;
            _instance = this;
        }
        else
        {
            _instance = this;
        }
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 offset = Player.transform.position - transform.position;

        distance = offset.sqrMagnitude;

        if(distance <= closeDistance * closeDistance)
        {
            inTrigger = true;
        }
        else
        {
            inTrigger = false;
        }

        if(inTrigger)
        {
            levelDustParticle.SetActive(false);
            VR_CharacterController._charController.playerAudioSource.volume = 0.02f;
        }
        else
        {
            levelDustParticle.SetActive(true);
            if(GameManager._gameManager.startTimer)
            {
                VR_CharacterController._charController.playerAudioSource.volume = 0.06f;
            }
            
        }
	}
}
