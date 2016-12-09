using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Drone : MonoBehaviour
{

    public float acceleration, rotationSpeed, rotationAngle;
    public Transform[] rayPoints;
    public Text droneText;

    private float yVelocity;
    private float upForce = 200f, height = 3f, rotTime = 4f, elevateSpeed = 2f, elevateIncrement = 1.5f;

    public bool pickUp;
    public bool techFound;
    private Rigidbody rigBody;

    public GameObject drone;
    public VR_CharacterController playerController;


    // Use this for initialization
    void Start()
    {
        StartUp();
        droneText.text = "Press interact to pickup";
    }

    // Update is called once per frame
    void Update()
    {
        if (pickUp)
        {
            droneText.text = "you have the tech";
        }
        if (techFound)
        {
            droneText.text = "tech returned b to exit";
            playerController.lockControls = false;
            drone.SetActive(false);
        }
    }
    private void FixedUpdate()
    {
        Movement();
    }
    private void Movement()
    {
        Hovering();
        Moving();
        RaiseLower();
    }
    private void Hovering()
    {
        RaycastHit hit;

        foreach (Transform rayPoint in rayPoints)
        {
            float distance;
            Vector3 down;

            if (Physics.Raycast(rayPoint.position, rayPoint.up * -1, out hit, height))
            {
                distance = 1 - (hit.distance / height);
                down = transform.up * upForce * distance;
                down = down * Time.deltaTime * rigBody.mass;

                rigBody.AddForceAtPosition(down, rayPoint.position);
            }
        }
    }
    private void RaiseLower()
    {
        if (Input.GetButton("TeleportEnable") && height < 7)
        {
            height += elevateIncrement * elevateSpeed * Time.deltaTime;
        }
        if (Input.GetButton("Lower") && height > 3)
        {
            height -= elevateIncrement * elevateSpeed * Time.deltaTime;
        }
    }
    private void Moving()
    {
        if (Physics.Raycast(transform.position, transform.up * -1, 3f))
        {
            rigBody.drag = 1;
            Vector3 forward = transform.forward * acceleration * Input.GetAxis("Vertical");
            forward = forward * Time.deltaTime * rigBody.mass;

            rigBody.AddForce(forward);
        }
        else
        {
            rigBody.drag = 0;
        }
        Vector3 turning = Vector3.up * rotationSpeed * Input.GetAxis("Horizontal");

        if (Input.GetAxisRaw("Horizontal") < 0 || Input.GetAxisRaw("Horizontal") > 0)
        {
            turning = turning * Time.deltaTime * rigBody.mass;
            rigBody.AddTorque(turning);
        }
        else
        {
            rigBody.AddTorque(-rigBody.angularVelocity);
        }

        Vector3 newRotation = transform.eulerAngles;
        newRotation.z = Mathf.SmoothDampAngle(newRotation.z, Input.GetAxis("Horizontal") * -rotationAngle, ref yVelocity, rotTime);

        transform.eulerAngles = newRotation;
    }
    private void OnTriggerStay(Collider collider)
    {
        if (collider.tag == "PickUp")
        {
            if (Input.GetButtonDown("Teleport"))
            {
                pickUp = true;
                collider.gameObject.SetActive(false);
            }
        }
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "End" && pickUp)
        {
            GameManager._gameManager.gameStates = GameManager.GameStates.Cannon;
            techFound = true;
        }
    }
    private void StartUp()
    {
        rigBody = GetComponent<Rigidbody>();
    }
}
