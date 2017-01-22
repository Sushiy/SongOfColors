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

	public SkinnedMeshRenderer meshrenderer;
    Animator animThis;
	//current Hue Value (HSV color)
	float currentHue;

	//Hue Value to interpolate to
	Color destColor;
    public ReactiveProperty<Color> currentColor;

	float lerpTime;
	public float lerpDuration = 2.5f;

	RespawnerScript respawner;

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
<<<<<<< HEAD
        animThis = GetComponentInChildren<Animator>();
=======
		respawner = GetComponent<RespawnerScript> ();
>>>>>>> 1e86259b62f0fa73dab6935ddb1dd403597d6a06
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
        float destHue, s, v;
        Color.RGBToHSV(currentColor.Value, out destHue, out s, out v);
        currentHue = Mathf.Lerp(currentHue, destHue, 1f - lerpTime / lerpDuration);
        animThis.SetFloat("fNote", currentHue / 65);
        meshrenderer.material.color = currentColor.Value; 
	}

	void FixedUpdate() {
        bool walking = false;
		if (isOnGround && Input.GetKeyDown(KeyCode.Space))
        {
			body.AddForce(Vector2.up * jumpPower);
            animThis.SetTrigger("tJump");
		}
		if (Input.GetKey(KeyCode.A)) {
			body.transform.position += Vector3.left * (horizontalSpeed * Time.deltaTime);
			transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            walking = true;

        }
        else if (Input.GetKey(KeyCode.D)) {
			body.transform.position += Vector3.right * (horizontalSpeed * Time.deltaTime);
			transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            walking = true;
        }
        /*
        if (Input.GetKeyDown(KeyCode.W)) {
			colorModel.addFrequence(10f);
		}
		if (Input.GetKeyDown(KeyCode.S)) {
			colorModel.addFrequence(-10f);
        }*/

        animThis.SetBool("bWalking", walking);
        animThis.SetBool("bAir", !isOnGround);
    }

	void OnCollisionEnter2D(Collision2D other) {
		if (other.collider.tag == "Enemy") {
			GameOver();
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
		respawner.Respawn();
	}
}
