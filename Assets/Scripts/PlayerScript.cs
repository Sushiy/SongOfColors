﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

	public float jumpPower = 100f;

	public float horizontalSpeed = 5f;

	Rigidbody2D body;

	bool isOnGround = false;

	public void OnGround(){
		isOnGround = true;
	}

	public void OffGround(){
		isOnGround = false;
	}

	// Use this for initialization
	void Start () {
		body = transform.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {}

	void FixedUpdate() {
		if (isOnGround && Input.GetKey(KeyCode.Space)) {
			body.AddForce(Vector2.up * jumpPower);
		}
		if (Input.GetKey (KeyCode.A)) {
			body.transform.position += Vector3.left * (horizontalSpeed * Time.deltaTime);
		}
		else if (Input.GetKey(KeyCode.D)) {
			body.transform.position += Vector3.right * (horizontalSpeed * Time.deltaTime);
		}
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.collider.tag == "Enemy") {
			GameOver();
		} else if (other.collider.tag == "Point") {
			//increase Points
			//despawn Point
		}
	}
		
	void GameOver() {
		Debug.Log("TOT!");
	}
}
