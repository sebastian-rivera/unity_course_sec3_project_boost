using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {
	Rigidbody _rigidBody;
	AudioSource _audioSource;
	[SerializeField]float thrustSpeed = 1000f;
	[SerializeField]float rotationSpeed = 100f;

	float acceleration = 0f;

	// Start is called before the first frame update
	void Start() {
		_rigidBody = GetComponent<Rigidbody>();
		//_rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
		_audioSource = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update() {
		Thrust();
		Rotate();

	}

	private void Rotate() {

		if (Input.GetKey(KeyCode.A)) {
			transform.Rotate(Time.deltaTime * rotationSpeed * Vector3.forward);
			_rigidBody.angularVelocity = Vector3.zero;
		}

		if (Input.GetKey(KeyCode.D)) {
			transform.Rotate(Time.deltaTime * rotationSpeed * -Vector3.forward);
			_rigidBody.angularVelocity = Vector3.zero;
		}

		
	}

	private void Thrust() {

		if (Input.GetKey(KeyCode.Space)) {

			if (Input.GetKey(KeyCode.Q)) {
				if (acceleration > -1000) {
					acceleration -= (1 * Time.deltaTime);
				}
			}

			if (Input.GetKey(KeyCode.E)) {
				if (acceleration < 1000) {
					acceleration += (1 * Time.deltaTime);
				}
			}

			_rigidBody.AddRelativeForce(Time.deltaTime * (thrustSpeed + acceleration) * Vector3.up);

			if (_audioSource.isPlaying == false) {
				_audioSource.Play();
			}

		} else {
			if (_audioSource.isPlaying) {
				_audioSource.Stop();
			}
		}
	}
}
