using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rocket : MonoBehaviour {
	Rigidbody _rigidBody;
	AudioSource _audioSource;
	[SerializeField]float maxThrustSpeed = 2000f;
	[SerializeField]float rotationForce = 150f;
	[SerializeField]float thrustAcceleration = 10f;
	[SerializeField]float fixedThrust = 800f;

	float deltaAccInterval = 0.05f;
	float deltaShutoffInterval = 0.01f;

	float thrustSpeed = 0f;
	Text debugText;
	float deltaSum;

	// Start is called before the first frame update
	void Start() {
		_rigidBody = GetComponent<Rigidbody>();
		//_rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
		_audioSource = GetComponent<AudioSource>();


		debugText = GameObject.FindGameObjectWithTag("DebugText").GetComponent<Text>();
		
	}

	private void OnCollisionEnter(Collision collision) {
		switch (collision.gameObject.tag) {

			case "Friendly":
				//doNothing
				print("You are safe!");
				break;			
			default:
				print("You are dead!");
				break;
		}
	}

	// Update is called once per frame
	void Update() {
		Thrust();
		Rotate();
	}

	private void Rotate() {

		if (Input.GetKey(KeyCode.A)) {
			transform.Rotate(Time.deltaTime * rotationForce * Vector3.forward);
			_rigidBody.angularVelocity = Vector3.zero;
		}

		if (Input.GetKey(KeyCode.D)) {
			transform.Rotate(Time.deltaTime * rotationForce * -Vector3.forward);
			_rigidBody.angularVelocity = Vector3.zero;
		}
		
	}

	private void Decelerate() {
		deltaSum += Time.deltaTime;

		if (thrustSpeed > 0 && deltaSum >= deltaAccInterval) {
			thrustSpeed -= thrustAcceleration * Mathf.Round(deltaSum / deltaAccInterval);
		}

		if (deltaSum >= deltaAccInterval) {
			deltaSum = 0;
		}

		debugText.text = $"Thrust Force:{(thrustSpeed)}\nTime.Delta:{Time.deltaTime}";
	}

	private void Accelerate() {

		deltaSum += Time.deltaTime;

		if (thrustSpeed < maxThrustSpeed && deltaSum >= deltaAccInterval) {
			thrustSpeed += thrustAcceleration * Mathf.Round(deltaSum / deltaAccInterval);
		}

		if (deltaSum >= deltaAccInterval) {
			deltaSum = 0;
		}
		debugText.text = $"Thrust Force:{(thrustSpeed)}\nTime.Delta:{Time.deltaTime}";


	}

	private void EngineShutoff() {

		deltaSum += Time.deltaTime;

		if (thrustSpeed > 0 && deltaSum >= deltaShutoffInterval) {
			thrustSpeed -= thrustAcceleration * Mathf.Round(deltaSum / deltaShutoffInterval);
		}

		if (deltaSum >= deltaShutoffInterval) {
			deltaSum = 0;
		}

		debugText.text = $"Thrust Force:{(thrustSpeed)}\nTime.Delta:{Time.deltaTime}";
	}

	private void Thrust() {
		
		if (Input.GetKey(KeyCode.Space)) {
			
			if (Input.GetKey(KeyCode.Q)) {
				Decelerate();
			}

			if (Input.GetKey(KeyCode.E)) {
				Accelerate();
			}

			debugText.text = $"Thrust Force:{(thrustSpeed)}\nTime.Delta:{Time.deltaTime}";

			if(fixedThrust > 0) {
				_rigidBody.AddRelativeForce(Time.deltaTime * (fixedThrust) * Vector3.up);
				if (_audioSource.isPlaying == false) {
					_audioSource.Play();
				}
			} else {
				_rigidBody.AddRelativeForce(Time.deltaTime * (thrustSpeed) * Vector3.up);

				if (_audioSource.isPlaying == false && thrustSpeed > 0) {
					_audioSource.Play();
				}

				if (_audioSource.isPlaying && thrustSpeed == 0) {
					_audioSource.Stop();
				}
			}
			


		} else {
			if (_audioSource.isPlaying) {
				_audioSource.Stop();
			}
			EngineShutoff();
			
		}


	}
}
