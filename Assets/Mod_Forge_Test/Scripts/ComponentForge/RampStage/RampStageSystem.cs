using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RampStageSystem : MonoBehaviour
{
    #region 单例封装

    private static RampStageSystem instance;

    public static RampStageSystem Instance
    {
        get
        {
            return instance;
        }
    }

    #endregion 单例封装


    public Animator animator;

    public float value;
    public float[] ticks;

    public RectTransform validTrans;
    public RectTransform limiterTrans;

    public GameObject linePrepared;
    public GameObject lineStart;
    public Transform upLinePar;
    public Transform downLinePar;
    public Image[] upLines;
    public Image[] downLines;
    public int lineIndex;
    public LineScript lineScript;

    public bool startRamp;
    public bool upLine;
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
        for (int i = 0; i < upLinePar.childCount; i++)
        {
            upLines[i] = upLinePar.GetChild(i).GetComponent<Image>();
            upLines[i].fillAmount = 0f;
        }
        for (int i = 0; i < downLinePar.childCount; i++)
        {
            downLines[i] = downLinePar.GetChild(i).GetComponent<Image>();
            downLines[i].fillAmount = 0f;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public float GetValue(float posX)
    {
        float value = (posX - validTrans.localPosition.x) / validTrans.sizeDelta.x + 0.5f;
        if (value > 1) { return 1; }
        if (value < 0) { return 0; }
        return value;
    }

    public Vector3 GetPos(float value)
    {
        float posX = (value - 0.5f) * validTrans.sizeDelta.x + validTrans.localPosition.x;
        return new Vector3(posX, limiterTrans.localPosition.y, 0);
    }

    public float GetNearValue(float value)
    {
        float minDuration = 1f;
        float nearValue = 0f;
        for (int i = 0; i < ticks.Length; i++)
        {
            if (Mathf.Abs(ticks[i] - value) < minDuration)
            {
                minDuration = Mathf.Abs(ticks[i] - value);
                nearValue = ticks[i];
            }
        }
        return nearValue;
    }

    public void UpLine()
    {
        if (upLine)
        {
            upLines[lineIndex].fillAmount = 1f;
            upLine = false;
        }
    }

    public void DownLine()
    {
        if (!upLine)
        {
            downLines[lineIndex].fillAmount = 1f;
            lineIndex++;
            upLine = true;
        }
    }


}
