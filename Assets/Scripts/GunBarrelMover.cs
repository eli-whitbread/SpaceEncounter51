using UnityEngine;
using System.Collections;

public class GunBarrelMover : MonoBehaviour {

    public Transform topBarrelsAnchor, bottomBarrelsAnchor;
    public GunShootScript gunShootScript;
    public float cycleTime;

    bool activated, topRecoiling, bottomRecoiling;
    
    public bool Activated
    {
        get { return activated; }
        set { activated = value; }
    }

    public float CycleTime
    {
        get { return cycleTime; }
        set { cycleTime = value; }
    }

    void Start()
    {
        activated = false;
        topRecoiling = true;
        bottomRecoiling = false;
    }
   
	// Update is called once per frame
	void Update () {
	
        if(activated)
        {
            Vector3 tPos = topBarrelsAnchor.transform.localPosition;
            Vector3 bPos = bottomBarrelsAnchor.transform.localPosition;

            //top barrels
            if (topRecoiling)
            {
                if(tPos.z >= -0.17f)
                {
                    tPos.z -= cycleTime * Time.deltaTime;
                }
                else
                {
                    topRecoiling = false;
                }
            }
            if(topRecoiling == false)
            {
                if(tPos.z <= 0)
                {
                    tPos.z += (cycleTime / 2) * Time.deltaTime;
                }
                else
                {
                    gunShootScript.CycleProjectileSpawnPoint(true);
                    topRecoiling = true;
                }
            }

            //bottom barrels
            if (bottomRecoiling && topRecoiling == false)
            {
                if (bPos.z >= -0.17f)
                {
                    bPos.z -= cycleTime * Time.deltaTime;
                }
                else
                {
                    bottomRecoiling = false;
                }
            }
            if (bottomRecoiling == false)
            {
                if (bPos.z <= 0)
                {
                    bPos.z += (cycleTime / 2) * Time.deltaTime;
                }
                else
                {
                    gunShootScript.CycleProjectileSpawnPoint(false);
                    bottomRecoiling = true;
                }
            }

            topBarrelsAnchor.transform.localPosition = tPos;
            bottomBarrelsAnchor.transform.localPosition = bPos;
        }
	}
}
