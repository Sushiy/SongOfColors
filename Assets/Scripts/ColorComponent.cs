using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class ColorComponent : MonoBehaviour {

	protected ColorModelScript colorModel;
    protected MeshRenderer meshrenderer;
    protected Collider2D colliderThis;
    protected Light lightThis;

	public ColorModelScript.Color ownColor;
    public float range = 5;

	protected Color currentColor;
	protected Color destColor;

	protected float lerpTime;
	protected float lerpDuration = 1f;

	protected bool fadingOut = false;

	protected DoorScript door;
	protected bool hasDoor;

	MovementScript movement;
	bool hasMovement;

	protected virtual void Awake()
    {
        meshrenderer = GetComponent<MeshRenderer>();
        colliderThis = GetComponentInChildren<Collider2D>();
        lightThis = GetComponentInChildren<Light>();

		door = GetComponent<DoorScript>();
		hasDoor = door;
    }

	// Use this for initialization
	protected virtual void Start ()
    {
		ColorDeactivated();
        
        lightThis.color = Color.HSVToRGB((float)ownColor / 360.0f, 1f, 1f); ;

        colorModel = ColorModelScript.instance;

		colorModel.activeColor
			.Where(x => ownColor == x && Vector3.Distance(PlayerScript.instance.transform.position, transform.position) < range)
			.Subscribe (x => ColorActivated())
			.AddTo(gameObject); 

		colorModel.oldColor
			.Where(x => ownColor == x)
			.Subscribe (x => ColorDeactivated())
			.AddTo(gameObject);
	}

	protected virtual void Update()
	{
        if (lerpTime > 0f)
        {
            if (!fadingOut)
            {
                lerpTime -= Time.deltaTime;
                Mathf.Clamp(lerpTime, 0f, lerpDuration);
            }
            else
            {
                lerpTime -= 5 * Time.deltaTime;
                if (lerpTime < 0.1f)
                {
                    if (meshrenderer != null)
                        meshrenderer.enabled = hasDoor ? true : false;
                    lightThis.enabled = false;
                    colliderThis.gameObject.layer = hasDoor ? 0 : 8;
                    lerpTime = 0;
                    fadingOut = false;
                }
            }

            currentColor = Color.Lerp(currentColor, destColor, 1f - lerpTime / lerpDuration);
            lightThis.color = Color.Lerp(currentColor, destColor, 1f - lerpTime / lerpDuration);
            if (meshrenderer != null)
                meshrenderer.material.color = currentColor;
        }
        else
        {

            if (Vector3.Distance(PlayerScript.instance.transform.position, transform.position) > range)
                ColorDeactivated();
            else if (!lightThis.enabled && colorModel.activeColor.Value == ownColor)
                ColorActivated();
        }
	}

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, range);
    }

    protected virtual void ColorActivated()
    {
		currentColor = new Color(0, 0, 0);
		destColor = Color.HSVToRGB((float)ownColor/360.0f, 1f, 1f);
		lerpTime = lerpDuration;
		colliderThis.gameObject.layer = 0;
        if(meshrenderer != null)
            meshrenderer.enabled = true;
        lightThis.enabled = true;

		if (hasDoor) {
			door.MoveUp();
		}
    }

	protected virtual void ColorDeactivated()
    {
		fadingOut = true;
		destColor= new Color(0, 0, 0);
		lerpTime = lerpDuration;

		if (hasDoor) {
			door.MoveDown();
		}
    }
}
