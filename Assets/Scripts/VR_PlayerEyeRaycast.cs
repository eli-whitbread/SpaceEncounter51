﻿using UnityEngine;
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

    GameObject currentInteractableObject;

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
            Debug.DrawRay(ray.origin, ray.direction, Color.green);
            if (reticle)
            {
                reticle.SetPosition(hit);
                if(hit.collider.CompareTag("Interactable") && Input.GetButtonDown("TeleportEnable"))
                {
                    VR_InteractableObject interactableObj = hit.collider.gameObject.GetComponent<VR_InteractableObject>();
                    if(interactableObj != null)
                    {
                        if (currentInteractableObject == null)
                        {
                            currentInteractableObject = hit.collider.gameObject;
                            interactableObj.Activate();
                            if(interactableObj.objectType == VR_InteractableObject.InteractableObjectType.Gun)
                            {
                                usingTurret = true;
                            }
                        }
                        else if(currentInteractableObject == hit.collider.gameObject)
                        {
                            currentInteractableObject = null;
                            interactableObj.Deactivate();
                            if (interactableObj.objectType == VR_InteractableObject.InteractableObjectType.Gun)
                            {
                                usingTurret = false;
                            }
                        }
                        else if(currentInteractableObject != null && currentInteractableObject != hit.collider.gameObject)
                        {
                            currentInteractableObject.GetComponent<VR_InteractableObject>().Deactivate();
                            if (interactableObj.objectType == VR_InteractableObject.InteractableObjectType.Gun)
                            {
                                usingTurret = false;
                            }
                            currentInteractableObject = hit.collider.gameObject;
                            interactableObj.Activate();

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
