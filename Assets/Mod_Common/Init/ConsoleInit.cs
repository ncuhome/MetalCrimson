using ER;
using UnityEngine;

namespace Init
{
    public class ConsoleInit:MonoBehaviour
    {
        private void Awake()
        {
            Debug.Log("替换指令解释器");
            ConsolePanel.interpreter = new AInterpreter();
        }
    }
}