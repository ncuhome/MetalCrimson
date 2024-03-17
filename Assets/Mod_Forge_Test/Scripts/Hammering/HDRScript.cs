using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HDRScript : MonoBehaviour
{
    public bool colored;
    public Color color;
    public Color HDRColor;
    public Image image;
    public Image HDRImage;
    public Material HDRmaterial1, HDRmaterial2;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        HDRImage = transform.GetChild(0).GetComponent<Image>();
        color = image.color;
        HDRColor = HDRImage.material.color;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RefreshColor()
    {
        HDRImage.enabled = true;
        if (colored)
        {
            color = image.color;
            float intensity = HammeringSystem.Instance.chainTimes * 1.1f - 2;
            float factor = Mathf.Pow(2, intensity);
            HDRColor = new Color(color.r * factor, color.g * factor, color.b * factor);
            HDRImage.material.color = HDRColor;
        }
        else
        {
            color = image.color;
            HDRColor = ColorController.Instance.HDRcolors[HammeringSystem.Instance.chainTimes - 1].color;
            HDRImage.material.color = HDRColor;
            image.color = ColorController.Instance.colors[HammeringSystem.Instance.chainTimes - 1];
        }
    }



    public void HideColor()
    {
        image.color = Color.white;
        HDRImage.enabled = false;
    }
}
