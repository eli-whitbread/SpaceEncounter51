using UnityEngine;
using System.Collections;
using UnityEngine.VR;
using UnityEngine.UI;

public class VR_CharacterController : MonoBehaviour {

    public float moveSpeed, mouseLookSpeed, blinkFadeOutTime, blinkFadeInTime, blinkFadeTimeMultiplyer;
    public Transform myCamera;
    public bool lockControls, teleport = false;
    public GameObject teleportPrefab;
    public PlayerReticle playerReticleScript;
    public CanvasGroup blinkCanvas;
    private bool TeleportActive = false;
    float yPos, blinkAlpha;

    void Start()
    {
        teleportPrefab.SetActive(false);
        yPos = transform.position.y;
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
            
            if(Input.GetMouseButtonDown(1) || Input.GetButtonDown("TeleportEnable"))
            {
                TeleportActive = true;
                Vector3 temp = playerReticleScript.ReticleTransform.position;
                teleportPrefab.transform.position = temp;
                teleportPrefab.SetActive(true);
            }
            else if(Input.GetMouseButtonUp(1) || Input.GetButtonUp("TeleportEnable"))
            {                
                teleportPrefab.SetActive(false);
                TeleportActive = false;
            }

            if(TeleportActive && Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Teleport"))
            {
                teleport = true;
                blinkAlpha = 1.0f;
                blinkCanvas.alpha = blinkAlpha;
                Vector3 temp = playerReticleScript.ReticleTransform.position;
                temp.y = transform.position.y;
                transform.position = temp;
               
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
                Debug.Log(blinkCanvas.alpha.ToString());

                if (blinkAlpha <= 0.01f)
                { 
                    blinkCanvas.alpha = 0;
                    teleport = false;
                }
            }
        }

        
    }

    
}
