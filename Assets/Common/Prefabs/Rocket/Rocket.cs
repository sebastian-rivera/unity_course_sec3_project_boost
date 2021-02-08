using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Rocket : MonoBehaviour {

	enum PlayerState {
		Alive,
		Dying,
		Transcending
	}

	Rigidbody _rigidBody;
	AudioSource _audioSource;
	[SerializeField] float rotationForce = 150f;
	[SerializeField] float thrustForce = 800f;
	[SerializeField] AudioClip mainEngineAudio;
	[SerializeField] AudioClip deathAudio;
	[SerializeField] AudioClip winAudio;	
	
	[SerializeField] ParticleSystem mainEngineParticle;
	[SerializeField] ParticleSystem deathParticle;
	[SerializeField] ParticleSystem winParticle;

	

	[SerializeField] float nextLevelDelay = 1f;
	PlayerState playerState = PlayerState.Alive;

	float deltaAccInterval = 0.05f;
	float deltaShutoffInterval = 0.01f;

	float thrustSpeed = 0f;
	Text debugText;
	float deltaSum;

	// Start is called before the first frame update
	void Start() {
		_rigidBody = GetComponent<Rigidbody>();
		_audioSource = GetComponent<AudioSource>();
		debugText = GameObject.FindGameObjectWithTag("DebugText").GetComponent<Text>();
		
	}

	private void OnCollisionEnter(Collision collision) {

		if (playerState != PlayerState.Alive) return;

		switch (collision.gameObject.tag) {

			case "Friendly":
				//doNothing				
				break;
			case "Finish":
				playerState = PlayerState.Transcending;
				_audioSource.Stop();
   				_audioSource.PlayOneShot(winAudio);
				Invoke(nameof(LoadNextScene), nextLevelDelay);				
				break;
			default:
				playerState = PlayerState.Dying;				
				_audioSource.Stop();
  				_audioSource.PlayOneShot(deathAudio);
				Invoke(nameof(LoadFirstScene), nextLevelDelay);
				break;
		}
	}

	// Update is called once per frame
	void Update() {

		if(playerState == PlayerState.Alive) {
			Thrust();
			Rotate();
		}

		
	}

	private void LoadFirstScene() {
		SceneManager.LoadScene(0);
	}

	private void LoadNextScene() {
		SceneManager.LoadScene(1);
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

	private void Thrust() {
		
		if (Input.GetKey(KeyCode.Space)) {			

			_rigidBody.AddRelativeForce(Time.deltaTime * (thrustForce) * Vector3.up);
			if (_audioSource.isPlaying == false) {
				_audioSource.PlayOneShot(mainEngineAudio);
			}
						
		} else {
			if (_audioSource.isPlaying) {
				_audioSource.Stop();
			}
			
		}

	}

}
