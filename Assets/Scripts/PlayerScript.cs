using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerScript : MonoBehaviour {

	public float jumpPower = 100f;

	public float horizontalSpeed = 5f;

	Rigidbody2D body;

	bool isOnGround = false;

	ColorModelScript colorModel;

	SpriteRenderer renderer;

	//current Hue Value (HSV color)
	float currentHue;

	//Hue Value to interpolate to
	float destHue;

	float lerpTime;

	public float lerpDuration = 2.5f;

	public void OnGround(){
		isOnGround = true;
	}

	public void OffGround(){
		isOnGround = false;
	}

	// Use this for initialization
	void Start () {
		colorModel = ColorModelScript.instance;
		body = transform.GetComponent<Rigidbody2D>();
		renderer = GetComponentInParent<SpriteRenderer>();
		currentHue = 0f;

		colorModel.activeColor
			.Subscribe(x => ChangeColor(x))
			.AddTo(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		if (lerpTime > 0f) {
			lerpTime -= Time.deltaTime;
			Mathf.Clamp (lerpTime, 0, lerpDuration);
		}
		currentHue = Mathf.Lerp(currentHue, destHue, 1f - lerpTime / lerpDuration);
		renderer.color = Color.HSVToRGB (currentHue, 1f, 1f);
	}

	void FixedUpdate() {
		if (isOnGround && Input.GetKey(KeyCode.Space)) {
			body.AddForce(Vector2.up * jumpPower);
		}
		if (Input.GetKey(KeyCode.A)) {
			body.transform.position += Vector3.left * (horizontalSpeed * Time.deltaTime);
		}
		else if (Input.GetKey(KeyCode.D)) {
			body.transform.position += Vector3.right * (horizontalSpeed * Time.deltaTime);
		}
		if (Input.GetKeyDown(KeyCode.W)) {
			colorModel.addFrequence(1f);
		}
		if (Input.GetKeyDown(KeyCode.S)) {
			colorModel.addFrequence(-1f);
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

	void ChangeColor(ColorModelScript.Color newColor) {
		destHue = (float)newColor / 360f;
		lerpTime = lerpDuration;
	}
		
	void GameOver() {
		Debug.Log("TOT!");
	}
}
