using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	private Transform _following;

	[SerializeField] 
		private float speed;
	[SerializeField]
	private float rotationHorizontalStep;
	[SerializeField]
	private float rotationVerticalStep;
	[SerializeField]
	private float minimumVerticalAngle;
	[SerializeField]
	private float maximumVerticalAngle;
	[SerializeField]
	private Vector2 initialAngleXY;
	[SerializeField]
	private Vector3 positionDelta;
	[SerializeField]
	private float cameraSpeed;

	private float rotationHorizontal = 0;
	private float rotationVertical = 0;

	private Vector3 currentPositionDelta;



	private bool hasObjectRequestedMove;
	private bool hasProvidedMoveRequest = true;
	private bool isCameraAnchored;

	void Awake() {
		currentPositionDelta = positionDelta;
	}

	public void SetFollowingObject (GameObject following) {
		_following = following.transform;
		rotationVertical = initialAngleXY.x;
		rotationHorizontal = initialAngleXY.y;
		currentPositionDelta = Quaternion.Euler(rotationVertical, rotationHorizontal, 0) * positionDelta;
		transform.position = _following.position+currentPositionDelta;
		transform.LookAt(_following.position);

	}

	public void ObjectHasMoved() {
		hasObjectRequestedMove = true;
	}

	void FixedUpdate () {
		if (_following == null)
			return;


		float horizontal = -Input.GetAxis ("CameraHorizontal");
		float vertical = -Input.GetAxis ("CameraVertical");


		bool hasChanged = false;

		if (!Mathf.Approximately (horizontal, 0.0f) ) {
			rotationHorizontal = (rotationHorizontal + horizontal * rotationHorizontalStep * Time.deltaTime) % 360.0f;
			hasChanged = true;
		}

		if ( !Mathf.Approximately (vertical, 0.0f) ) {
			float calculatedRotation = (rotationVertical + vertical * rotationVerticalStep * Time.deltaTime);

			if (calculatedRotation < minimumVerticalAngle)
				rotationVertical = minimumVerticalAngle;
			else if (calculatedRotation > maximumVerticalAngle)
				rotationVertical = maximumVerticalAngle;
			else
				rotationVertical = calculatedRotation;
			
			hasChanged = true;
		}

		if ( hasChanged )
		{
			currentPositionDelta = Quaternion.Euler(rotationVertical, rotationHorizontal, 0) * positionDelta;
		}


		bool cameraIsInItsPosition = Vector3.Distance (transform.position, _following.position+positionDelta) < positionDelta.magnitude + 0.1f;
		if (hasObjectRequestedMove) {
			if (!hasProvidedMoveRequest && !cameraIsInItsPosition) { // fins que no sigui fora de la seva posicio no haura proporcionat el moviment
				hasProvidedMoveRequest = true;
			} else if (hasProvidedMoveRequest && cameraIsInItsPosition) { // un cop ha proporcionat el moviment fins que no torni a estar al seu lloc no acaba la peticio de moviment
				hasObjectRequestedMove = false;
			}
		}

		transform.position = Vector3.Lerp( transform.position, _following.position + currentPositionDelta , 0.25f);

		Debug.DrawRay (_following.position, currentPositionDelta);
		transform.LookAt(_following.position);
	}
}