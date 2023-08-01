using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidMovement : MonoBehaviour
{
    [Header("Control the speed of the Asteroid")]
    public float maxSpeed;
    public float minSpeed;

    [Header("Control the rotational speed")]
    public float rotationalSpeedMin;
    public float rotationalSpeedMax;

    private float rotationalSpeed;
    private float xAngle, yAngle, zAngle;

    public Vector3 movementDirection;

    private float asteroidSpeed;

    private GameObject lowPolyAsteroid;
    private Material asteroidMaterial;

    private float colorRed;
    private float colorGreen;

    // Start is called before the first frame update
    void Start()
    {
        //get a random speed
        asteroidSpeed = Random.Range(minSpeed, maxSpeed);

        //get a random rotation
        xAngle = Random.Range(0, 360);
        yAngle = Random.Range(0, 360);
        zAngle = Random.Range(0, 360);

        transform.Rotate(xAngle, yAngle, zAngle);

        rotationalSpeed = Random.Range(rotationalSpeedMin, rotationalSpeedMax);

        lowPolyAsteroid = transform.GetChild(0).gameObject;
        asteroidMaterial = lowPolyAsteroid.GetComponent<Renderer>().material;
        colorGreen = Random.Range(0.00332858f, 0.007689557f);
        if (asteroidSpeed >= 8)
        {
            colorRed = Random.Range(0.09f, 0.14f);
            asteroidMaterial.SetColor("_EmissionColor", new Color(colorRed, colorGreen, colorGreen));
        }
        if (asteroidSpeed >= 5 && asteroidSpeed < 8)
        {
            colorRed = Random.Range(0.04f, 0.08f);
            asteroidMaterial.SetColor("_EmissionColor", new Color(colorRed, colorGreen, colorGreen));
        }
        else
        {
            colorRed = Random.Range(0.01f, 0.03f);
            asteroidMaterial.SetColor("_EmissionColor", new Color(colorRed, colorGreen, colorGreen));
        }
    }

    // Update is called once per frame
    void Update()
    {

        transform.Translate(movementDirection * Time.deltaTime * asteroidSpeed, Space.World);
        transform.Rotate(Vector3.up * Time.deltaTime * rotationalSpeed);

    }
}
