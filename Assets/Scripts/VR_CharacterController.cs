using UnityEngine;
using System.Collections;
using UnityEngine.VR;
using UnityEngine.UI;

public class VR_CharacterController : MonoBehaviour {

    public static VR_CharacterController _charController;

    public float moveSpeed, mouseLookSpeed, blinkFadeOutTime, blinkFadeInTime, blinkFadeTimeMultiplyer, snapTurnAmount, startGameFadeInTime;
    public Transform myCamera, directionChecker;
    public float vrCameraRenderScale = 1.0f, collisionDistance;
    public bool lockControls, teleportIsOn = false, teleportTooFar = false, isUsingGun;
    public GameObject teleportPrefab, teleportCapsule, teleportBase, cameraAnchor;
    public ParticleSystem teleportParticle1, teleportParticle2;
    public PlayerReticle playerReticleScript;
    public CanvasGroup blinkCanvas;
    public GameObject ParticleOnOff, BrokenGlassEffect;
    public GameObject Terrain;
    [SerializeField]
    private float groundLevel;
    public bool TeleportActive = false, snapTurnActive = false, gameStart;
    float yPos, blinkAlpha;
    Vector3 temp = Vector3.zero;
    public float maxTeleportDistance = 15f;
    int teleportOffDistance = 25;
    public AudioSource playerAudioSource;
    public bool playerIsInShack;

    //for Mouse Look
    Vector2 _mouseAbsolute;
    Vector2 _smoothMouse;
    public Vector2 clampInDegrees = new Vector2(360, 180);
    public bool lockCursor;
    public Vector2 sensitivity = new Vector2(2, 2);
    public Vector2 smoothing = new Vector2(3, 3);
    public Vector2 targetDirection;
    public Vector2 targetCharacterDirection;

    public bool IsUsingGun
    {
        get { return isUsingGun; }
        set { isUsingGun = value; }
    }

    public bool PlayerIsInShack
    {
        get { return playerIsInShack; }
        set { playerIsInShack = value; }
    }

