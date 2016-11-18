using UnityEngine;
using System;

public class VR_PlayerEyeRaycast : MonoBehaviour {

    public event Action<RaycastHit> OnRaycastHit;

    public GunShootScript gunShootScript;

    public bool usingTurret;

    [SerializeField]
    Transform playerCamera;
    [SerializeField]
    LayerMask exclusionLayers;
    [SerializeField]
    PlayerReticle reticle;
    //need ref to VrInput script here for interaction events
    [SerializeField]
    bool showDebugRay;
    [SerializeField]
    float debugRayLength = 5.0f;
    [SerializeField]
    float debugRayDuration = 1.0f;
    [SerializeField]
    float rayLength = 500.0f;

    GameObject currentInteractableObject, lookAtObj;

    void Awake()
    {
        usingTurret = false;
    }

    void Update()
    {
        EyeRaycast();
    }

    void EyeRaycast()
    {
        if (showDebugRay)
        {
            Debug.DrawRay(playerCamera.position, playerCamera.forward * debugRayLength, Color.blue, debugRayDuration);
        }

        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayLength, ~exclusionLayers))
        {
            //Debug.DrawRay(ray.origin, ray.direction, Color.green);
            if (reticle)
            {
                reticle.SetPosition(hit);
                if (hit.collider.CompareTag("Interactable"))
                {
                    lookAtObj = hit.collider.gameObject;
                    VR_InteractableObject obj = lookAtObj.gameObject.GetComponent<VR_InteractableObject>();
                    obj.ShowToolTip(hit.point);
                }
                else
                {
                    if (lookAtObj != null)
                    {
                        VR_InteractableObject obj = lookAtObj.GetComponent<VR_InteractableObject>();
                        obj.HideToolTip();
                        lookAtObj = null;
                    }
                }
                
                if (hit.collider.CompareTag("Interactable") && Input.GetButtonDown("Teleport") && VR_CharacterController._charController.TeleportActive == false)
                {
                    VR_InteractableObject interactableObj = hit.collider.gameObject.GetComponent<VR_InteractableObject>();
                    
                    if (interactableObj != null)
                    {
                        
                        if (currentInteractableObject == null)
                        {
                            currentInteractableObject = hit.collider.gameObject;
                            interactableObj.Activate(hit.point);
                            if(interactableObj.objectType == VR_InteractableObject.InteractableObjectType.Gun)
                            {
                                ButtonColourChanger colChangeScript = hit.collider.GetComponent<ButtonColourChanger>();
                                colChangeScript.ChangeState(true);
                                usingTurret = true;
                            }
                        }
                        if(currentInteractableObject == hit.collider.gameObject)
                        {
                            currentInteractableObject = null;
                            interactableObj.Deactivate();
                            if (interactableObj.objectType == VR_InteractableObject.InteractableObjectType.Gun)
                            {

                                ButtonColourChanger colChangeScript = hit.collider.GetComponent<ButtonColourChanger>();
                                colChangeScript.ChangeState(false);
                                usingTurret = false;
                            }
                            if (interactableObj.objectType == VR_InteractableObject.InteractableObjectType.NPC)
                            {

                                VR_NPCInteractableObject npcInteractObj = hit.collider.GetComponent<VR_NPCInteractableObject>();
                                npcInteractObj.NPCInteracted();
                              
                            }

                        }
                        else if(currentInteractableObject != null && currentInteractableObject != hit.collider.gameObject)
                        {
                            currentInteractableObject.GetComponent<VR_InteractableObject>().Deactivate();
                            if (interactableObj.objectType == VR_InteractableObject.InteractableObjectType.Gun)
                            {
                                ButtonColourChanger colChangeScript = hit.collider.GetComponent<ButtonColourChanger>();
                                colChangeScript.ChangeState(false);
                                usingTurret = false;
                            }
                            currentInteractableObject = hit.collider.gameObject;
                            interactableObj.Activate(hit.point);

                        }
                        
                    }
                }
                if (gunShootScript != null && usingTurret == true)
                {
                    gunShootScript.Target = hit.collider.gameObject.transform;
                }
            }
            //if (OnRaycastHit != null)
            //{
            //    OnRaycastHit(hit);
            //}
        }
        else
        {
            if (reticle)
            {
                reticle.SetPosition();
                gunShootScript.Target = null;
            }
        }
    }
}
