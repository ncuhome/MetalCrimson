using Mod_Console;
using Mod_Save;
using System;
using UnityEngine;

namespace Mod_Common
{
    public class ProjectConfiger:MonoBehaviour
    {
        private void Start()
        {
            DontDestroyOnLoad(gameObject);
            //设置使用的指令解释器
            ConsolePanel.Instance.interpreter = new AInterpreter();
            SettingsManager.Instance["Volume"] = 100 + "";
            SettingsManager.Instance["Move Up"] = (int)KeyCode.W + "";
            SettingsManager.Instance["Move Down"] = (int)KeyCode.S + "";
            SettingsManager.Instance["Move Left"] = (int)KeyCode.A + "";
            SettingsManager.Instance["Move Right"] = (int)KeyCode.D + "";
            SettingsManager.Instance["Attack"] = (int)KeyCode.J+"";
            SettingsManager.Instance["Debug"] = (int)KeyCode.F12 + "";
            ConsolePanel.Instance.Print("Loading Settings");
            SettingsManager.Instance.SaveSettings();
            ConsolePanel.Instance.Print("Saving Settings");
            ConsolePanel.Instance.Print($"音量大小：{SettingsManager.Instance["Volume"]}");
            ConsolePanel.Instance.SwitchUsing();
        }

        private void Update()
        {
            KeyCode code = (KeyCode)Convert.ToInt32(SettingsManager.Instance["Debug"]);

            if(Input.anyKeyDown)
            {
                if (Input.GetKeyDown(code))//开启/关闭控制台
                {
                    ConsolePanel.Instance.SwitchUsing();
                }
            }
        }
    }
}