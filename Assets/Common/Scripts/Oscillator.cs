using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour {

	[SerializeField] 
	Vector3 _movementVecor;

	[SerializeField]
	float period = 2f;

	[Range(0,1)]
	[SerializeField]
	float _movementFactor;

	Vector3 _startingPos;


	// Start is called before the first frame update
	void Start() {
		_startingPos = transform.position;

	}

	// Update is called once per frame
	void Update() {

		if (period <= Mathf.Epsilon) return;

		float cycles = Time.time / period;
		float rawSinWave = Mathf.Sin(cycles * Mathf.PI * 2);
		_movementFactor = rawSinWave / 2f + 0.5f;

		Vector3 offset = _movementFactor * _movementVecor;
		transform.position = _startingPos + offset;

	}
}
