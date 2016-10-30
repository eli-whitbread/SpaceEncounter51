using UnityEngine;
using System.Collections;
using UnityEngine.VR;
using UnityEngine.UI;

public class VR_CharacterController : MonoBehaviour {

    public float moveSpeed, mouseLookSpeed, blinkFadeOutTime, blinkFadeInTime, blinkFadeTimeMultiplyer;
    public Transform myCamera;
    public bool lockControls, teleport = false, isUsingGun;
    public GameObject teleportPrefab;
    public PlayerReticle playerReticleScript;
    public CanvasGroup blinkCanvas;
    private bool TeleportActive = false;
    float yPos, blinkAlpha;
    Vector3 temp = Vector3.zero;
    //private TeleportUnable teleportUnableScriptAccess;

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
        //teleportUnableScriptAccess = teleportPrefab.GetComponent<TeleportUnable>();
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
                teleportPrefab.transform.position = new Vector3(temp.x, 0, temp.z);
            }
            
            if(Input.GetMouseButtonDown(1) || Input.GetButtonDown("TeleportEnable"))
            {
                //if (teleportUnableScriptAccess.cannotTeleport == false)
                //{
                    TeleportActive = true;
                    temp = playerReticleScript.ReticleTransform.position;
                    //teleportPrefab.transform.position = temp;
                    teleportPrefab.SetActive(true);
                //}
            }
            else if(Input.GetMouseButtonUp(1) || Input.GetButtonUp("TeleportEnable"))
            {                
                teleportPrefab.SetActive(false);
                TeleportActive = false;
            }

            if(TeleportActive && Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Teleport") && TeleportActive == true)
            {
                teleport = true;
                blinkAlpha = 1.0f;
                blinkCanvas.alpha = blinkAlpha;
                temp = playerReticleScript.ReticleTransform.position;
                temp.y = transform.position.y;
                transform.position = temp;
                //InputTracking.Recenter();
               
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

            if (teleport)
            {
                blinkAlpha = Mathf.Lerp(blinkAlpha, 0, blinkFadeInTime * blinkFadeTimeMultiplyer);
                blinkCanvas.alpha = blinkAlpha;

                if (blinkAlpha <= 0.01f)
                { 
                    blinkCanvas.alpha = 0;
                    teleport = false;
                }
            }
        }

        
    }

    
}
