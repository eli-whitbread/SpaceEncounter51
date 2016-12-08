using UnityEngine;
using System.Collections;

public class DoorEject : MonoBehaviour {
    
    public GameObject[] ejectors;
    public GameObject[] particles;
    public GameObject shipAIHead;
    private AudioSource ejectorAudioSouce;
    public AudioClip ejectorButtonSound;

    public bool[] ejectIndex;
    public Transform explosionPos;
    private void Start()
    {
        ejectIndex = new bool[ejectors.Length];
        ejectorAudioSouce = GetComponent<AudioSource>();
        ejectorAudioSouce.clip = ejectorButtonSound;
    }
        
    private void EjectDoor()
    {
        foreach (GameObject ejector in ejectors)
        {
            ejector.SetActive(false);

        }
        foreach (GameObject particle in particles)
        {
            particle.SetActive(false);

        }
        Rigidbody doorRigidBody = GetComponent<Rigidbody>();
        doorRigidBody.isKinematic = false;

        doorRigidBody.AddExplosionForce(Random.Range(4.5f, 30.0f),explosionPos.position, Random.Range(0.9f, 2.0f), Random.Range(0.3f, 0.9f), ForceMode.Impulse);
        VR_CharacterController._charController.lockControls = false;
        VR_CharacterController._charController.teleportIsOn = true;
        shipAIHead.transform.parent = GameObject.Find("CameraAnchor").transform;
        shipAIHead.transform.localPosition = new Vector3(1.385f, -0.517f, 0.81f);
        shipAIHead.transform.eulerAngles = new Vector3(17.9f, 20f, 3.6f);
        //shipAIHead.SetActive(false);
        GameManager._gameManager.startTimer = true;
        VR_CharacterController._charController.playerAudioSource.volume = 0.06f;
        TurnPodMeshColliderOnOff._instance.SwitchCollider(false);
    }
    public void EjectorOn(GameObject obj)
    {
        for (int i = 0; i < ejectors.Length; i++)
        {
            if (ejectors[i] == obj)
            {
                ejectIndex[i] = true;
                obj.GetComponent<Renderer>().material.color = Color.green;
                ejectorAudioSouce.PlayOneShot(ejectorButtonSound);
                particles[i].SetActive(true);
            }
        }
        if (ejectIndex[0] && ejectIndex[1] && ejectIndex[2] && ejectIndex[3])
        {
            EjectDoor();
        }

    }
}
