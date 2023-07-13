using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MaterialScript : MonoBehaviour
{
    public string materialName = null;
    public int materialNum = 0; 
    public TextMeshProUGUI materialNameText = null;
    public TextMeshProUGUI materialNumText = null;
    public Image materialImage = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (materialNum == 0)
        {
            materialImage.color = Color.gray;
        }
        materialNameText.text = materialName;
        materialNumText.text = "拥有："+materialNum.ToString();
    }
}
