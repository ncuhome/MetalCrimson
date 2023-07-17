using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class MaterialScript : MonoBehaviour
{
    public TextMeshProUGUI materialNameText = null;
    public TextMeshProUGUI materialNumText = null;
    public Image materialImage = null;
    public MaterialImage imageScript = null;
    public GameObject materialVisual = null;
    public ER.Items.ItemVariable MaterialItem = null;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (MaterialItem.GetInt("Num") == 0)
        {
            materialImage.color = Color.gray;
            imageScript.canBeDrag = false;
        }
        else
        {
            imageScript.canBeDrag = true;
            materialImage.color = Color.white;
        }
        materialNameText.text = MaterialItem.GetText("Name");
        materialNumText.text = "拥有："+ MaterialItem.GetInt("Num");
    }
}
