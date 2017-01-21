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

	public MeshRenderer meshrenderer;

	//current Hue Value (HSV color)
	float currentHue;

	//Hue Value to interpolate to
	Color destColor;

    public ReactiveProperty<Color> currentColor;

	float lerpTime;

	public float lerpDuration = 2.5f;

	public void OnGround(){
		isOnGround = true;
	}

	public void OffGround(){
		isOnGround = false;
	}

    private void Awake()
    {
        currentColor = new ReactiveProperty<Color>();
        body = transform.GetComponent<Rigidbody2D>();
    }

	// Use this for initialization
	void Start ()
    {
        colorModel = ColorModelScript.instance;
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
        currentColor.Value = Color.Lerp(currentColor.Value, destColor, 1f - lerpTime / lerpDuration);
        meshrenderer.material.color = currentColor.Value; 
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

	void ChangeColor(ColorModelScript.Color newColor)
    {
        if(newColor == ColorModelScript.Color.NONE)
        {
            destColor = Color.white;
        }
        else
		    destColor = Color.HSVToRGB((float)newColor / 360f, 1f, 1f);
		lerpTime = lerpDuration;
	}
		
	void GameOver() {
		Debug.Log("TOT!");
	}
}
