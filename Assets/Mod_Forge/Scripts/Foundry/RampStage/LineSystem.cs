using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineSystem : MonoBehaviour
{
    #region 单例封装

    private static LineSystem instance;

    public static LineSystem Instance
    {
        get
        {
            return instance;
        }
    }

    #endregion 单例封装
    /// <summary>
    /// 绕线杆父物体Transform组件
    /// </summary>
    public Transform LinesParentTrans = null;
    /// <summary>
    /// 绕线杆预制件
    /// </summary>
    public GameObject LinePrefab = null;
    /// <summary>
    /// 选择界面的Transform组件
    /// </summary>
    public Transform chooseLineTransform = null;
    /// <summary>
    /// 绕线杆组对应的Layout组件
    /// </summary>
    public GridLayoutGroup LinesLayout = null;
    /// <summary>
    /// 目标坐标
    /// </summary>
    private Vector3 targetVec;
    /// <summary>
    /// 原坐标
    /// </summary>
    private Vector3 oldVec;
    /// <summary>
    /// 是否正在移动
    /// </summary>
    public bool move = false;
    /// <summary>
    /// 移动时间
    /// </summary>
    private float moveTime;
    public GameObject[] Lines;
    public int[] LineDiameters;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        InitLineSystem();
        targetVec = chooseLineTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Magnitude(chooseLineTransform.localPosition - targetVec) < 0.5f)
        {
            chooseLineTransform.localPosition = targetVec;
            moveTime = 0f;
            move = false;
        }
        else
        {
            move = true;
            moveTime += Time.deltaTime * 5;
            Vector3 newVec = Vector3.Lerp(oldVec, targetVec, moveTime);
            chooseLineTransform.localPosition = newVec;
        }
    }

    void InitLineSystem()
    {
        Lines = new GameObject[LineDiameters.Length];
        for (int i = 0; i < LineDiameters.Length; i++)
        {
            Lines[i] = Instantiate(LinePrefab, LinesParentTrans);
            Lines[i].GetComponent<Line>().diameter = LineDiameters[i];
            Debug.Log(LineDiameters[i]);
        }
    }

    /// <summary>
    /// 点击显示更多按钮事件
    /// </summary>
    public void ShowMoreButtonClick()
    {
        oldVec = chooseLineTransform.localPosition;
        if (ChooseLineSystem.Instance.showMore)
        {
            targetVec = Vector3.zero;
            LinesLayout.constraintCount = 1;
            ChooseLineSystem.Instance.showMore = false;
        }
        else
        {
            targetVec = new Vector3(400, 0, 0);
            LinesLayout.constraintCount = 3;
            ChooseLineSystem.Instance.showMore = true;
        }
    }
}
