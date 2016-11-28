using UnityEngine;
using System.Collections;

public class TurnPodMeshColliderOnOff : MonoBehaviour {

    public static TurnPodMeshColliderOnOff _instance;

   

    void Awake()
    {
        _instance = this;
    }

	public void SwitchCollider(bool on)
    {
        if(on)
        {
            GetComponent<MeshCollider>().enabled = true;
        }
        else
        {
            GetComponent<MeshCollider>().enabled = false;

        }
    }
}
