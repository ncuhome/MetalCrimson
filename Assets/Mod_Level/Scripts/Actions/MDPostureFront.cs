using ER.Entity2D;
using UnityEngine;

namespace Mod_Level
{
    public class MDPostureFront : MDAction
    {
        public ATActionRegion region;
        public MDPostureFront() { actionName = "PostureFront"; layer = 0; }
        public override void Initialize()
        {
            region.time = -1f;
            region.actor = manager.Owner;
            region.actionName = "PostureUpDefense";
            region.actionType = "PassiveDefense";
            region.GetComponent<ATActionResponse>().JudgeBreak = Defense;
            region.Initialize();
        }

        private bool Defense(ActionInfo info)
        {
            return true;
        }
        public override bool ActionJudge()
        {
            return true;
        }
        protected override void StartAction(params string[] keys)
        {
            ATCharacterState state = manager.Owner.GetAttribute<ATCharacterState>();
            if (state != null)
            {
                state.ActPosture = ATCharacterState.Posture.Front;
            }
            else
            {
                Debug.LogError("找不到角色状态器");
            }
            region.Reset();
        }
        protected override void StopAction(params string[] keys)
        {
            region.gameObject.SetActive(false);
        }
    }
}