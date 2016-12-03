﻿using UnityEngine;
using System.Collections;

public class ArrowBob : MonoBehaviour {

    public Transform gunPosition;
    public Transform droneLaptop;
    public Transform dronePickup;

    public float amplitudeY = 5.0f;
    public float omegaY = 5.0f;
    private float height;
    float index;

    public bool moveToGun, moveToLaptop, moveToPickup;

	// Use this for initialization
	void Start ()
    {
        height = transform.position.y;
	}
	
	// Update is called once per frame
	void Update ()
    {
        index += Time.deltaTime;
        float y = Mathf.Abs(amplitudeY * Mathf.Sin(omegaY * index));
        transform.localPosition = new Vector3(0, y + height, 0);

        switch (GameManager._gameManager.gameStates)
        {
            case GameManager.GameStates.Start:
                this.GetComponent<SpriteRenderer>().enabled = false;
                moveToGun = false; moveToLaptop = false; moveToPickup = false;
                break;
            case GameManager.GameStates.End:
                this.GetComponent<SpriteRenderer>().enabled = false;
                moveToGun = false;  moveToLaptop = false; moveToPickup = false;
                break;
            case GameManager.GameStates.Drone:
                if(GameManager._gameManager.canUseDrone && !GameManager._gameManager.droneIsActive)
                {
                    // If on Laptop
                    moveToLaptop = true;  moveToGun = false; moveToPickup = false;
                    this.GetComponent<SpriteRenderer>().enabled = true;
                }
                else if(GameManager._gameManager.canUseDrone && GameManager._gameManager.droneIsActive)
                {
                    moveToPickup = true; moveToLaptop = false; moveToGun = false;
                    this.GetComponent<SpriteRenderer>().enabled = true;
                }
                else
                {
                    this.GetComponent<SpriteRenderer>().enabled = true;
                    moveToGun = false;  moveToLaptop = false; moveToPickup = false;
                }

                break;
            case GameManager.GameStates.Cannon:

                moveToGun = true;  moveToLaptop = false; moveToPickup = false;
                this.GetComponent<SpriteRenderer>().enabled = true;

                break;
            case GameManager.GameStates.Free:
                this.GetComponent<SpriteRenderer>().enabled = false;
                moveToGun = false; moveToLaptop = false; moveToPickup = false;
                break;
            default:
                this.GetComponent<SpriteRenderer>().enabled = false;
                moveToGun = false;  moveToLaptop = false; moveToPickup = false;
                break;
        }

        if(moveToGun)
        {
            this.transform.parent.transform.position = gunPosition.position;            
        }
        else if(moveToLaptop)
        {
            this.transform.parent.transform.position = droneLaptop.position;            
        }
        else if(moveToPickup)
        {
            this.transform.parent.transform.position = dronePickup.position;
        }

        Vector3 targetPosition = new Vector3(Camera.main.transform.position.x, this.transform.position.y, Camera.main.transform.position.z);
        this.transform.LookAt(targetPosition);
	}
}
