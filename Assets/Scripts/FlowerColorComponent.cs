using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerColorComponent : ColorComponent
{

    public MeshRenderer leaf1;
    public MeshRenderer leaf2;
    public MeshRenderer leaf3;
    public MeshRenderer leaf4;

    Animator animThis;

    protected override void Awake()
    {
        meshrenderer = leaf1;
        animThis = GetComponent<Animator>();
        colliderThis = GetComponentInChildren<Collider2D>();
        lightThis = GetComponentInChildren<Light>();
        door = GetComponent<DoorScript>();
        hasDoor = door;
    }
    protected override void Update()
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
                    meshrenderer.enabled = hasDoor ? true : false;
                    lightThis.enabled = false;
                    colliderThis.gameObject.layer = hasDoor ? 0 : 8;
                    lerpTime = 0;
                    fadingOut = false;
                }
            }

            currentColor = Color.Lerp(currentColor, destColor, 1f - lerpTime / lerpDuration);
            lightThis.color = Color.Lerp(currentColor, destColor, 1f - lerpTime / lerpDuration);
            leaf1.material.color = currentColor;
            leaf2.material.color = currentColor;
            leaf3.material.color = currentColor;
            leaf4.material.color = currentColor;
        }
        else
        {

            if (Vector3.Distance(PlayerScript.instance.transform.position, transform.position) > range)
                ColorDeactivated();
            else if (!lightThis.enabled && colorModel.activeColor.Value == ownColor)
                ColorActivated();
        }
    }

    protected override void ColorActivated()
    {
        base.ColorActivated();
        animThis.SetBool("bOpen", true);
    }

    protected override void ColorDeactivated()
    {
        base.ColorDeactivated();
        animThis.SetBool("bOpen", false);
    }
}