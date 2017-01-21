using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundColliderScript : MonoBehaviour {

	PlayerScript father;

	// Use this for initialization
	void Start () {
		father = transform.GetComponentInParent<PlayerScript>();
	}
	
	// Update is called once per frame
	void Update () {}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.collider.tag == "ground")
        {
            Debug.Log("ground");
			father.OnGround();
		}
	}

	void OnCollisionStay2D(Collision2D other) {
		if (other.collider.tag == "ground") {
			father.OnGround();
		}
	}

	void OnCollisionExit2D(Collision2D other) {
		if (other.collider.tag == "ground") {
			father.OffGround();
		}
	}
}
