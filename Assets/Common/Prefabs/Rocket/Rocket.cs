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
	[SerializeField] float _rotationForce = 150f;
	[SerializeField] float _thrustForce = 800f;
	[SerializeField] AudioClip _mainEngineAudio;
	[SerializeField] AudioClip _deathAudio;
	[SerializeField] AudioClip _winAudio;	
	
	[SerializeField] ParticleSystem _mainEngineParticle;
	[SerializeField] ParticleSystem _deathParticle;
	[SerializeField] ParticleSystem _winParticle;
	
	[SerializeField] float _nextLevelDelay = 2f;

	PlayerState _playerState = PlayerState.Alive;

	[SerializeField] Light _leftLight;
	[SerializeField] Light _rightLight;
	[SerializeField] bool _lightsOn;
	
	Text _debugText;


	// Start is called before the first frame update
	void Start() {
		_rigidBody = GetComponent<Rigidbody>();
		_audioSource = GetComponent<AudioSource>();
		_debugText = GameObject.FindGameObjectWithTag("DebugText").GetComponent<Text>();								
	}

	private void OnCollisionEnter(Collision collision) {

		if (_playerState != PlayerState.Alive) return;

		switch (collision.gameObject.tag) {

			case "Friendly":
				//doNothing				
				break;
			case "Finish":
				_playerState = PlayerState.Transcending;
				_audioSource.Stop();
   				_audioSource.PlayOneShot(_winAudio);
				_winParticle.Play();
				Invoke(nameof(LoadNextScene), _nextLevelDelay);				
				break;
			default:
				_playerState = PlayerState.Dying;
				_deathParticle.Play();
				_audioSource.Stop();
  				_audioSource.PlayOneShot(_deathAudio);
				Invoke(nameof(LoadFirstScene), _nextLevelDelay);
				break;
		}
	}

	// Update is called once per frame
	void Update() {

		if(_playerState == PlayerState.Alive) {
			Thrust();
			Rotate();
		}

		if(_lightsOn && _rightLight.enabled == false && _leftLight.enabled == false) {
			_rightLight.enabled = _lightsOn;
			_leftLight.enabled = _lightsOn;
		}

		if(_lightsOn == false && _rightLight.enabled && _leftLight.enabled) {
			_rightLight.enabled = _lightsOn;
			_leftLight.enabled = _lightsOn;
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
			transform.Rotate(Time.deltaTime * _rotationForce * Vector3.forward);
			_rigidBody.angularVelocity = Vector3.zero;
		}

		if (Input.GetKey(KeyCode.D)) {
			transform.Rotate(Time.deltaTime * _rotationForce * -Vector3.forward);
			_rigidBody.angularVelocity = Vector3.zero;
		}
		
	}

	private void Thrust() {
		
		if (Input.GetKey(KeyCode.Space)) {			
			_rigidBody.AddRelativeForce(Time.deltaTime * (_thrustForce) * Vector3.up);
 			if (_audioSource.isPlaying == false) {
				_audioSource.PlayOneShot(_mainEngineAudio);
			}

			if(_mainEngineParticle.isEmitting == false) {
				_mainEngineParticle.Play();
			}
						
		} else {
			if (_audioSource.isPlaying) {
				_audioSource.Stop();
			}

			if(_mainEngineParticle.isEmitting) {
				_mainEngineParticle.Stop();
			}

		}

	}

}
