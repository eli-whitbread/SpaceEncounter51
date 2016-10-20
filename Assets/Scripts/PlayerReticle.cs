using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerReticle : MonoBehaviour {

    [SerializeField]
    float defaultDistance;
    [SerializeField]
    bool alignToNormals;
    [SerializeField]
    Image reticleImage;
    [SerializeField]
    Transform reticleTransform;
    [SerializeField]
    Transform playerCamera;

    public float reticleHitFloatingDistance;

    Vector3 originalScale;
    Quaternion originalRotation;

    public bool UseNormal
    {
        get { return alignToNormals; }
        set { alignToNormals = value; }
    }

    public Transform ReticleTransform
    {
        get { return reticleTransform; }
    }

    private void Awake()
    {
        //store the initial scale and rotation
        originalRotation = reticleTransform.localRotation;
        originalScale = reticleTransform.localScale;

    }

    public void Hide()
    {
        reticleImage.enabled = false;
    }

    public void Show()
    {
        reticleImage.enabled = true;
    }

    //used when VREyeRaycaster does not hit anything - reset all values to default
    public void SetPosition()
    {
        reticleTransform.position = playerCamera.position + playerCamera.forward * defaultDistance;
        reticleTransform.localScale = originalScale * defaultDistance;
        reticleTransform.localRotation = originalRotation;
    }

    //used when VREyeRaycaster does hit something
    public void SetPosition(RaycastHit hit)
    {
        reticleTransform.position = hit.point;
        reticleTransform.localScale = originalScale * (hit.distance - reticleHitFloatingDistance);

        if (alignToNormals)
        {
            reticleTransform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
        }
        else
        {
            reticleTransform.localRotation = originalRotation;
        }
    }
}
