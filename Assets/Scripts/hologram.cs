using UnityEngine;
using System.Collections;

public class hologram : MonoBehaviour
{
    public GameObject screen;
    public GameObject player;
    public GameObject drone;
   
    private bool playSound;
    public float speed = 1.0F;
    private float startTime;

    private float journeyLength;

    private Vector3 startscale;
    private Vector3 endscale;

    private AudioSource jetsound;

    void Awake()
    {
        jetsound = GetComponent<AudioSource>();
    }
    // Use this for initialization
    void Start()
    {
       
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("B"))
        {
            player.GetComponent<VR_CharacterController>().lockControls = false;
            drone.SetActive(false);
            playSound = false;

        }
        if (playSound)
        {
            jetsound.Play();
        }
        else
        {
            jetsound.Stop();
        }
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            
            if (Input.GetButtonDown("Teleport"))
            {
                player.GetComponent<VR_CharacterController>().lockControls = true;
                GameManager._gameManager.droneIsActive = true;
                drone.SetActive(true);
                playSound = true;
            }
            
        }
    }
    
   
}
