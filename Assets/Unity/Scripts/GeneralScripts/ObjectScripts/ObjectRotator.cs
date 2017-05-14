using UnityEngine;
using System.Collections;

public class ObjectRotator : MonoBehaviour {

    [SerializeField]
    float speed;

    [SerializeField]
    float delayBetweenChange;

    private float nextChange;
    private Vector3 rotation;


    void Update()
    {
        if (Time.time >= nextChange)
        {
            nextChange = Time.time + delayBetweenChange;
            rotation = Random.onUnitSphere;
        }
        transform.Rotate(rotation*speed*Time.deltaTime);
    }
}
