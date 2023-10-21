using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollSlider : MonoBehaviour
{
    public Slider slider;
    public float speed = 0.05f;

    public float _mapWidth;
    public float _mapHight;
    public Transform _map;
    // Start is called before the first frame update
    void Start()
    {
        _map = GetComponent<Transform>();
    }


    void Update()
    {
        //Debug.Log(IsTouchInUi(Input.mousePosition) ? "在地图上" : "不在上面");
        if (IsTouchInUi(Input.mousePosition))
        {
            if (Input.mouseScrollDelta.y != 0)
            {
                slider.value -= speed * Input.mouseScrollDelta.y;
            }
        }
    }

    /// <summary>
    /// 获取ui的屏幕坐标
    /// </summary>
    /// <param name="trans">UI物体</param>
    /// <returns></returns>
    Vector3 GetUiToScreenPos(Transform trans)
    {
        _mapWidth = trans.GetComponent<RectTransform>().rect.width;//获取ui的实际宽度
        _mapHight = trans.GetComponent<RectTransform>().rect.height;//长度
        Vector2 pos2D;
        pos2D = RectTransformUtility.WorldToScreenPoint(Camera.main,trans.position);
        Vector3 newPos = new Vector3(pos2D.x, pos2D.y, 0);
        return newPos;
    }

    /// <summary>
    /// 判断是否在ui上
    /// </summary>
    /// <param name="pos">输入的坐标信息</param>
    /// <returns></returns>
    bool IsTouchInUi(Vector3 pos)
    {
        bool isInRect = false;
        Vector3 newPos = GetUiToScreenPos(_map);
        //Debug.Log(pos + " " + newPos);
        if (pos.x < (newPos.x+_mapWidth/2) && pos.x > (newPos.x-_mapWidth/2) && 
            pos.y < (newPos.y+_mapHight/2) && pos.y > (newPos.y-_mapHight/2))
        {
            isInRect = true;
        }
        return isInRect;
    }

    
}
