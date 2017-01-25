using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundColliderScript : MonoBehaviour {

	PlayerScript player;

	// Use this for initialization
	void Start () {
		player = transform.GetComponentInParent<PlayerScript>();
	}
	
	// Update is called once per frame
	void Update () {}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.collider.tag == "ground")
            player.OnGround();
	}

	void OnCollisionStay2D(Collision2D other) {
		if (other.collider.tag == "ground")
			player.OnGround();
	}

	void OnCollisionExit2D(Collision2D other) {
		if (other.collider.tag == "ground")
			player.OffGround();
	}
}
