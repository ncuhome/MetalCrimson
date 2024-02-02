using System.Collections;
using System.Collections.Generic;
using ER;
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

    }

    public void InitInputManager()
    {
        if (instance == null)
        {
            instance = this;
        }

        InputManager.InputActions.UI.QTE.performed += StartQTE;
        InputManager.InputActions.UI.QTE.canceled += FinishQTE;
        InputManager.InputActions.UI.Hammering.performed += StartHammering;

        InputManager.InputActions.UI.UpLine.performed += UpLine;
        InputManager.InputActions.UI.DownLine.performed += DownLine;
    }

    private void StartQTE(InputAction.CallbackContext ctx)
    {
        if (ConsolePanel.Instance.canvas.activeSelf) {return;}
        if (HammeringSystem.Instance.startHammering)
        {
            QTE.Instance.StartQTE();
        }
    }

    private void FinishQTE(InputAction.CallbackContext ctx)
    {
        if (ConsolePanel.Instance.canvas.activeSelf) {return;}
        if (HammeringSystem.Instance.startHammering)
        {
            HammeringSystem.Instance.HammerMaterial(QTE.Instance.QTEJudgement());
        }
    }

    private void StartHammering(InputAction.CallbackContext ctx)
    {
        if (ConsolePanel.Instance.canvas.activeSelf) {return;}
        HammeringSystem.Instance.StartHammering();
    }

    private void UpLine(InputAction.CallbackContext ctx)
    {
        if (ConsolePanel.Instance.canvas.activeSelf) {return;}
        if (RampStageSystem.Instance.startRamp)
        {
            RampStageSystem.Instance.UpLine();
        }
    }

    private void DownLine(InputAction.CallbackContext ctx)
    {
        if (ConsolePanel.Instance.canvas.activeSelf) {return;}
        if (RampStageSystem.Instance.startRamp)
        {
            RampStageSystem.Instance.DownLine();
        }
    }

}
