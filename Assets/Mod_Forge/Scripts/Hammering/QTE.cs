using UnityEngine;
using System.Collections;
public enum QTEPerf
{
    None, Bad, Great, Perfect
}

public class QTE : MonoBehaviour
{
    #region 单例封装

    private static QTE instance;

    public static QTE Instance
    {
        get
        {
            return instance;
        }
    }

    #endregion 单例封装

    private enum MoveDirection
    { backward = -1, forward = 1 }

    private float value;
    private float HammerValue;
    private MoveDirection HammerMoveDirection;
    public bool startQTE = false;
    public float moveSpeed;
    public float QTEValue;
    public float[] QTEValues;
    public float QTERange1, QTERange2;
    public int QTERangeNum;
    private MoveDirection moveDirection;
    public RectTransform QTERange1Transform, QTERange2Transform, QTEBackgroundTransform, QTEValueTransform, HammerTransform;
    public GameObject QTEPrefab;
    public RectTransform[] QTERange1Transforms, QTERange2Transforms;
    public Transform QTEParent;
    private float t;

    private float waitTime;
    public QTEPerf[] QTEPerfs;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (startQTE)
        {
            if (waitTime < 0.5f)
            {
                waitTime += Time.deltaTime;
            }
            else
            {
                value += Time.deltaTime * moveSpeed * (int)moveDirection;
                HammerValue += Time.deltaTime * moveSpeed * (int)HammerMoveDirection;
                if (Mathf.Abs(value) < 0.01f) { moveDirection = MoveDirection.forward; }
                if (Mathf.Abs(value - 1f) < 0.01f) { moveDirection = MoveDirection.backward; }
                if (HammerValue < 0.95f) { HammerMoveDirection = MoveDirection.forward; }
                if (HammerValue > 1f) { HammerMoveDirection = MoveDirection.backward; }
            }

            if (QTEPerfs[QTERangeNum - 1] != QTEPerf.None) { FinishQTE(); }

            if (moveDirection == MoveDirection.backward)
            {
                if (HammeringSystem.Instance.startHammering)
                {
                    for (int i = 0; i < QTERangeNum; i++)
                    {
                        if (QTEPerfs[i] == QTEPerf.None)
                        {
                            QTEJudgement();
                        }
                    }
                    FinishQTE();
                }
            }
        }
        else
        {
            t += Time.deltaTime * (1 - value);
            value = Mathf.Lerp(value, 0, t * 5);
            HammerValue = Mathf.Lerp(HammerValue, 0, t * 5);
        }
        HammerTransform.eulerAngles = new Vector3(0, 0, -HammerValue * 120 + 30);
        QTEValueTransform.localPosition = new Vector3(QTEBackgroundTransform.localPosition.x + (value - 0.5f) * QTEBackgroundTransform.rect.height, QTEBackgroundTransform.localPosition.y);
    }


    public void QTEJudgement()
    {
        if (!startQTE) { return; }
        for (int i = 0; i < QTERangeNum; i++)
        {
            if (Mathf.Abs(value - QTEValues[i]) < QTERange1)
            {
                Debug.Log("Perfect");
                QTEPerfs[i] = QTEPerf.Perfect;
                return;
            }
            else if (Mathf.Abs(value - QTEValues[i]) < QTERange2)
            {
                Debug.Log("Great");
                QTEPerfs[i] = QTEPerf.Great;
                return;
            }
        }
        for (int i = 0; i < QTERangeNum; i++)
        {
            if (QTEPerfs[i] == QTEPerf.None)
            {
                Debug.Log("Bad" + i);
                QTEPerfs[i] = QTEPerf.Bad;
                return;
            }
        }
    }

    public void StartQTE()
    {
        if (HammeringSystem.Instance.forgeCompleteness != 0) { return; }
        if (startQTE) { return; }

        Debug.Log("StartQTE");
        QTEBackgroundTransform.parent.gameObject.SetActive(true);
        moveDirection = MoveDirection.forward;
        value = 0;
        t = 0;

        QTEValues = new float[QTERangeNum];
        QTERange1Transforms = new RectTransform[QTERangeNum];
        QTERange2Transforms = new RectTransform[QTERangeNum];

        for (int i = 0; i < QTERangeNum; i++)
        {
            GameObject QTEObject = Instantiate(QTEPrefab);
            QTEObject.transform.parent = QTEParent;
            QTEObject.transform.localScale = Vector3.one;
            QTEObject.transform.localPosition = Vector3.zero;
            QTEObject.transform.SetSiblingIndex(i + 1);
            QTERange1Transforms[i] = QTEObject.transform.Find("QTERange1").GetComponent<RectTransform>();
            QTERange2Transforms[i] = QTEObject.transform.Find("QTERange2").GetComponent<RectTransform>();

            while (true)
            {
                bool b = true;
                QTEValues[i] = Random.Range(QTERange2, 1 - QTERange2);
                for (int j = 0; j < i; j++)
                {
                    if (Mathf.Abs(QTEValues[j] - QTEValues[i]) < QTERange2 * 2)
                    {
                        b = false;
                    }
                }
                if (b) { break; }
            }

            QTERange1Transforms[i].sizeDelta = new Vector2(QTEBackgroundTransform.rect.width, QTEBackgroundTransform.rect.height * QTERange1 * 2);
            QTERange1Transforms[i].localPosition = new Vector3(QTEBackgroundTransform.localPosition.x + QTEBackgroundTransform.rect.height * (QTEValues[i] - 0.5f), QTEBackgroundTransform.localPosition.y);
            QTERange2Transforms[i].sizeDelta = new Vector2(QTEBackgroundTransform.rect.width, QTEBackgroundTransform.rect.height * QTERange2 * 2);
            QTERange2Transforms[i].localPosition = new Vector3(QTEBackgroundTransform.localPosition.x + QTEBackgroundTransform.rect.height * (QTEValues[i] - 0.5f), QTEBackgroundTransform.localPosition.y);
        }

        for (int i = 0; i < QTERangeNum; i++)
        {
            for (int j = 0; j < i; j++)
            {
                if (QTEValues[i] < QTEValues[j])
                {
                    float t = QTEValues[i];
                    QTEValues[i] = QTEValues[j];
                    QTEValues[j] = t;
                }
            }
        }
        // QTERange1Transform.sizeDelta = new Vector2(QTEBackgroundTransform.rect.width, QTEBackgroundTransform.rect.height * QTERange1 * 2);
        // QTERange1Transform.localPosition = new Vector3(QTEBackgroundTransform.localPosition.x + QTEBackgroundTransform.rect.height * (QTEValue - 0.5f), QTEBackgroundTransform.localPosition.y);
        // QTERange2Transform.sizeDelta = new Vector2(QTEBackgroundTransform.rect.width, QTEBackgroundTransform.rect.height * QTERange2 * 2);
        // QTERange2Transform.localPosition = new Vector3(QTEBackgroundTransform.localPosition.x + QTEBackgroundTransform.rect.height * (QTEValue - 0.5f), QTEBackgroundTransform.localPosition.y);

        QTEPerfs = new QTEPerf[QTERangeNum];
        startQTE = true;

        UIManager.Instance.ReturnButton.interactable = false;
        UIManager.Instance.CancelButton.interactable = false;
        //UIManager.Instance.FinishButton.interactable = false;
    }

    public void FinishQTE()
    {
        Debug.Log("FinishQTE");
        for (int i = 0; i < QTERangeNum; i++)
        {
            Destroy(QTERange1Transforms[i].gameObject);
            Destroy(QTERange2Transforms[i].gameObject);
        }

        HammeringSystem.Instance.HammerMaterial();
        startQTE = false;
        UIManager.Instance.ReturnButton.interactable = true;
        UIManager.Instance.CancelButton.interactable = true;
        //UIManager.Instance.FinishButton.interactable = true;
        QTEBackgroundTransform.parent.gameObject.SetActive(false);
        HammeringSystem.Instance.FinishHammering();
    }

    public bool QTEPerfect()
    {
        foreach (var i in QTEPerfs)
        {
            if (i != QTEPerf.Perfect) { return false; }
        }
        return true;
    }

    public int QTEFailed()
    {
        int i = 0;
        foreach (var QTEPerf in QTEPerfs)
        {
            if (QTEPerf == QTEPerf.Bad)
            {
                i++;
            }
        }
        return i;
    }

}