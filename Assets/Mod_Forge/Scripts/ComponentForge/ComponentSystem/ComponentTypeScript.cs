using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ComponentTypeScript : MonoBehaviour
{
    public int typeID;
    public Image typeImage;
    public TMP_Text typeText;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChooseType()
    {
        ComponentSystem.Instance.currentTypeID = typeID;
        ComponentChooseSystem.Instance.RefreshTypes();
        ComponentSystem.Instance.RefreshTypes();
    }
}
