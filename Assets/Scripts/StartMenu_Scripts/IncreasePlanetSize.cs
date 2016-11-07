using UnityEngine;
using System.Collections;

public class IncreasePlanetSize : MonoBehaviour {

    [SerializeField]
    private GameObject menuAsteroids;   //Set this within the inspector
    [SerializeField]
    private GameObject approachParticleEffect;  //Set this in inspector
    public Vector3 newAsteroidPOS = new Vector3(-1.15f, -1.347597f, 12.37f);
    public float asteroidMoveSpeed;
    private bool startIncrease = false;

    public float startScale = 2f;
    public float endScale = 3.0f;
    public float scaleSpeed = 0.1f;

	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        if (startIncrease)
        {
            startScale += scaleSpeed * Time.deltaTime;

            if(startScale > endScale)
            {
                startScale = endScale;
            }

            transform.localScale = new Vector3(startScale, startScale, startScale);

            StartCoroutine("MoveObject");
            
        }

    }


    public void increasePlanet()
    {
        startIncrease = true;
        approachParticleEffect.SetActive(true);


    }

    private IEnumerator MoveObject()
    {        
        while(true)
        {
            menuAsteroids.transform.position = Vector3.Lerp(menuAsteroids.transform.position, newAsteroidPOS, Time.deltaTime * asteroidMoveSpeed);

            //If object is in new position, stop script
            if(menuAsteroids.transform.position == newAsteroidPOS)
            {
                yield break;
            }

            // Otherwise, continue next frame
            yield return null;
        }

    }
}
