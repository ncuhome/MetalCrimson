using ER.Entity2D;
using System.Collections.Generic;
using UnityEngine;

namespace Mod_Level
{
    public class MDDefence : MDAction
    {
        [SerializeField]
        [Tooltip("武器的格挡区域")]
        private ATActionRegion region;
        [SerializeField]
        [Tooltip("武器的被动格挡区域")]
        private ATActionRegion region_passive;

        private ATCharacterState ownerState;

        public MDDefence()
        { actionName = "Defence"; controlType = ControlType.Bool; }

        public override void Initialize()
        {
            ownerState = manager.Owner.GetAttribute<ATCharacterState>();


            region.time = -1f;
            region.actor = manager.Owner;
            region.actionName = "Defence";
            region.actionType = "Defence";
            region.GetComponent<ATActionResponse>().JudgeBreak = Defence;
            region.Initialize();

            region_passive.time = -1f;
            region_passive.actor = manager.Owner;
            region_passive.actionName = "Defence";
            region_passive.actionType = "Defence";
            region_passive.GetComponent<ATActionResponse>().JudgeBreak = Defence;
            region_passive.Initialize();

        }

        private bool Defence(ActionInfo info)
        {
            return true;
        }

        public override bool ActionJudge()
        {
            if (ownerState.stamina.Value <= 0) return false;
            return true;
        }

        protected override void StartAction(params string[] keys)
        {
            if (state != ActionState.Disable) return;//处于防御状态不执行新的防御
            state = ActionState.Waiting;
            enabled = true;
        }

        protected override void StopAction(params string[] keys)
        {
            state = ActionState.Disable;
            enabled = false;
        }

        private void FuncDefence()
        {
            state = ActionState.Acting;
            region.Reset();
            region_passive.gameObject.SetActive(false);
        }

        private void FuncStop()
        {
            Debug.Log("防御停止");
            state = ActionState.Stoped;
            region.gameObject.SetActive(false);
            region_passive.Reset();
        }

        public override void ActionFunction(string key)
        {
            switch (key)
            {
                case "Defence":
                    FuncDefence();
                    break;

                case "Stop":
                    FuncStop();
                    break;

                default:
                    Debug.Log("未知参数");
                    break;
            }
        }

        private void Update()
        {
            if (state == ActionState.Acting) return;
            ownerState.stamina.ModifyValue(-10 * Time.deltaTime, manager.Owner);
            if (ownerState.stamina.Value <= 0) StopAction();
        }

        protected override void BreakAction(params string[] keys)
        {
            state = ActionState.Disable;
            enabled = false;
        }
    }
}