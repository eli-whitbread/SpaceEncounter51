using UnityEngine;
using System.Collections;

public class ViveANchorMover : MonoBehaviour {

    public Transform player;

	
	// Update is called once per frame
	void Update () {

        transform.position = player.transform.position;
	}
}
