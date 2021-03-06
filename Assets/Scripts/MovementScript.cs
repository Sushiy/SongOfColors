﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour {

	public Vector3 moveVec;

	Vector3 startPos;

	public float lerpTime;

    Rigidbody2D rigid2d;

	float time;

	bool movingBack;

	// Use this for initialization
	void Start () {
        rigid2d = GetComponent<Rigidbody2D>();
		startPos = transform.position;
		time = 0;
		movingBack = false;
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (!movingBack) {
			time += Time.deltaTime;
			time = Mathf.Clamp (time, 0f, lerpTime);
			rigid2d.MovePosition(Vector3.Lerp (startPos, startPos + moveVec, time / lerpTime));
			if (time == lerpTime)
				movingBack = true;
		} else {
			time -= Time.deltaTime;
			time = Mathf.Clamp (time, 0f, lerpTime);
            rigid2d.MovePosition(Vector3.Lerp(startPos, startPos + moveVec, time / lerpTime));
            if (time == 0f)
				movingBack = false;
		}
	}

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position + new Vector3(0,0.5f, 0), transform.position + moveVec + new Vector3(0, 0.5f, 0));
    }
}