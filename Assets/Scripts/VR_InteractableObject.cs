using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VR_InteractableObject : MonoBehaviour {

    public enum InteractableObjectType { None, Pickup, Switch, Gun, NPC, Ejector};
    public InteractableObjectType objectType;

    public GameObject secondaryObject;
    public Canvas toolTipCanvas;
    public Text toolTipText;
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
                if(shootScript != null && GameManager._gameManager.canUseGun)
                {
                    
                    shootScript.EnableGunControl();
                    secondaryObject.GetComponent<GunAiming>().GunActive = true;
                    GameManager._gameManager.isInGun = true;
                }
                break;
            case InteractableObjectType.NPC:
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
            toolTipCanvas.transform.position = new Vector3(transform.position.x, gameObject.GetComponent<MeshRenderer>().bounds.size.y + tipHoverHeight, transform.position.z);

            toolTipCanvas.enabled = true;
            toolTipText.text = "Press E to Interact";
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
