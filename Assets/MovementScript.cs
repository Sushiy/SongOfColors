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
	void Update () {
		if (!movingBack && time < lerpTime) {
			time += Time.deltaTime;
			time = Mathf.Clamp (time, 0f, lerpTime);
			transform.position = Vector3.Lerp (startPos, startPos + moveVec, time / lerpTime);
		} else if (movingBack && time > 0f) {
			time -= Time.deltaTime;
			time = Mathf.Clamp (time, 0f, lerpTime);
			transform.position = Vector3.Lerp (startPos, startPos + moveVec, time / lerpTime);
		}
	}

	public void MoveUp()
	{
		movingBack = false;
	}

	public void MoveDown()
	{
		movingBack = true;
	}
}