    void Awake()
    {
        blinkAlpha = 1.0f;
        gameStart = true;
        teleportIsOn = false;
        _charController = this;
        isUsingGun = false;
        VRSettings.renderScale = vrCameraRenderScale;
        lockControls = true;
        
    }
    void Start()
    {
        
        teleportPrefab.SetActive(false);
        yPos = transform.position.y;
        groundLevel = Terrain.transform.position.y;

        //for mouse look
        // Set target direction to the camera's initial orientation.
        targetDirection = myCamera.transform.localRotation.eulerAngles;

        // Set target direction for the character body to its inital state.
        targetCharacterDirection = transform.localRotation.eulerAngles;
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
        if (gameStart)
        {
            blinkAlpha = Mathf.Lerp(blinkAlpha, 0, startGameFadeInTime * Time.deltaTime);
            blinkCanvas.alpha = blinkAlpha;

            if (blinkAlpha <= 0.01f)
            {
                blinkCanvas.alpha = 0;
                gameStart = false;
            }
        }
        if (VRDevice.isPresent)
        {
            Quaternion lookDirection = InputTracking.GetLocalRotation(VRNode.Head);
            lookDirection.x = 0;
            lookDirection.z = 0;
            transform.rotation = lookDirection;
        }
        else
        {
            MouseLook();
        }
        
        if (!lockControls)
        {
            //blink turn
            if(Input.GetAxis("SnapTurn") == 0 && snapTurnActive == true)
            {
                snapTurnActive = false;
            }
            if(Input.GetAxis("SnapTurn") != 0 && snapTurnActive == false)
            {
                snapTurnActive = true;
                //float snapDir = Input.GetAxis("SnapTurn");

                teleportIsOn = true;
                blinkAlpha = 1.0f;
                blinkCanvas.alpha = blinkAlpha;
                Quaternion snapRot = cameraAnchor.transform.rotation;
                //snapRot.y = snapRot.y + (snapTurnAmount * snapDir);
                cameraAnchor.transform.rotation = Quaternion.AngleAxis(snapRot.y + 25.0f, Vector3.up);
                //InputTracking.Recenter();
                
            }            
            if (TeleportActive)
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
                else if(Vector3.SqrMagnitude(temp - transform.position) > (maxTeleportDistance + teleportOffDistance) * (maxTeleportDistance + teleportOffDistance))
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

            //if ((TeleportActive && Input.GetKeyDown(KeyCode.Space)) || (Input.GetButtonDown("Teleport") && TeleportActive == true) && !teleportTooFar)
            if (!teleportTooFar)
            {
                if ((Input.GetButtonDown("Teleport") || Input.GetMouseButtonDown(0)) && TeleportActive == true)
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
            }

            //check if the player's intended path is blocked
            if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                Vector3 checkDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                directionChecker.transform.rotation = Quaternion.LookRotation(checkDir);
                RaycastHit colHit;
                if(Physics.Raycast(directionChecker.position, directionChecker.forward, out colHit, collisionDistance))
                {
                    if (colHit.collider.CompareTag("Drone"))
                    {
                    }
                    else
                    {
                        return;

                    }
                }
                
            }

            //WARNING CONTROLs HAVE BEEN FLIPPED

            if (Input.GetAxis("Horizontal") != 0)
            {
                float sideMove = Input.GetAxis("Horizontal");
                transform.Translate(myCamera.forward * sideMove * moveSpeed * Time.deltaTime);
            }
            if (Input.GetAxis("Vertical") != 0)
            {
                float forwardMove = Input.GetAxis("Vertical");
                transform.Translate(transform.right * -forwardMove * moveSpeed * Time.deltaTime);
            }
            transform.position = new Vector3(myCamera.position.x, yPos, transform.position.z);

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
        
        BrokenGlassEffect.transform.rotation = myCamera.transform.rotation;
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

    public void MovePlayer(Transform pos)
    {
        Vector3 newPos = pos.position;
        newPos.y = transform.position.y;
        transform.position = newPos;
    }

    public void MovePlayerWithBlink(Transform pos)
    {
        blinkCanvas.alpha = blinkAlpha;
        teleportIsOn = true;
        Vector3 newPos = pos.position;
        newPos.y = pos.position.y;
        transform.position = newPos;
    }

    void MouseLook()
    {

        //Adapted from https://forum.unity3d.com/threads/a-free-simple-smooth-mouselook.73117/

        // Allow the script to clamp based on a desired target value.
        var targetOrientation = Quaternion.Euler(targetDirection);
        var targetCharacterOrientation = Quaternion.Euler(targetCharacterDirection);

        // Get raw mouse input for a cleaner reading on more sensitive mice.
        var mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        // Scale input against the sensitivity setting and multiply that against the smoothing value.
        mouseDelta = Vector2.Scale(mouseDelta, new Vector2(sensitivity.x * smoothing.x, sensitivity.y * smoothing.y));

        // Interpolate mouse movement over time to apply smoothing delta.
        _smoothMouse.x = Mathf.Lerp(_smoothMouse.x, mouseDelta.x, 1f / smoothing.x);
        _smoothMouse.y = Mathf.Lerp(_smoothMouse.y, mouseDelta.y, 1f / smoothing.y);

        // Find the absolute mouse movement value from point zero.
        _mouseAbsolute += _smoothMouse;

        // Clamp and apply the local x value first, so as not to be affected by world transforms.
        if (clampInDegrees.x < 360)
            _mouseAbsolute.x = Mathf.Clamp(_mouseAbsolute.x, -clampInDegrees.x * 0.5f, clampInDegrees.x * 0.5f);

        var xRotation = Quaternion.AngleAxis(-_mouseAbsolute.y, targetOrientation * Vector3.right);
        myCamera.transform.localRotation = xRotation;

        // Then clamp and apply the global y value.
        if (clampInDegrees.y < 360)
            _mouseAbsolute.y = Mathf.Clamp(_mouseAbsolute.y, -clampInDegrees.y * 0.5f, clampInDegrees.y * 0.5f);

        myCamera.transform.localRotation *= targetOrientation;


        var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, transform.up);
        transform.localRotation = yRotation;
        transform.localRotation *= targetCharacterOrientation;

        //else
        //{
        //    var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, transform.InverseTransformDirection(Vector3.up));
        //    myCamera.transform.localRotation *= yRotation;
        //}


        //rotate the player capsule (forward move vector)
        //Quaternion Bodyrot = myCamera.transform.rotation;
        //Bodyrot.x = 0;
        //Bodyrot.z = 0;
        //transform.localRotation = Bodyrot;
    }
}
