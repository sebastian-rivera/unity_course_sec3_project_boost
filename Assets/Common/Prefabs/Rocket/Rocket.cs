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
	bool _lightsOn = true;
	
	Text _debugText;
	bool _debugMode;


	// Start is called before the first frame update
	void Start() {
		_rigidBody = GetComponent<Rigidbody>();
		_audioSource = GetComponent<AudioSource>();
		_debugText = GameObject.FindGameObjectWithTag("DebugText").GetComponent<Text>();	
		_debugText.text = $"Debug Mode: {_debugMode}";
	}

	private void OnCollisionEnter(Collision collision) {

		if (_playerState != PlayerState.Alive || _debugMode) return;

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
				Invoke(nameof(LoadCurrentScene), _nextLevelDelay);
				break;
		}
	}

	// Update is called once per frame
	void Update() {

		if(_playerState == PlayerState.Alive) {
			Thrust();
			Rotate();
			ToggleLight();
			ToggleDebugMode();
			NextLevel();
		}

		
	}

	private void ToggleLight() {
		if(Input.GetKeyUp(KeyCode.F)) {
			_lightsOn = !_lightsOn;
			_rightLight.enabled = _lightsOn;
			_leftLight.enabled = _lightsOn;
		}		
	}

	private void ToggleDebugMode() {
		if(Input.GetKeyUp(KeyCode.X)) {
			_debugMode = !_debugMode;

			_debugText.text = $"Debug Mode: {_debugMode}";

		}
	}

	private void NextLevel() {
		if(Input.GetKeyUp(KeyCode.L)) {
			LoadNextScene();
		}
	}

	private void LoadCurrentScene() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	private void LoadNextScene() {
		if(SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings -1) {
			SceneManager.LoadScene(0);
		} else {
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		}
		
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
