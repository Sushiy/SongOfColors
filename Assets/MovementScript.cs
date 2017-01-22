using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour {

	public Vector3 moveVec;

	Vector3 startPos;

	public float lerpTime;

	float time;

	bool movingBack;

	// Use this for initialization
	void Start () {
		startPos = transform.position;
		time = 0;
		movingBack = false;
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (!movingBack) {
			time += Time.deltaTime;
			time = Mathf.Clamp (time, 0f, lerpTime);
			transform.position = Vector3.Lerp (startPos, startPos + moveVec, time / lerpTime);
			if (time == lerpTime)
				movingBack = true;
		} else {
			time -= Time.deltaTime;
			time = Mathf.Clamp (time, 0f, lerpTime);
			transform.position = Vector3.Lerp (startPos, startPos + moveVec, time / lerpTime);
			if (time == 0f)
				movingBack = false;
		}
	}
}