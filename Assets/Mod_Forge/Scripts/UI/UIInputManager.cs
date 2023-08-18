using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIInputManager : MonoBehaviour
{
    #region 单例封装

    private static UIInputManager instance;

    public static UIInputManager Instance
    {
        get
        {
            return instance;
        }
    }

    #endregion 单例封装

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InitInputManager()
    {
        if (instance == null)
        {
            instance = this;
        }

        InputManager.inputActions.UI.QTE.performed += StartQTE;
        InputManager.inputActions.UI.QTE.canceled += FinishQTE;
        InputManager.inputActions.UI.Hammering.performed += StartHammering;

        InputManager.inputActions.UI.UpLine.performed += UpLine;
        InputManager.inputActions.UI.DownLine.performed += DownLine;
    }

    private void StartQTE(InputAction.CallbackContext ctx)
    {
        if (HammeringSystem.Instance.startHammering)
        {
            QTE.Instance.StartQTE();
        }
    }

    private void FinishQTE(InputAction.CallbackContext ctx)
    {
        if (HammeringSystem.Instance.startHammering)
        {
            HammeringSystem.Instance.HammerMaterial(QTE.Instance.QTEJudgement());
        }
    }

    private void StartHammering(InputAction.CallbackContext ctx)
    {
        HammeringSystem.Instance.StartHammering();
    }

    private void UpLine(InputAction.CallbackContext ctx)
    {
        if (RampStageSystem.Instance.startRamp)
        {
            RampStageSystem.Instance.UpLine();
        }
    }

    private void DownLine(InputAction.CallbackContext ctx)
    {
        if (RampStageSystem.Instance.startRamp)
        {
            RampStageSystem.Instance.DownLine();
        }
    }

}
