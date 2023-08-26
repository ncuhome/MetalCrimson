using ER.Entity2D;
using UnityEngine;

namespace Mod_Level
{
    public class MDIdle : MDAction
    {
        public MDIdle() { actionName = "Idle"; layer = 0; }
        public override void Initialize()
        {

        }
        public override bool ActionJudge()
        {
            return true;
        }

        protected override void StartAction(params string[] keys)
        {
            Debug.LogWarning("不要主动启用待机动作");
        }

        protected override void StopAction(params string[] keys)
        {
            Debug.LogWarning("不要关闭启用待机动作");
        }
    }
}