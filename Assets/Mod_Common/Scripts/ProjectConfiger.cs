using Mod_Console;
using UnityEngine;

namespace Mod_Common
{
    public class ProjectConfiger:MonoBehaviour
    {
        private void Start()
        {
            //����ʹ�õ�ָ�������
            ConsolePanel.Instance.interpreter = new AInterpreter();
        }
    }
}