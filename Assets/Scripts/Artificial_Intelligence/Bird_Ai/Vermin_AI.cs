using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Vermin_AI : MonoBehaviour
{
   


    public bool flocking = true;

    public Vector3 newPos, newVeloc, newAcc;
    //public Transform currentSwoopGoal;

    
    private int currentFlockTarg = 0;
    private int currentSwoopTarg = 0;
    private Flock_Controller flock_Controller;
    private Vector3 dir;
    private Quaternion rotation;
    private float speed = 20;
    private float turnSpeed = 10;
    private bool oneTime = false;
    private bool returnSwoop;




    public Transform AddSwoop { set { swoopPoints.Add(value); } }
    public Transform AddFlockGoal { set { flockingGoals.Add(value); } }
    //remember to convert these to arrays in any method yo uuse them
    public List<Transform> swoopPoints = new List<Transform>();
    public List<Transform> flockingGoals = new List<Transform>();

    // Use this for initialization
    void Start()
    {
        StartUp();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }
    private void StartUp()
    {
        newPos = transform.position;
        newVeloc = new Vector3(Random.Range(-3, 3), 0, Random.Range(-3, 3));
        flock_Controller = FindObjectOfType<Flock_Controller>();
    }
    private void Movement()
    {
        if (flocking)
        {
            UpdateFlocking();
        }
        else if (!flocking)
        {
            SwoopMovement();
            UpdateSwooping();
        }
    }
    private void UpdateFlocking()
    {
        float deltaTime = Time.deltaTime;

        newAcc = JoinVectors();

        newAcc = Vector3.ClampMagnitude(newAcc, flock_Controller.maxSpeed);

        newVeloc = newVeloc + newAcc * deltaTime;

        newVeloc = Vector3.ClampMagnitude(newVeloc, flock_Controller.maxVeloc);

        newPos = newPos + newVeloc * deltaTime;

        transform.position = newPos;

        if ((CheckDistanceToPoint(transform.position, flockingGoals[currentFlockTarg].transform.position)) < 3)
        {
            ChooseFlockTarget();
        }
        if (newVeloc.magnitude > 0)
        {
            transform.LookAt(newPos + newVeloc);
        }
    }
    private void SwoopMovement()
    {
        if (returnSwoop)
        {
            dir = (flockingGoals[currentSwoopTarg].transform.position - transform.position).normalized;
        }
        else
        {
            dir = (swoopPoints[currentSwoopTarg].transform.position - transform.position).normalized;
        }

        rotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnSpeed * Time.deltaTime);
        transform.position += transform.forward * speed * Time.deltaTime;

    }
    private void UpdateSwooping()
    {
        if (!returnSwoop)
        {
            ChooseSwoopTarget();
            if ((CheckDistanceToPoint(transform.position, swoopPoints[currentSwoopTarg].transform.position)) < 3)
            {
                if (!returnSwoop)
                {
                    returnSwoop = true;
                }
            }
        }
        else if(returnSwoop && (CheckDistanceToPoint(transform.position, flockingGoals[currentSwoopTarg].transform.position)) <= 5)
        {
            flocking = true;
            oneTime = false;
            returnSwoop = false;
            flock_Controller.MinusSwooper();

        }
    }
    private void ChooseSwoopTarget()
    {
        if (!oneTime)
        {
            currentSwoopTarg = Random.Range(0, swoopPoints.Count);
            oneTime = true;
        }
    }
    private void ChooseFlockTarget()
    {
        currentFlockTarg = Random.Range(0, flockingGoals.Count);
    }
    private float CheckDistanceToPoint(Vector3 pos1, Vector3 pos2)
    {
        float distance = Vector3.Distance(pos1, pos2);
        return distance;
    }

    private Vector3 GoalSeeking()
    {
        Vector3 goalVector = new Vector3();

        flockingGoals.ToArray();

        goalVector += flockingGoals[currentFlockTarg].transform.position;

        goalVector -= this.newPos;

        goalVector = Vector3.Normalize(goalVector);
        return goalVector;
    }
    private Vector3 Cohesion()
    {
        Vector3 cohVector = new Vector3();

        var flock = flock_Controller.getNeighbourFlock(this, flock_Controller.cohRange);

        if (flock.Count == 0)
        {
            return cohVector;
        }
        foreach (var bird in flock)
        {
            cohVector += bird.newPos;
        }

        cohVector /= flock.Count;

        cohVector = cohVector - this.newPos;

        cohVector = Vector3.Normalize(cohVector);
        return cohVector;
    }
    private Vector3 Seperation()
    {
        Vector3 sepVector = new Vector3();

        var flock = flock_Controller.getNeighbourFlock(this, flock_Controller.sepRange);

        if (flock.Count == 0)
        {
            return sepVector;
        }
        foreach (var bird in flock)
        {
            Vector3 towardsThis = this.newPos - bird.newPos;

            if (towardsThis.magnitude > 0)
            {
                sepVector += towardsThis.normalized / towardsThis.magnitude;
            }
        }

        sepVector = Vector3.Normalize(sepVector);
        return sepVector;
    }
    private Vector3 Allignment()
    {
        Vector3 allVector = new Vector3();

        var flock = flock_Controller.getNeighbourFlock(this, flock_Controller.allRange);

        if (flock.Count == 0)
        {
            return allVector;
        }

        foreach (var bird in flock)
        {
            allVector += bird.newVeloc;
        }

        allVector = Vector3.Normalize(allVector);
        return allVector;
    }
    private Vector3 JoinVectors()
    {
        Vector3 joint = new Vector3();

        joint = Cohesion() * flock_Controller.cohAmp + Seperation() * flock_Controller.sepAmp + GoalSeeking() * flock_Controller.flockAmp + Allignment() * flock_Controller.allAmp;
        return joint;
    }
}
