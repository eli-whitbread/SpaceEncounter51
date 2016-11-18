﻿using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager _gameManager;
    public int deadBirdsRequired;
    public int birdsDestroyed;


    public enum GameStates { Start, End, Drone, Cannon, Free }
    public GameStates gameStates;
    public float passoutTime;
    public Transform wakeUpPosition;
    public UnityEngine.UI.Image fadeInImage;

    public bool playerPassedOut = false;
    public bool isInGun = false;
    public bool canUseGun = false;
    public bool canUseDrone = false;
    public bool startTimer = false;
    public bool podHasLanded = false;

    void Awake()
    {
        _gameManager = this;
    }

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update () {

        switch (gameStates)
        {
            case GameStates.Start:
                if(podHasLanded)
                {
                    FadeInDropPod();
                }
                if (startTimer)
                {
                    passoutTime -= Time.deltaTime;
                }
                if (passoutTime <= 0)
                {
                    playerPassedOut = true;
                    gameStates = GameStates.Drone;
                }
                break;

            case GameStates.End:

                break;

            case GameStates.Drone:
                break;

            case GameStates.Cannon:
                if (canUseGun == false)
                {
                    Bird_Controller._curBirdController.BirdBathing(true);
                    canUseGun = true;
                }
                
                if (isInGun != true && birdsDestroyed >= deadBirdsRequired)
                {
                    Bird_Controller._curBirdController.BirdBathing(false);

                    gameStates = GameStates.End;
                }
                break;

            case GameStates.Free:
                if(VR_CharacterController._charController.teleportIsOn == false)
                {
                    VR_CharacterController._charController.teleportIsOn = true;
                }
                break;

            default:
                break;
        }

        if (playerPassedOut == true)
        {
            
            
            StartCoroutine("FadeOut");
            //playerPassedOut = false;
        }

    }

    public void DestroyedObject()
    {
        birdsDestroyed++;
    }

    IEnumerator FadeOut()
    {
        gameObject.GetComponent<FadeScript>().BeginFade(1);
        yield return new WaitForSeconds(2);
        VR_CharacterController._charController.MovePlayer(wakeUpPosition);
        StartCoroutine("FadeIn");
        
    }

    IEnumerator FadeIn()
    {
        playerPassedOut = false;
        gameObject.GetComponent<FadeScript>().BeginFade(-1);
        yield return new WaitForSeconds(passoutTime);

    }

    void FadeInDropPod()
    {
        if (fadeInImage.color.a <= 0.5f)
        {
            fadeInImage.gameObject.SetActive(false);
            podHasLanded = false;
        }
        else
        {
            Color tmpColor = new Color(0, 0, 0, 0);
            fadeInImage.color = Color.Lerp(fadeInImage.color, tmpColor, 5f * Time.deltaTime);
            //fadeInImage.gameObject.SetActive(false);
        }
    }
}
