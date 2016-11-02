using UnityEngine;
using System.Collections;

public class DroneCamera : MonoBehaviour {

    public Transform target;
    public float distUp, distBack, minHeight;

    private Vector3 posVe;



    private Camera cam;



	// Use this for initialization
	void Start ()
    {
        StartUp();
       
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
    private void FixedUpdate()
    {
        Vector3 newPos = target.position + (target.forward * distBack);
        newPos.y = Mathf.Max(newPos.y + distUp, minHeight);

        transform.position = Vector3.SmoothDamp(transform.position, newPos, ref posVe, 0.18f);

        Vector3 focalpoint = target.position + (target.forward * 5);
        transform.LookAt(focalpoint);
    }
    
    private void DrawRays()
    {

       

    }
    private void StartUp()
    {
        cam = GetComponent<Camera>();
    }
}
