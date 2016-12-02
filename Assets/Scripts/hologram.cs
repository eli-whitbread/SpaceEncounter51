using UnityEngine;
using System.Collections;

public class hologram : MonoBehaviour
{
    public GameObject screen;
    public GameObject cone;
    public Transform startPos;
    public Transform endPos;
    public GameObject player;
    public GameObject drone;
    private bool opening;
    private bool closing;
    private bool spin;

    public float speed = 1.0F;
    private float startTime;

    private float journeyLength;

    private Vector3 startscale;
    private Vector3 endscale;

    // Use this for initialization
    void Start()
    {
        startTime = Time.time;
        spin = true;
        journeyLength = Vector3.Distance(startPos.position, endPos.position);
        startscale = screen.transform.localScale;
        endscale = new Vector3(1.7f, 1.7f, 1.7f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("B"))
        {
            player.GetComponent<VR_CharacterController>().lockControls = false;
            drone.SetActive(false);

        }
        if (spin)
        {
            screen.transform.Rotate(0, 1, 0);
        }
        else
        {
            screen.transform.eulerAngles = new Vector3(0, 0,0);
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            opening = false;
            if (Input.GetButtonDown("Teleport"))
            {
                player.GetComponent<VR_CharacterController>().lockControls = true;
                drone.SetActive(true);
            }
            
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            spin = true;
        }
    }
   
}
