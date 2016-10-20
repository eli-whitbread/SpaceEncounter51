using UnityEngine;
using System.Collections;
using UnityEngine.VR;

public class VR_CharacterController : MonoBehaviour {

    public float moveSpeed;
    public Transform myCamera;
    public bool lockControls;
    public GameObject teleportPrefab;
    public PlayerReticle playerReticleScript;
    private bool TeleportActive = false;
    float yPos;

   void Start()
    {
        teleportPrefab.SetActive(false);
        yPos = transform.position.y;
    }

    void Update()
    {

        Quaternion lookDirection = InputTracking.GetLocalRotation(VRNode.Head);
        lookDirection.x = 0;
        lookDirection.z = 0;
        transform.rotation = lookDirection;
        
        if (!lockControls)
        {
            if(Input.GetMouseButtonDown(1))
            {
                TeleportActive = true;
                Vector3 temp = playerReticleScript.ReticleTransform.position;
                teleportPrefab.transform.position = temp;
                teleportPrefab.SetActive(true);
            }
            else if(Input.GetMouseButtonUp(1))
            {                
                teleportPrefab.SetActive(false);
                TeleportActive = false;
            }

            if(TeleportActive && Input.GetKeyDown(KeyCode.Space))
            {
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
        }
    }
}
