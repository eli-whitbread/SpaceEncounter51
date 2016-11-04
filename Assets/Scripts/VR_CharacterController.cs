using UnityEngine;
using System.Collections;
using UnityEngine.VR;
using UnityEngine.UI;

public class VR_CharacterController : MonoBehaviour {

    public float moveSpeed, mouseLookSpeed, blinkFadeOutTime, blinkFadeInTime, blinkFadeTimeMultiplyer;
    public Transform myCamera;
    public bool lockControls, teleportIsOn = false, teleportTooFar = false, isUsingGun;
    public GameObject teleportPrefab, teleportCapsule, teleportBase;
    public ParticleSystem teleportParticle1, teleportParticle2;
    public PlayerReticle playerReticleScript;
    public CanvasGroup blinkCanvas;
    public ParticleSystem dustStormParticles;
    public GameObject ParticleOnOff;
    public GameObject Terrain;
    [SerializeField]
    private float groundLevel;
    private bool TeleportActive = false;
    float yPos, blinkAlpha;
    Vector3 temp = Vector3.zero;
    float maxTeleportDistance = 15f;

    public bool IsUsingGun
    {
        get { return isUsingGun; }
        set { isUsingGun = value; }
    }

    void Awake()
    {
        isUsingGun = false;
    }
    void Start()
    {
        teleportPrefab.SetActive(false);
        yPos = transform.position.y;
        groundLevel = Terrain.transform.position.y;
    }

    public void EnableGunControl()
    {
        gameObject.GetComponent<VR_PlayerEyeRaycast>().usingTurret = true;
    }

    public void DisableGunControl()
    {
        gameObject.GetComponent<VR_PlayerEyeRaycast>().usingTurret = false;
    }

    void Update()
    {
        if (VRDevice.isPresent)
        {
            Quaternion lookDirection = InputTracking.GetLocalRotation(VRNode.Head);
            lookDirection.x = 0;
            lookDirection.z = 0;
            transform.rotation = lookDirection;
        }
        else
        {
            float lookX = Input.GetAxis("Mouse X") * mouseLookSpeed;
            float lookY = Input.GetAxis("Mouse Y") * mouseLookSpeed;
            Vector3 newRot = new Vector3(-lookY, lookX, 0);

            newRot.z = 0;

            myCamera.Rotate(newRot);

            newRot.x = 0;
            newRot.z = 0;
            transform.Rotate(newRot);
        }
        
        if (!lockControls)
        {
            if(TeleportActive)
            {                
                temp = playerReticleScript.ReticleTransform.position;
                
                if (Vector3.SqrMagnitude(temp - transform.position) <= maxTeleportDistance * maxTeleportDistance)
                {
                    // Teleport is within range, can teleport here. Teleport will be blue coloured.
                    teleportPrefab.SetActive(true);
                    teleportTooFar = false;

                    // Set Teleport Capsule to Blue                    
                    Color32 blue = new Color32(12, 43, 135, 128);              
                    teleportCapsule.GetComponent<Renderer>().material.SetColor("_TintColor", blue);

                    // Set Particle System Start Color so to Show Material Blue Colour
                    blue = new Color32(255, 255, 255, 255);
                    teleportParticle1.startColor = blue;

                    // Set Particle System Start Color so to Show Material Blue Colour
                    blue = new Color32(7, 224, 244, 216);
                    teleportParticle2.startColor = blue;

                    // Set Teleport Base Color so to Show Material Blue Colour
                    blue = new Color32(128, 128, 128, 179);
                    teleportBase.GetComponent<Renderer>().material.SetColor("_TintColor", blue);
                }
                else if(Vector3.SqrMagnitude(temp - transform.position) > (maxTeleportDistance + 30f) * (maxTeleportDistance + 30f))
                {
                    // TURN TELEPORTER OFF, AS TOO FAR

                    // Teleport is made inactive (invisible)
                    teleportTooFar = true;

                    teleportPrefab.SetActive(false);
                }
                else
                {
                    // TELEPORT TOO FAR, BUT WITHIN VISIBLE DISTANCE
                    // -- cannot teleport here. Teleport will be red coloured.]
                    teleportPrefab.SetActive(true);
                    teleportTooFar = true;

                    // Set Teleport Capsule to red
                    Color32 red = new Color32(255, 0, 0, 127);
                    teleportCapsule.GetComponent<Renderer>().material.SetColor("_TintColor", red);

                    // Set Particle System Start Color so to Show Material red Colour
                    red = new Color32(255, 0, 0, 255);
                    teleportParticle1.startColor = red;

                    // Set Particle System Start Color so to Show Material red Colour
                    red = new Color32(163, 0, 30, 216);
                    teleportParticle2.startColor = red;

                    // Set Teleport Base Color so to Show Material red Colour
                    red = new Color32(255, 0, 0, 179);
                    teleportBase.GetComponent<Renderer>().material.SetColor("_TintColor", red);
                }
                teleportPrefab.transform.position = new Vector3(temp.x, groundLevel, temp.z);
            }
                        
                if (Input.GetMouseButtonDown(1) || Input.GetButtonDown("TeleportEnable"))
                {
                    TeleportActive = true;
                    temp = playerReticleScript.ReticleTransform.position;

                    teleportPrefab.SetActive(true);
                }
                else if (Input.GetMouseButtonUp(1) || Input.GetButtonUp("TeleportEnable"))
                {
                    teleportPrefab.SetActive(false);
                    TeleportActive = false;
                }

                if ((TeleportActive && Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Teleport") && TeleportActive == true) && !teleportTooFar)
                {
                    teleportIsOn = true;
                    blinkAlpha = 1.0f;
                    blinkCanvas.alpha = blinkAlpha;
                    temp = playerReticleScript.ReticleTransform.position;
                    temp.y = transform.position.y;
                    transform.position = temp;
                    //InputTracking.Recenter();

                    teleportPrefab.SetActive(false);
                    TeleportActive = false;
                }
            

            if (Input.GetAxis("Horizontal") != 0)
            {
                float sideMove = Input.GetAxis("Horizontal");
                transform.Translate(transform.right * sideMove * moveSpeed * Time.deltaTime);
            }
            if (Input.GetAxis("Vertical") != 0)
            {
                float forwardMove = Input.GetAxis("Vertical");
                transform.Translate(transform.forward * forwardMove * moveSpeed * Time.deltaTime);
            }
            transform.position = new Vector3(transform.position.x, yPos, transform.position.z);

            if (teleportIsOn)
            {
                blinkAlpha = Mathf.Lerp(blinkAlpha, 0, blinkFadeInTime * blinkFadeTimeMultiplyer);
                blinkCanvas.alpha = blinkAlpha;

                if (blinkAlpha <= 0.01f)
                { 
                    blinkCanvas.alpha = 0;
                    teleportIsOn = false;
                }
            }
        }

        dustStormParticles.transform.rotation = new Quaternion(dustStormParticles.transform.rotation.x, 0, dustStormParticles.transform.rotation.z, dustStormParticles.transform.rotation.w);
    }

    void OnTriggerEnter(Collider hit)
    {
        if(hit.tag == "Dust Cloud")
        {
            ParticleOnOff.SetActive(true);
        }
    }

    void OnTriggerExit(Collider hit)
    {
        if (hit.tag == "Dust Cloud")
        {
            ParticleOnOff.SetActive(false);
        }
    }
}
