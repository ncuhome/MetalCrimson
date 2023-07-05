using Mod_Console;
using UnityEngine;

namespace Mod_Common
{
    public class ProjectConfiger:MonoBehaviour
    {
        private void Start()
        {
            //设置使用的指令解释器
            ConsolePanel.Instance.interpreter = new AInterpreter();
        }
    }
}