using UnityEngine;
using System.Collections;

public class Jet_Rotate : MonoBehaviour
{

    public float tiltAmount = 0.5f;
    public float smooth = 1000.0f;
    public GameObject[] thrusterModels;

    float tiltX;
    float tiltY;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MoveThruster();
    }
    private void MoveThruster()
    {
        foreach (GameObject thruster in thrusterModels)
        {

            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                tiltX += (-Input.GetAxisRaw("Horizontal")/smooth * tiltAmount * Time.deltaTime);
                tiltX = Mathf.Clamp(tiltX, -5, 5);
                thruster.transform.eulerAngles = new Vector3(thruster.transform.eulerAngles.x, thruster.transform.eulerAngles.y, tiltX);

                thruster.transform.Rotate(0, 0, tiltX);
            }
            else
            {
                thruster.transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.identity, Time.deltaTime * smooth);
            }


            if (Input.GetAxisRaw("Vertical") != 0)
            {
                tiltY -= (-Input.GetAxisRaw("Vertical") * tiltAmount * Time.deltaTime);
                tiltY= Mathf.Clamp(tiltY, -5, 5);
                thruster.transform.eulerAngles = new Vector3(tiltY, thruster.transform.eulerAngles.y, thruster.transform.eulerAngles.z);

                thruster.transform.Rotate(tiltY, 0,0 );
            }
            else
            {
               //thruster.transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.identity, 2 * Time.deltaTime);
            }


        }

    }

}
