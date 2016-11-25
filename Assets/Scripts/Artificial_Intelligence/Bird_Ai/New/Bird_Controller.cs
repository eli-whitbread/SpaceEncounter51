using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bird_Controller : MonoBehaviour
{


    public static Bird_Controller _curBirdController;




    //public GameObject birdy;
    //public int birdNumber;
    public Transform[] swoops;
    public Transform[] flocks;
    public int amountSwoop = 0;
    private List<Bird_Agent> birdys = new List<Bird_Agent>();




   void Awake()
    {
        _curBirdController = this;
    }
    // Use this for initialization
    void Start()
    {
        StartUp();
    }

    // Update is called once per frame
    void Update()
    {
        MakeBirdySwoop();
    }
    private void StartUp()
    {
        
        birdys.AddRange(GameObject.FindObjectsOfType<Bird_Agent>());
        AssignBirdys();
       // BirdBathing(false);

    }
   
    private void AssignBirdys()
    {
        foreach (Bird_Agent bird in birdys)
        {
            bird.swoopPoints.AddRange(swoops);
            bird.flockGoals.AddRange(flocks);
            bird.bController = gameObject.GetComponent<Bird_Controller>();
            bird.gameObject.transform.parent = transform;
        }
    }
    private void MakeBirdySwoop()
    {
        if (amountSwoop <= 10)
        {
            int r = Random.Range(0, birdys.Count);

            birdys[r].flocking = false;
            amountSwoop++;
        }

    }
    public void BirdBathing(bool on)
    {
        if (on)
        {
            for (int i = 0; i < birdys.Count; i++)
            {
                if (!birdys[i].gameObject.activeInHierarchy)
                {
                    birdys[i].gameObject.SetActive(true);

                }

            }
        }
        if (!on)
        {
            for (int i = 0; i < birdys.Count; i++)
            {
                if (birdys[i].gameObject.activeInHierarchy)
                {
                    birdys[i].gameObject.SetActive(false);

                }

            }
        }
    }
}
