using ER.Entity2D;
using UnityEngine;

namespace Mod_Battle
{
    public class MDPostureFront : MDAction
    {
        public MDPostureFront() { actionName = "PostureFront"; }
        public override void Initialize()
        {

        }
        public override void StartAction()
        {
            ATCharacterState state = manager.Owner.GetAttribute<ATCharacterState>();
            if (state != null)
            {
                state.posture = ATCharacterState.Posture.Front;
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