using UnityEngine;
using System.Collections;

public class ShackTrigger : MonoBehaviour {


    BoxCollider col;
    public GameObject player;

    bool playerInShack = false;

    void Start()
    {
        col = gameObject.GetComponent<BoxCollider>();
    }
    
	// Update is called once per frame
	void FixedUpdate () {

        if (playerInShack == false)
        {
            if (col.bounds.Contains(player.transform.position))
            {
                playerInShack = true;
                VR_CharacterController._charController.PlayerIsInShack = true;
                DialogueManager._instance.PlayerInShack = true;
                ParticleSystemSwapShack._instance.SwitchParticles(false);
            }
        }
        else
        {
            if (!col.bounds.Contains(player.transform.position))
            {
                playerInShack = false;
                VR_CharacterController._charController.PlayerIsInShack = false;
                DialogueManager._instance.PlayerInShack = false;
                ParticleSystemSwapShack._instance.SwitchParticles(true);
            }
        }
	}
}
