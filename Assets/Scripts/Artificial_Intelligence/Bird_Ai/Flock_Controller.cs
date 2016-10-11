using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Flock_Controller : MonoBehaviour
{

    [Range(0.0f, 5.0f)]
    public float cohRange, sepRange, allRange;

    [Range(0.0f, 50.0f)]
    public float maxSpeed, maxVeloc;


    public float cohAmp, sepAmp, allAmp, flockAmp;

    public Transform[] flockingPoints;
    public Transform[] swoopingPoints;
    public Transform birdPrefab;
    public int spawnNumber;
    public int totalSwooping = 3;
    private int currentSwooping = 0;

    private List<GameObject> birdList;


    // Use this for initialization
    void Start()
    {
        StartUp();
    }

    // Update is called once per frame
    void Update()
    {
        ChooseRandomSwoop();
    }
    private void StartUp()
    {
        birdList = new List<GameObject>();
        SpawnBirds(birdPrefab, spawnNumber);
        birdList.AddRange(GameObject.FindGameObjectsWithTag("Bird_AI"));

        foreach (GameObject bird in birdList)
        {
            foreach (Transform point in flockingPoints)
            {
                bird.GetComponent<Vermin_AI>().AddFlockGoal = point;
            }
            foreach (Transform point in swoopingPoints)
            {
                bird.GetComponent<Vermin_AI>().AddSwoop = point;
            }
        }
    }
    private void SpawnBirds(Transform prefab, int numBirds)
    {
        for (int i = 0; i < numBirds; i++)
        {
            Instantiate(prefab, new Vector3(Random.Range(-3, 3), 0, Random.Range(-3, 3)), Quaternion.identity);
        }
    }
    public List<Vermin_AI> getNeighbourFlock(Vermin_AI bird, float range)
    {
        List<Vermin_AI> thisFlock = new List<Vermin_AI>();

        foreach (GameObject otherBird in birdList)
        {
            if (otherBird.GetComponent<Vermin_AI>() == bird)
            {
                continue;
            }

            if (Vector3.Distance(bird.transform.position, otherBird.transform.position) <= range)
            {
                thisFlock.Add(otherBird.GetComponent<Vermin_AI>());
            }
        }
        return thisFlock;
    }
    private void ChooseRandomSwoop()
    {
        foreach (GameObject bird in birdList)
        {
            if (RandomBoolean() && currentSwooping <= totalSwooping)
            {
                bird.GetComponent<Vermin_AI>().flocking = false;
                currentSwooping++;
            }
        }
    }
    public void MinusSwooper()
    {
        currentSwooping--;
    }

    private bool RandomBoolean()
    {
        if (Random.value >= .8)
        {
            return true;
        }
        return false;
    }
}
