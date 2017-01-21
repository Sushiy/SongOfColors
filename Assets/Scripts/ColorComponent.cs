using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ColorComponent : MonoBehaviour {

	ColorModelScript colorModel;
    MeshRenderer meshrenderer;
    Collider2D collider;

	public ColorModelScript.Color ownColor;

    private void Awake()
    {
        meshrenderer = GetComponent<MeshRenderer>();
        collider = GetComponentInChildren<Collider2D>();
    }

	// Use this for initialization
	void Start () {
		ColorDeactivated();
        meshrenderer.material.color = Color.HSVToRGB((float)ownColor/360.0f, 1f, 1f);
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
        gameObject.layer = 0;
        meshrenderer.enabled = true;

    }

    protected void ColorDeactivated()
    {
        meshrenderer.enabled = false;
        gameObject.layer = 8;
    }
}
