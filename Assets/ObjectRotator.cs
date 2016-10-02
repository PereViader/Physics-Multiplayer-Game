using UnityEngine;
using System.Collections;

public class ObjectRotator : MonoBehaviour {

    [SerializeField]
    float speed;

	// Update is called once per frame
	void Update () {
        float horizontal = -Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        transform.Rotate( new Vector3(vertical*speed*Time.deltaTime, horizontal * speed * Time.deltaTime),Space.World);
	}
}
