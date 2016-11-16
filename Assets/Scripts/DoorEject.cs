using UnityEngine;
using System.Collections;

public class DoorEject : MonoBehaviour {



    public GameObject[] ejectors;
    public bool[] ejectIndex;
    public Transform explosionPos;
    private void Start()
    {
        ejectIndex = new bool[ejectors.Length];
    }
        
    private void EjectDoor()
    {
        foreach (GameObject ejector in ejectors)
        {
            ejector.SetActive(false);
        }
        Rigidbody doorRigidBody = GetComponent<Rigidbody>();
        doorRigidBody.isKinematic = false;

        doorRigidBody.AddExplosionForce(Random.Range(4.5f, 40.5f),explosionPos.position, Random.Range(0.9f, 2.0f), Random.Range(0.3f, 0.9f), ForceMode.Impulse);
        VR_CharacterController._charController.lockControls = false;
        VR_CharacterController._charController.teleportIsOn = true;
        GameManager._gameManager.startTimer = true;
    }
    public void EjectorOn(GameObject obj)
    {
        for (int i = 0; i < ejectors.Length; i++)
        {
            if (ejectors[i] == obj)
            {
                ejectIndex[i] = true;
                obj.GetComponent<Renderer>().material.color = Color.green;
                
            }
        }
        if (ejectIndex[0] && ejectIndex[1] && ejectIndex[2] && ejectIndex[3])
        {
            EjectDoor();
        }

    }
}
