using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraFollowSmooth : MonoBehaviour {

    // test zone ...
    private bool isInputEnabled = true;
    //----------------

    // Variables
    [SerializeField] private float _cameraLerpFactor;
	[SerializeField] private Transform _following;

	//----------------- Distances
	 private float _distanceFromObject;
	[SerializeField] private float _maxDistanceFromObject;


	// ---------------- Vectors
	private Vector3 _basicVector;
	private Vector3 _positionFromObject;
	private Vector3 _positionFromObjectMax;

	// ----------------- Rotations
	[SerializeField] private float _invertVertical = -1;
	[SerializeField] private float _invertHorizontal = -1;

	[SerializeField] private float _rotationHorizontalStep;
	[SerializeField] private float _rotationVerticalStep;

	[SerializeField] private float _minimumVerticalAngle;
	[SerializeField] private float _maximumVerticalAngle;

	[SerializeField] private float _rotationHorizontal;
	[SerializeField] private float _rotationVertical;

	[SerializeField] private float _smooth = 1.5f;
		// -------------------Angles



	void Awake () {
		_basicVector = Vector3.back;
	}
	
	void FixedUpdate () {
		if (!_following || !isInputEnabled)
			return;

		float horizontal = _invertHorizontal * Input.GetAxis ("CameraHorizontal");
		float vertical = _invertVertical * Input.GetAxis ("CameraVertical");

		bool hasChanged = false;


		if (!Mathf.Approximately (horizontal, 0.0f) ) {
			_rotationHorizontal = (_rotationHorizontal + horizontal * _rotationHorizontalStep * Time.fixedDeltaTime) % 360.0f;
			hasChanged = true;
		}

		if ( !Mathf.Approximately (vertical, 0.0f) ) {
			float calculatedRotation = (_rotationVertical + vertical * _rotationVerticalStep * Time.fixedDeltaTime);
			
			if (calculatedRotation < _minimumVerticalAngle)
				_rotationVertical = _minimumVerticalAngle;
			else if (calculatedRotation > _maximumVerticalAngle)
				_rotationVertical = _maximumVerticalAngle;
			else
				_rotationVertical = calculatedRotation;
			
			hasChanged = true;
		}

		if ( hasChanged )
		{
			updatePositionFromObjectMax();
		}

		// Si no es veu el jugador des de _positionFromObjectMax calcular un nou _positionFromObject
		// o sigui si hi ha una paret al cami apropar el vector per veure l'objecte

		transform.position = Vector3.Lerp( transform.position, _following.position + _positionFromObject , _cameraLerpFactor);
        Vector3 toLook = _following.position;
        toLook.y += 2;

        transform.LookAt (toLook);
		//SmoothLookAt ();

	}

	void updatePositionFromObjectMax() {
		_positionFromObjectMax = Quaternion.Euler(_rotationVertical, _rotationHorizontal, 0) * (_basicVector * _maxDistanceFromObject);
		_positionFromObject = _positionFromObjectMax;
	}

	void SmoothLookAt ()
	{
        // Create a vector from the camera towards the player.
        Vector3 relPlayerPosition = _following.position - transform.position;
		
		// Create a rotation based on the relative position of the player being the forward vector.
		Quaternion lookAtRotation = Quaternion.LookRotation(relPlayerPosition, Vector3.up);
		
		// Lerp the camera's rotation between it's current rotation and the rotation that looks at the player.
		transform.rotation = Quaternion.Lerp(transform.rotation, lookAtRotation, _smooth * Time.fixedDeltaTime);
	}

	public void SetFollowingObject (GameObject following, float horizontal, float vertical) {
        SetCameraStartingRotation(horizontal, vertical);
		_following = following.transform;
        updatePositionFromObjectMax ();
		transform.position = _following.position + _positionFromObjectMax;
		SmoothLookAt ();
	}

    public void SetFollowingObject(GameObject following)
    {
        Vector3 angles = following.transform.rotation.eulerAngles;
        SetCameraStartingRotation(angles.y, 30);

        _following = following.transform;
        updatePositionFromObjectMax();
        transform.position = _following.position + _positionFromObjectMax;
        SmoothLookAt();
    }

	private void SetCameraStartingRotation(float horizontal, float vertical) {
		_rotationVertical = vertical;
		_rotationHorizontal = horizontal;
	}

    public void SetInput(bool isActive)
    {
        isInputEnabled = isActive;
    }
}
