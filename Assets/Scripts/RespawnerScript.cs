using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnerScript : MonoBehaviour {

	public Vector3 spawnPosition;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Respawn() {
		transform.position = spawnPosition;
	}
}
