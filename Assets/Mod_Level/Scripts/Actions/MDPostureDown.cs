using ER.Entity2D;
using UnityEngine;

namespace Mod_Level
{
    public class MDPostureDown : MDAction
    {
        public ATActionRegion region;
        public MDPostureDown() { actionName = "PostureDown"; }
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
            region.Reset();
        }
        public override void StopAction()
        {
            region.gameObject.SetActive(false);
        }
    }
}