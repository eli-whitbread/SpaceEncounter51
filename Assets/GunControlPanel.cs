using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class GunControlPanel : MonoBehaviour {

    public GameObject panel;
    public Text panelText;


	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        panelText.text = GameManager._gameManager.birdsDestroyed.ToString() + "/10";
        
	}
}
