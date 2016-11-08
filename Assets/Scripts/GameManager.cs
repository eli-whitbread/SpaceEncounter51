using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager _gameManager;
    public int birdsDestroyed;

    void Awake()
    {
        _gameManager = this;

    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DestroyedObject()
    {
        birdsDestroyed++;
    }
}
