using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Menu : MonoBehaviour
{

    public Canvas MainCavas;
    public Canvas OptionsCanvas;

    void Awake()
    {
        OptionsCanvas.enabled = false;
    }

    public void OptionsOn()
    {
        OptionsCanvas.enabled = true;
        MainCavas.enabled = false;
    }

    public void ReturnOn()
    {
        OptionsCanvas.enabled = false;
        MainCavas.enabled = true;
    }
	
    public void LoadOn()
    {
        //Application.LoadLevel (1);
    }
}
