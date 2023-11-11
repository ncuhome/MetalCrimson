using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ER.Items;

[System.Serializable]
public struct ComponentStruct
{
    public string NameTmp;
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
}
[System.Serializable]
public class WeaponInfo
{
    public string Name;
    public ItemVariable weaponItem;
    public Transform handlePointTrans;
    public List<ComponentStruct> components = new List<ComponentStruct>();
}
public class Weapon : MonoBehaviour
{
    public string Name;
    public ItemVariable weaponItem;
    public WeaponInfo weaponInfo;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FinishBuild()
    {
        weaponInfo = new WeaponInfo();
        weaponInfo.weaponItem = weaponItem;
        weaponInfo.Name = Name;

        transform.localEulerAngles = new Vector3(0, 0, -135);
        transform.localScale = new Vector3(0.01f, 0.01f);

        weaponInfo.handlePointTrans = FindHandlePoint();
        for (int i = 0; i < transform.childCount; i++)
        {
            ComponentStruct component = new ComponentStruct();
            Transform childTrans = transform.GetChild(i);
            component.NameTmp = childTrans.GetComponent<ComponentSprite>().componentScript.ComponentItem.GetText("NameTmp");
            component.position = childTrans.position - weaponInfo.handlePointTrans.position;
            component.rotation = childTrans.rotation;
            component.scale = childTrans.lossyScale;
            weaponInfo.components.Add(component);
        }
        BagStore.Instance.weaponInfos.Add(weaponInfo);
    }

    public Transform FindHandlePoint()
    {
        Transform[] transforms = GetComponentsInChildren<Transform>();
        foreach (Transform transform in transforms)
        {
            if (transform.name == "HandlePoint")
            {
                return transform;
            }
        }
        return null;
    }


}
