using UnityEngine;
using System.Collections;

public class VR_InteractableObject : MonoBehaviour {

    public enum InteractableObjectType { Pickup, Switch, Gun};
    public InteractableObjectType objectType;

    public GameObject secondaryObject;


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
                break;
            case InteractableObjectType.Gun:
                GunShootScript shootScript = secondaryObject.GetComponent<GunShootScript>();
                if(shootScript != null)
                {
                    shootScript.EnableGunControl();
                    secondaryObject.GetComponent<GunAiming>().GunActive = true;
                }
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
                }
                break;
            default:
                break;
        }
    }
}
