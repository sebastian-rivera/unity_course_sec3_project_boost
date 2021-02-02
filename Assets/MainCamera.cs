using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour {

	GameObject _player;

	// Start is called before the first frame update
	void Start() {
		_player = GameObject.FindWithTag("Player");
	}

	// Update is called once per frame
	void Update() {
		transform.position = new Vector3(_player.transform.position.x, _player.transform.position.y, -18);
	}
}
