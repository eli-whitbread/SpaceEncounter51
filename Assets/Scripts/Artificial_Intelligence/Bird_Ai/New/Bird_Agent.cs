using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Bird_Agent : MonoBehaviour
{

    public float maxSpeed;
    public float maxVeloc;
    public float cohAmp, sepAmp, allAmp, goalAmp;
    public Vector3 newPos, newVeloc, newAcc;
    Vector3 coh = new Vector3();
    Vector3 sep = new Vector3();
    Vector3 all = new Vector3();
    Vector3 goal = new Vector3();
    //--------------Swooping-----------
    public int currentSwoopTarg = 0;
    public int currentFlockTarg = 0;
    private Vector3 dir;
    private Quaternion rotation;
    private float speed = 10;
    private float turnspeed = 10;
    private bool oneTime = false;
    private bool returnSwoop;
    public Bird_Controller bController;
    public bool flocking = true;


    //---------------------------------




    public int flockOrder = 0;
    public int groupSize = 0;
    private float neighboursRange = 3.0f;
    public List<Bird_Agent> cur_Group = new List<Bird_Agent>();
    public List<Transform> swoopPoints = new List<Transform>();
    public List<Transform> flockGoals = new List<Transform>();


    private Rigidbody rigid;

    private void Awake()
    {
        StartUp();
    }
    // Use this for initialization
    void Start()
    {


    }
    // Update is called once per frame
    void Update()
    {
        FlockHQ();

    }
    private void FlockHQ()
    {
        int tempSize = (Physics.OverlapSphere(transform.position, neighboursRange).Length);
        if (groupSize != tempSize)
        {
            cur_Group = GatherNeihbours(neighboursRange);
            groupSize = cur_Group.Count;
        }
        if (flocking)
        {
            UpdateFlight();

        }
        else if (!flocking)
        {
            UpdateSwoop();
            SwoopMovement();
        }
    }
    private void UpdateFlight()
    {
        float deltaTime = Time.deltaTime;

        newAcc = FlockVector();

        newAcc = Vector3.ClampMagnitude(newAcc, maxSpeed);

        newVeloc = newVeloc + newAcc * deltaTime;

        newVeloc = Vector3.ClampMagnitude(newVeloc, maxVeloc);

        newPos = newPos + newVeloc * deltaTime;
        transform.position = newPos;

        if (CheckDistanceToPoint(transform.position, flockGoals[currentFlockTarg].transform.position) <= 3)
        {
            ChooseFlockTarget();
        }
        if (newVeloc.magnitude > 0)
        {
            transform.LookAt(newPos + newVeloc);
        }

    }
    private void UpdateSwoop()
    {
        if (!returnSwoop)
        {
            ChooseSwoopTarget();
            if ((CheckDistanceToPoint(transform.position, swoopPoints[currentSwoopTarg].transform.position)) <= 3)
            {
                if (!returnSwoop)
                {
                    returnSwoop = true;
                }
            }
        }
        else if (returnSwoop && (CheckDistanceToPoint(transform.position, flockGoals[currentSwoopTarg].transform.position)) <= 3)
        {
            flocking = true;
            oneTime = false;
            returnSwoop = false;
            bController.amountSwoop--;

        }
    }
    private void SwoopMovement()
    {
        if (returnSwoop)
        {
            dir = (flockGoals[currentSwoopTarg].transform.position - transform.position).normalized;
        }
        else
        {
            dir = (swoopPoints[currentSwoopTarg].transform.position - transform.position).normalized;
        }

        rotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnspeed * Time.deltaTime);
        transform.position += transform.forward * speed * Time.deltaTime;
    }
    private void ChooseFlockTarget()
    {
        currentFlockTarg = Random.Range(0, flockGoals.Count - 1);
    }
    private void ChooseSwoopTarget()
    {
        if (!oneTime)
        {
            currentSwoopTarg = Random.Range(0, swoopPoints.Count);
            oneTime = true;
        }
    }
    private Vector3 FlockVector()
    {

        Vector3 flockVector = new Vector3();

        for (int i = 0; i < cur_Group.Count; i++)
        {
            if (cur_Group[0] == null || !cur_Group[0].isActiveAndEnabled)
            {
                cur_Group.RemoveAt(i);
            }
        }

        switch (flockOrder)
        {
            case 0:
                if (cur_Group.Count > 1)
                {
                    foreach (var bird in cur_Group)
                    {

                        coh += bird.newPos;


                    }
                    coh /= cur_Group.Count;
                    coh = coh - this.newPos;
                    coh = Vector3.Normalize(coh);
                    coh = coh * cohAmp;

                }
                flockOrder++;
                break;
            case 1:
                if (cur_Group.Count > 1)
                {

                    foreach (var bird in cur_Group)
                    {
                        Vector3 towardsThis = this.newPos - bird.newPos;

                        if (towardsThis.magnitude > 0)
                        {
                            sep += towardsThis.normalized / towardsThis.magnitude;
                        }
                    }
                    sep = Vector3.Normalize(sep);
                    sep = sep * sepAmp;

                }
                flockOrder++;
                break;
            case 2:
                if (cur_Group.Count > 1)
                {
                    foreach (var bird in cur_Group)
                    {
                        all += bird.newVeloc;
                    }
                    all = Vector3.Normalize(all);
                    all = all * allAmp;
                }
                flockOrder++;
                break;
            case 3:


                goal = flockGoals[currentFlockTarg].transform.position;
                goal -= this.newPos;
                goal *= goalAmp;

                flockOrder = 0;
                break;
        }

        flockVector = coh + sep + all + goal;
        return flockVector;
    }
    private float CheckDistanceToPoint(Vector3 pos1, Vector3 pos2)
    {
        float distance = Vector3.Distance(pos1, pos2);
        return distance;
    }
    private List<Bird_Agent> GatherNeihbours(float range)
    {
        //For some reason if the group only increases by one, when the agnt leaves the group with only
        //the one agent it will remain inside its list of neighbours.
        //Drag one agent near to only 1 other and then drag away, the list wont empty.
        Collider[] colliderGroup;
        List<Bird_Agent> tempBirds = new List<Bird_Agent>();

        if (Physics.OverlapSphere(transform.position, range) != null)
        {
            colliderGroup = Physics.OverlapSphere(transform.position, range);
            foreach (var bird in colliderGroup)
            {
                if (bird.tag == "Bird_AI" && bird.transform != this.transform)
                {
                    tempBirds.Add(bird.gameObject.GetComponent<Bird_Agent>());
                }
            }
        }
        return tempBirds;
    }
    private void StartUp()
    {
        newPos = transform.position;
        newVeloc = new Vector3(Random.Range(-1, 1), 0, Random.Range(-1, 1));
        rigid = GetComponent<Rigidbody>();
    }
    //public void ApplyDamage()
    //{
       
    //        gameObject.SetActive(false);
    //        GameManager._gameManager.DestroyedObject();
            
        
    //}
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Projectile")
        {
            gameObject.SetActive(false);
        }
    }
    public void IsDead()
    {
        rigid.isKinematic = false;
        rigid.useGravity = true;
    }
}