using UnityEngine;
using System.Collections;
using UnityEngine.VR;

public class VR_CharacterController : MonoBehaviour {

    public float moveSpeed;
    public Transform myCamera;
    public bool lockControls;

    float yPos;

   void Start()
    {

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
