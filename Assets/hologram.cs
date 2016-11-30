using UnityEngine;
using System.Collections;

public class hologram : MonoBehaviour {

    public GameObject screen;
    public GameObject holograph;
    public Transform newpos;
    // Use this for initialization
    void Start()
    {
        ScreenActivated();

    }

    // Update is called once per frame
    void Update()
    {
    }
    private void SpinHolograph()
    {
        
    }
    public void ScreenActivated()
    {
        holograph.transform.localScale += new Vector3(85, 85, 85);
        screen.transform.localScale += new Vector3(1.5f, 1.5f, 1.5f);
        screen.transform.position = newpos.position;
    }

}
