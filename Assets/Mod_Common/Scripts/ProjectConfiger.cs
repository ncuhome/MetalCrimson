using Mod_Console;
using Mod_Save;
using UnityEngine;

namespace Mod_Common
{
    public class ProjectConfiger:MonoBehaviour
    {
        private void Start()
        {
            //����ʹ�õ�ָ�������
            ConsolePanel.Instance.interpreter = new AInterpreter();
            SettingsManager.Instance["Volume"] = 100.ToString();
            SettingsManager.Instance["Move Up"] = KeyCode.W.ToString();
            SettingsManager.Instance["Move Down"] = KeyCode.S.ToString();
            SettingsManager.Instance["Move Left"] = KeyCode.A.ToString();
            SettingsManager.Instance["Move Right"] = KeyCode.D.ToString();
            SettingsManager.Instance["Attack"] = KeyCode.J.ToString();
            ConsolePanel.Instance.Print("Loading Settings");
            SettingsManager.Instance.SaveSettings();
            ConsolePanel.Instance.Print("Saving Settings");
            ConsolePanel.Instance.Print($"������С��{SettingsManager.Instance["Volume"]}");
        }
    }
}