using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerScript : MonoBehaviour {

    public static PlayerScript instance;
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

    public ReactiveProperty<bool> cPlaying;
    public ReactiveProperty<bool> dPlaying;
    public ReactiveProperty<bool> ePlaying;
    public ReactiveProperty<bool> fPlaying;
    public ReactiveProperty<bool> gPlaying;
    public ReactiveProperty<bool> aPlaying;
    public ReactiveProperty<bool> bPlaying;
    public ReactiveProperty<bool> c2Playing;


    public void OnGround(){
		isOnGround = true;
	}

	public void OffGround(){
		isOnGround = false;
	}

    private void Awake()
    {
        instance = this;
        currentColor = new ReactiveProperty<Color>();
        body = transform.GetComponent<Rigidbody2D>();
        animThis = GetComponentInChildren<Animator>();
        respawner = GetComponent<RespawnerScript>();

        cPlaying = new ReactiveProperty<bool>(false);
        dPlaying = new ReactiveProperty<bool>(false);
        ePlaying = new ReactiveProperty<bool>(false);
        fPlaying = new ReactiveProperty<bool>(false);
        gPlaying = new ReactiveProperty<bool>(false);
        aPlaying = new ReactiveProperty<bool>(false);
        bPlaying = new ReactiveProperty<bool>(false);
        c2Playing = new ReactiveProperty<bool>(false);
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
	void Update ()
    {
        cPlaying.Value = GetAnyKey(new KeyCode[] { KeyCode.Q, KeyCode.A, KeyCode.Y });
        dPlaying.Value = GetAnyKey(new KeyCode[] { KeyCode.W, KeyCode.S, KeyCode.X });
        ePlaying.Value = GetAnyKey(new KeyCode[] { KeyCode.E, KeyCode.D, KeyCode.C });
        fPlaying.Value = GetAnyKey(new KeyCode[] { KeyCode.R, KeyCode.F, KeyCode.V });
        gPlaying.Value = GetAnyKey(new KeyCode[] { KeyCode.T, KeyCode.G, KeyCode.B });
        aPlaying.Value = GetAnyKey(new KeyCode[] { KeyCode.Z, KeyCode.H, KeyCode.N });
        bPlaying.Value = GetAnyKey(new KeyCode[] { KeyCode.U, KeyCode.J, KeyCode.M });
        c2Playing.Value = GetAnyKey(new KeyCode[] { KeyCode.I, KeyCode.K, KeyCode.Comma});

        if (lerpTime > 0f) {
			lerpTime -= Time.deltaTime;
			Mathf.Clamp (lerpTime, 0, lerpDuration);
		}
        currentColor.Value = Color.Lerp(currentColor.Value, destColor, 1f - lerpTime / lerpDuration);
        float destHue, s, v;
        Color.RGBToHSV(destColor, out destHue, out s, out v);
        Color.RGBToHSV(currentColor.Value, out currentHue, out s, out v);
        currentHue = Mathf.Lerp(currentHue, destHue, 1f - lerpTime / lerpDuration);
        animThis.SetFloat("fNote", 1 - currentHue);
        meshrenderer.material.color = currentColor.Value;
	}

    bool GetAnyKey(KeyCode[] keys)
    {
        foreach (KeyCode key in keys)
        {
            if (Input.GetKey(key))
                return true;
        }
        return false;
    }

	void FixedUpdate()
    {
        bool walking = false;

        if (isOnGround && Input.GetKeyDown(KeyCode.UpArrow))
        {
            body.AddForce(Vector2.up * jumpPower);
            animThis.SetTrigger("tJump");
		}
		if (Input.GetKey(KeyCode.LeftArrow)) {
			body.transform.position += Vector3.left * (horizontalSpeed * Time.deltaTime);
			transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            walking = true;

        }
        else if (Input.GetKey(KeyCode.RightArrow)) {
			body.transform.position += Vector3.right * (horizontalSpeed * Time.deltaTime);
			transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            walking = true;
        }
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
