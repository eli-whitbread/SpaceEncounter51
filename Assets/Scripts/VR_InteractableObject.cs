using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class VR_InteractableObject : MonoBehaviour {

    public enum InteractableObjectType { None, Pickup, Switch, Gun, NPC, Ejector, ChangeScene, ActivateTurret, DroneScreen};
    public InteractableObjectType objectType;

    public Transform toolTipAnchorPoint;
    public GameObject secondaryObject;
    public Canvas toolTipCanvas;
    public Text toolTipText;
    public string toolTip;
    public float tipHoverHeight;
    

    void Awake()
    {
        if (toolTipCanvas != null)
        {
            toolTipCanvas.enabled = false;
        }
    }

	public void Activate(Vector3 rayHitPos)
    {
        switch (objectType)
        {
            case InteractableObjectType.Pickup:
                VR_Pickupable pickupScript = gameObject.GetComponent<VR_Pickupable>();
                if(pickupScript != null)
                {
                    pickupScript.EnableInteraction(rayHitPos);
                }
                break;
            case InteractableObjectType.Switch:
                DoorEject door = secondaryObject.GetComponent<DoorEject>();
                door.EjectorOn(this.gameObject);
                break;
            case InteractableObjectType.Ejector:

                break;
            case InteractableObjectType.Gun:
                GunShootScript shootScript = secondaryObject.GetComponent<GunShootScript>();
                if (shootScript != null && GameManager._gameManager.canUseGun)
                {
                    shootScript.EnableGunControl();
                    secondaryObject.GetComponent<GunAiming>().GunActive = true;
                    GameManager._gameManager.isInGun = true;
                }
                break;
            case InteractableObjectType.NPC:
                break;
            case InteractableObjectType.ChangeScene:
                SceneManager.LoadScene(0);
                break;
            case InteractableObjectType.ActivateTurret:
                ToggleTurret togTurret = secondaryObject.GetComponent<ToggleTurret>();
                togTurret.TeleportPlayer();
                break;
            case InteractableObjectType.DroneScreen:
                break;
            default:
                break;
        }
    }
    public void Deactivate()
    {
        switch (objectType)
        {
            case InteractableObjectType.Pickup:
                VR_Pickupable pickupScript = gameObject.GetComponent<VR_Pickupable>();
                if (pickupScript != null)
                {
                    pickupScript.DisableInteraction();
                }
                break;
            case InteractableObjectType.Switch:
                break;
            case InteractableObjectType.Gun:
                GunShootScript shootScript = secondaryObject.GetComponent<GunShootScript>();
                if (shootScript != null)
                {
                    Debug.Log("Deactivate Gun");
                    shootScript.DisableGunControl();
                    secondaryObject.GetComponent<GunAiming>().GunActive = false;
                    GameManager._gameManager.isInGun = false;
                }
                break;
            default:
                break;
        }
    }

    public void ShowToolTip(Vector3 pos)
    {
        if (toolTipCanvas != null)
        {
            MeshRenderer meshRnd = gameObject.GetComponent<MeshRenderer>();
            if (meshRnd != null)
            {
                toolTipCanvas.transform.position = new Vector3(transform.position.x, meshRnd.bounds.size.y + tipHoverHeight, transform.position.z);
            }
            else
            {
                toolTipCanvas.transform.position = new Vector3(transform.position.x, toolTipAnchorPoint.position.y + tipHoverHeight, transform.position.z);
            }

            toolTipCanvas.enabled = true;
            if (toolTip == "")
            {
                toolTipText.text = "Interact";
            }
            else
            {
                toolTipText.text = toolTip;
            }
        }
        
    }
    public void HideToolTip()
    {
        if (toolTipCanvas != null)
        {
            toolTipCanvas.enabled = false;
        }
    }
}
