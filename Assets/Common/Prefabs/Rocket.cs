using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {
	Rigidbody _rigidBody;
	AudioSource _audioSource;
	float thrustSpeed = 1000;
	float rotationSpeed = 100;
	// Start is called before the first frame update
	void Start() {
		_rigidBody = GetComponent<Rigidbody>();
		_audioSource = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update() {
		if (Input.GetKey(KeyCode.Space)) {
			_rigidBody.AddRelativeForce(Time.deltaTime * thrustSpeed * Vector3.up);

			if(_audioSource.isPlaying == false) {
				_audioSource.Play();
			}

		} else {
			if(_audioSource.isPlaying) {
				_audioSource.Stop();
			}
		}

		if(Input.GetKey(KeyCode.A)) {
			transform.Rotate(Time.deltaTime * rotationSpeed * Vector3.forward);
			_rigidBody.angularVelocity = Vector3.zero;
		}

		if(Input.GetKey(KeyCode.D)) {
			transform.Rotate(Time.deltaTime * rotationSpeed * -Vector3.forward);
			_rigidBody.angularVelocity = Vector3.zero;
		}

		transform.rotation = new Quaternion(0, 0, transform.rotation.z, transform.rotation.w);
		transform.position = new Vector3(transform.position.x, transform.position.y, 0);

	}
}
