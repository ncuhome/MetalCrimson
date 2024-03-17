using TMPro;
using UnityEngine;
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
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    /// <summary>
    /// 刷新信息
    /// </summary>
    public void RefreshInfo()
    {
        if (MaterialItem == null) { return; }

        materialNameText.text = MaterialItem.GetText("Name");
        materialNumText.text = "拥有：" + MaterialItem.GetFloat("Num");

        if (UIManager.Instance.materialProgressing.gameObject.activeSelf)
        {
            if (MaterialItem.GetText("Tags").Equals("Product"))
            {
                imageScript.canBeDrag = false;
            }
            else
            {
                imageScript.canBeDrag = true;
            }
        }

        if (UIManager.Instance.foundry.gameObject.activeSelf)
        {
            if (MaterialItem.GetText("Tags").Equals("Product"))
            {
                imageScript.canBeDrag = true;
            }
            else
            {
                imageScript.canBeDrag = false;
            }
        }

        if (MaterialItem.GetFloat("Num") < MaterialSystem.Instance.needMaterialNum)
        {
            materialImage.color = Color.gray;
            imageScript.canBeDrag = false;
        }
        else
        {
            imageScript.canBeDrag = true;
            materialImage.color = Color.white;
        }
        
                
        MaterialSystem.Instance.FixedMaterialOjects();
    }

}