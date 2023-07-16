using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class MoveBackMaterial : MonoBehaviour, IPointerClickHandler
{
    /// <summary>
    /// 在炉子里的顺序ID
    /// </summary>
    public int ID;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// 点击时返还材料
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        HammeringSystem.Instance.MoveBackMaterial(ID);
    }
}
