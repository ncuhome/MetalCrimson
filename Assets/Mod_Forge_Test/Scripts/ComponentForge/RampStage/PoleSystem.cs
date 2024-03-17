using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoleSystem : MonoBehaviour
{
    #region 单例封装

    private static PoleSystem instance;

    public static PoleSystem Instance
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
    public Transform polesParentTrans = null;
    /// <summary>
    /// 绕线杆预制件
    /// </summary>
    public GameObject polePrefab = null;
    /// <summary>
    /// 选择界面的Transform组件
    /// </summary>
    public Transform choosePoleTransform = null;
    /// <summary>
    /// 绕线杆组对应的Layout组件
    /// </summary>
    public GridLayoutGroup polesLayout = null;
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
    public GameObject[] poles;
    public int[] poleDiameters;
    public CurrentPole currentPole;
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
        InitPoleSystem();
        targetVec = choosePoleTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Magnitude(choosePoleTransform.localPosition - targetVec) < 0.5f)
        {
            choosePoleTransform.localPosition = targetVec;
            moveTime = 0f;
            move = false;
        }
        else
        {
            move = true;
            moveTime += Time.deltaTime * 5;
            Vector3 newVec = Vector3.Lerp(oldVec, targetVec, moveTime);
            choosePoleTransform.localPosition = newVec;
        }
    }

    void InitPoleSystem()
    {
        poles = new GameObject[poleDiameters.Length];
        for (int i = 0; i < poleDiameters.Length; i++)
        {
            poles[i] = Instantiate(polePrefab, polesParentTrans);
            poles[i].GetComponent<Pole>().diameter = poleDiameters[i];
            Debug.Log(poleDiameters[i]);
        }
    }

    /// <summary>
    /// 点击显示更多按钮事件
    /// </summary>
    public void ShowMoreButtonClick()
    {
        oldVec = choosePoleTransform.localPosition;
        if (ChoosePoleSystem.Instance.showMore)
        {
            targetVec = Vector3.zero;
            polesLayout.constraintCount = 1;
            ChoosePoleSystem.Instance.showMore = false;
        }
        else
        {
            targetVec = new Vector3(400, 0, 0);
            polesLayout.constraintCount = 3;
            ChoosePoleSystem.Instance.showMore = true;
        }
    }

    /// <summary>
    /// 隐藏材料面板
    /// </summary>
    public void HidePolePanel()
    {
        oldVec = choosePoleTransform.localPosition;
        targetVec = new Vector3(-400, 0, 0);
    }
    /// <summary>
    /// 显示材料面板
    /// </summary>
    public void ShowPolePanel()
    {
        oldVec = choosePoleTransform.localPosition;
        targetVec = Vector3.zero;
    }
}
