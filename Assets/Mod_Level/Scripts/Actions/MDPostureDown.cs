using ER.Entity2D;
using UnityEngine;

namespace Mod_Level
{
    public class MDPostureDown : MDAction
    {
        public MDPostureDown() { actionName = "PostureDown"; }
        public override void Initialize()
        {

        }
        public override void StartAction()
        {
            ATCharacterState state = manager.Owner.GetAttribute<ATCharacterState>();
            if (state != null)
            {
                state.posture = ATCharacterState.Posture.Down;
            }
            else
            {
                Debug.LogError("找不到角色状态器");
            }
        }
        public override void StopAction()
        {

        }
    }
}