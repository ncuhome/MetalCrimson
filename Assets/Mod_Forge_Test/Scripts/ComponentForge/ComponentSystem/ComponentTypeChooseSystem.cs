using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ComponentTypeChooseSystem : MonoBehaviour
{
    #region 单例封装

    private static ComponentTypeChooseSystem instance;

    public static ComponentTypeChooseSystem Instance
    {
        get
        {
            return instance;
        }
    }

    #endregion 单例封装

    /// <summary>
    /// 是否扩展选择区
    /// </summary>
    public bool showMore = false;
    /// <summary>
    /// 扩展按钮组件
    /// </summary>
    public ShowMoreComponent showMoreButton = null;
    /// <summary>
    /// 滑动条组件
    /// </summary>
    public Slider slider = null;
    /// <summary>
    /// 材料父物体Transform组件
    /// </summary>
    public RectTransform ComponentTypeParentTransform = null;
    /// <summary>
    /// 父物体基准位置
    /// </summary>
    private Vector3 oldVec;
    /// <summary>
    /// 滑动条目标值
    /// </summary>
    public float targetValue;
    /// <summary>
    /// 滑动条原有值
    /// </summary>
    private float oldValue;
    /// <summary>
    /// 计时器
    /// </summary>
    private float timer;
    /// <summary>
    /// 是否能进行拖动滑动条
    /// </summary>
    public bool sliderDrag;

    public bool showPanel;
    /// <summary>
    /// 目标坐标
    /// </summary>
    private Vector3 targetPanelVec;
    /// <summary>
    /// 原坐标
    /// </summary>
    private Vector3 oldPanelVec;
    /// <summary>
    /// 是否正在移动
    /// </summary>
    public bool move = false;
    /// <summary>
    /// 移动时间
    /// </summary>
    private float moveTime;

    public Transform chooseTypeTransform;

    public GridLayoutGroup typeLayout;
    /// <summary>
    /// 初始化单例
    /// </summary>
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
        oldVec = ComponentTypeParentTransform.localPosition;
        showPanel = true;
    }

    // Update is called once per frame
    void Update()
    {
        MoveComponentType();
        MoveSlider();
        ShowSlider();
        MovePanel();
    }
    /// <summary>
    /// 进行滑动条的拖动或者自动移动
    /// </summary>
    private void MoveSlider()
    {
        if (sliderDrag)
        {
            oldValue = slider.value;
            targetValue = slider.value;
            timer = 0;
        }
        else
        {
            if (Mathf.Abs(targetValue - slider.value) < 0.01f)
            {
                slider.value = targetValue;
                oldValue = targetValue;
                timer = 0;
                sliderDrag = true;
            }
            else
            {
                timer += Time.deltaTime;
                slider.value = Mathf.Lerp(oldValue, targetValue, timer * 2.5f);
            }
        }
    }
    /// <summary>
    /// 移动材料位置
    /// </summary>
    private void MoveComponentType()
    {
        if (showMore)
        {
            if (ComponentTypeParentTransform.childCount <= 15)
            {
                slider.gameObject.SetActive(false);
            }
            else
            {
                slider.gameObject.SetActive(true);
            }
            float num = Mathf.Ceil(ComponentTypeParentTransform.childCount / 3.0f - 5);
            float n = (num > 0) ? num : 0;
            ComponentTypeParentTransform.localPosition = new Vector3(oldVec.x, oldVec.y + slider.value * 200 * n);
        }
        else
        {
            if (ComponentTypeParentTransform.childCount <= 5)
            {
                slider.gameObject.SetActive(false);
            }
            else
            {
                slider.gameObject.SetActive(true);
            }
            float num = ComponentTypeParentTransform.childCount - 5;
            float n = (num > 0) ? num : 0;
            ComponentTypeParentTransform.localPosition = new Vector3(oldVec.x, oldVec.y + slider.value * 200 * n);
        }
    }

    private void ShowSlider()
    {
        if (showPanel)
        {
            slider.transform.localPosition = new Vector3(-ComponentSystem.Instance.chooseTypeTransform.localPosition.x + 930, 0, 0);
        }
        else
        {
            slider.transform.localPosition = new Vector3(950, 0, 0);
        }
    }

    public void MovePanel()
    {

        if (Vector3.Magnitude(chooseTypeTransform.localPosition - targetPanelVec) < 0.5f)
        {
            chooseTypeTransform.localPosition = targetPanelVec;
            moveTime = 0f;
            move = false;
        }
        else
        {
            move = true;
            moveTime += Time.deltaTime * 5;
            Vector3 newVec = Vector3.Lerp(oldPanelVec, targetPanelVec, moveTime);
            chooseTypeTransform.localPosition = newVec;
        }
    }

    public void ShowMoreButtonClick()
    {
        oldPanelVec = chooseTypeTransform.localPosition;
        if (showMore)
        {
            targetPanelVec = new Vector3(0, 0, 0);
            typeLayout.constraintCount = 1;
            showMore = false;
        }
        else
        {
            targetPanelVec = new Vector3(-395, 0, 0);
            typeLayout.constraintCount = 3;
            showMore = true;
        }
    }
}
