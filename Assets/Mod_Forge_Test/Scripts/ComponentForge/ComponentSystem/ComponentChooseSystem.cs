using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ComponentChooseSystem : MonoBehaviour
{
    #region 单例封装

    private static ComponentChooseSystem instance;

    public static ComponentChooseSystem Instance
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
    public RectTransform ComponentsParentTransform = null;
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
        oldVec = ComponentsParentTransform.localPosition;
        showPanel = true;
    }

    // Update is called once per frame
    void Update()
    {
        MoveComponents();
        MoveSlider();
        ShowSlider();
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
    private void MoveComponents()
    {
        if (showMore)
        {
            if (ComponentSystem.Instance.currentComponentNum <= 15)
            {
                slider.gameObject.SetActive(false);
            }
            else
            {
                slider.gameObject.SetActive(true);
            }
            float num = Mathf.Ceil(ComponentSystem.Instance.currentComponentNum / 3.0f - 5);
            float n = (num > 0) ? num : 0;
            ComponentsParentTransform.localPosition = new Vector3(oldVec.x - slider.value * 305 * n, oldVec.y);
        }
        else
        {
            if (ComponentSystem.Instance.currentComponentNum <= 5)
            {
                slider.gameObject.SetActive(false);
            }
            else
            {
                slider.gameObject.SetActive(true);
            }
            float num = ComponentSystem.Instance.currentComponentNum - 5;
            float n = (num > 0) ? num : 0;
            ComponentsParentTransform.localPosition = new Vector3(oldVec.x - slider.value * 305 * n, oldVec.y);
        }
    }

    private void ShowSlider()
    {
        if (showPanel)
        {
            slider.transform.localPosition = new Vector3(0, -ComponentSystem.Instance.chooseComponentTransform.localPosition.y - 530, 0);
        }
        else
        {
            slider.transform.localPosition = new Vector3(0, -520, 0);
        }
    }



    public void RefreshTypes()
    {
        slider.value = 0f;
    }
}
