using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    enum MoveDirection { backward = -1, forward = 1 }
    private float value;
    private float HammerValue;
    private MoveDirection HammerMoveDirection;
    public bool startQTE = false;
    public float moveSpeed;
    public float QTEValue;
    public float QTERange1, QTERange2;
    private MoveDirection moveDirection;
    public RectTransform QTERange1Transform, QTERange2Transform, QTEBackgroundTransform, QTEValueTransform, HammerTransform;
    private float t;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
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

    }

    // Update is called once per frame
    void Update()
    {
        if (startQTE)
        {
            value += Time.deltaTime * moveSpeed * (int)moveDirection;
            HammerValue += Time.deltaTime * moveSpeed * (int)HammerMoveDirection;
            if (Mathf.Abs(value) < 0.01f) { moveDirection = MoveDirection.forward; }
            if (Mathf.Abs(value - 1f) < 0.01f) { moveDirection = MoveDirection.backward; }
            if (HammerValue < 0.95f) { HammerMoveDirection = MoveDirection.forward; }
            if (HammerValue > 1f) { HammerMoveDirection = MoveDirection.backward; }

        }
        else
        {
            t += Time.deltaTime * (1 - value);
            value = Mathf.Lerp(value, 0, t * 5);
            HammerValue = Mathf.Lerp(HammerValue, 0, t * 5);
        }
        HammerTransform.eulerAngles = new Vector3(0, 0, -HammerValue * 120 + 30);
        QTEValueTransform.sizeDelta = new Vector2(QTEBackgroundTransform.rect.width, value * QTEBackgroundTransform.rect.height);
        QTEValueTransform.localPosition = new Vector3(QTEBackgroundTransform.localPosition.x, QTEBackgroundTransform.localPosition.y - QTEBackgroundTransform.rect.height / 2);

    }

    public float QTEJudgement()
    {
        startQTE = false;
        UIManager.Instance.ReturnButton.interactable = true;
        UIManager.Instance.CancelButton.interactable = true;
        UIManager.Instance.FinishButton.interactable = true;
        if ((value > QTEValue - QTERange1) && (value < QTEValue + QTERange1))
        {
            Debug.Log("QTEPerfect");
            return 1f;
        }
        else if ((value > QTEValue - QTERange2) && (value < QTEValue + QTERange2))
        {
            Debug.Log("QTEGreat");
            return 0.9f;
        }
        else
        {
            Debug.Log("QTEFailed");
            return 0.5f;
        }
    }

    public void StartQTE()
    {
        QTEBackgroundTransform.parent.gameObject.SetActive(true);
        moveDirection = MoveDirection.forward;
        value = 0;
        t = 0;

        QTERange1Transform.sizeDelta = new Vector2(QTEBackgroundTransform.rect.width, QTEBackgroundTransform.rect.height * QTERange1 * 2);
        QTERange1Transform.localPosition = new Vector3(QTEBackgroundTransform.localPosition.x, QTEBackgroundTransform.localPosition.y + QTEBackgroundTransform.rect.height * (QTEValue - 0.5f));
        QTERange2Transform.sizeDelta = new Vector2(QTEBackgroundTransform.rect.width, QTEBackgroundTransform.rect.height * QTERange2 * 2);
        QTERange2Transform.localPosition = new Vector3(QTEBackgroundTransform.localPosition.x, QTEBackgroundTransform.localPosition.y + QTEBackgroundTransform.rect.height * (QTEValue - 0.5f));

        startQTE = true;

        UIManager.Instance.ReturnButton.interactable = false;
        UIManager.Instance.CancelButton.interactable = false;
        UIManager.Instance.FinishButton.interactable = false;
    }

    public void FinishQTE()
    {
        QTEBackgroundTransform.parent.gameObject.SetActive(false);
        HammeringSystem.Instance.FinishHammering();
    }
}
