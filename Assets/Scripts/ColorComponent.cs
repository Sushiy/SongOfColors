using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ColorComponent : MonoBehaviour {

	ColorModelScript colorModel;
    SpriteRenderer spriterenderer;
    Collider2D collider;

	public ColorModelScript.Color ownColor;

    private void Awake()
    {
        spriterenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
    }

	// Use this for initialization
	void Start () {
		ColorDeactivated();

		colorModel = ColorModelScript.instance;

		colorModel.activeColor
			.Where(x => ownColor == x)
			.Subscribe (x => ColorActivated())
			.AddTo(gameObject); 

		colorModel.oldColor
			.Where(x => ownColor == x)
			.Subscribe (x => ColorDeactivated())
			.AddTo(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	protected void ColorActivated()
    {
        spriterenderer.color = Color.HSVToRGB ((int)ownColor / 360f, 1f, 1f);
        collider.enabled = true;

    }

	protected void ColorDeactivated()
    {
        spriterenderer.color = new Color(0,0,0,0);
        collider.enabled = false;
    }
}
