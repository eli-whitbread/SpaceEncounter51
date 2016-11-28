using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Fallpod : MonoBehaviour
{
    private float fraction = 0, currentPos;
    public float lerpPodDropSpeed = 0.5f;
    public GameObject FallingdropPod, groundDropPod;
    public GameObject cameraPlayer, whereHeadShouldBe;
    public GameObject startPosEmpty, endPosEmpty;
    public Image fadeImg;
    public Transform playerStartPOS;
    public bool hasLanded, startFade;

    public float howLongFadeTakes = 4.0f;
    private float currentTime = 0.0f, distance;


    void Awake()
    {
        fadeImg.rectTransform.localScale = new Vector2(Screen.width, Screen.height);
    }
    // Use this for initialization
    void Start()
    {

        currentPos = startPosEmpty.transform.position.y;
        distance = startPosEmpty.transform.position.y - endPosEmpty.transform.position.y;
        //Debug.Log("Distance Equals = " + distance);
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasLanded)
        {
            // Determine whether start fade should begin
            if (currentPos < (distance / 4) + 1f && currentPos > (distance / 4) - 1f)
            {
                //Debug.Log("Distance Midway Equals = " + currentPos);
                //UnityEditor.EditorApplication.isPaused = true;
                startFade = true;
            }


            if (currentPos > 0.1f)
            {
                // Continue moving Pod
                fraction += Time.deltaTime * lerpPodDropSpeed;

                currentPos = Mathf.Lerp(currentPos, endPosEmpty.transform.position.y, fraction);

                FallingdropPod.transform.position = new Vector3(FallingdropPod.transform.position.x, currentPos, FallingdropPod.transform.position.z);

                cameraPlayer.transform.position = whereHeadShouldBe.transform.position;
                //cameraPlayer.transform.rotation = whereHeadShouldBe.transform.rotation;
            }
            //else
            //{
            //    // Pod has reached the ground
            //    hasLanded = true;
            //    //Debug.Log("Has Landed");
            //}

            if (startFade)
            {
                // Check whether counted seconds equals to fade time - if it does, start fade
                FadeToBlack();
            }
        }
        else
        {

            LastFunction();
        }


    }

    void FadeToBlack()
    {
        // Lerp the colour of the image between itself and black.

        //Debug.Log(fadeImg.color.a);

        if (fadeImg.color.a >= 0.85f)
        {
            fadeImg.color = new Color(0, 0, 0, 255);
            hasLanded = true;
        }
        else
        {
            fadeImg.color = Color.Lerp(fadeImg.color, Color.black, howLongFadeTakes * Time.deltaTime);
        }
    }

    void LastFunction()
    {
        // Place camera where it's meant to be
        //cameraPlayer.transform.position = endHeadPosition.transform.position;

        // Position Pod how it's meant to be (rotation and position)
        //FallingdropPod.transform.position = endPosEmpty.transform.position;
        //FallingdropPod.transform.rotation = endPosEmpty.transform.rotation;
        cameraPlayer.transform.position = playerStartPOS.position;
        //cameraPlayer.transform.rotation = playerStartPOS.rotation;
        UnityEngine.VR.InputTracking.Recenter();
        FallingdropPod.SetActive(false);
        groundDropPod.SetActive(true);

        GameManager._gameManager.podHasLanded = true;
        //fadeImg.gameObject.SetActive(false);
        

        // Turn this script off! (Nothing will run following this)
        gameObject.GetComponent<Fallpod>().enabled = false;
    }
}
