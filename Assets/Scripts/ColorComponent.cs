using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ColorComponent : MonoBehaviour {

	ColorModelScript colorModel;
    MeshRenderer meshrenderer;
    Collider2D colliderThis;
    Light lightThis;

	public ColorModelScript.Color ownColor;

    private void Awake()
    {
        meshrenderer = GetComponent<MeshRenderer>();
        colliderThis = GetComponentInChildren<Collider2D>();
        lightThis = GetComponentInChildren<Light>();
    }

	// Use this for initialization
	void Start () {
		ColorDeactivated();
        meshrenderer.material.color = Color.HSVToRGB((float)ownColor/360.0f, 1f, 1f);
        lightThis.color = meshrenderer.material.color;

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
	protected void ColorActivated()
    {
        colliderThis.gameObject.layer = 0;
        meshrenderer.enabled = true;
        lightThis.enabled = true;
    }

    protected void ColorDeactivated()
    {
        meshrenderer.enabled = false;
        lightThis.enabled = false;
        colliderThis.gameObject.layer = 8;
    }
}
