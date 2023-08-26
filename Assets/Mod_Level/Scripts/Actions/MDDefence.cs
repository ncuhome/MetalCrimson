using ER.Entity2D;
using System.Collections.Generic;
using UnityEngine;

namespace Mod_Level
{
    public class MDDefence : MDAction
    {
        public List<ATActionRegion> regions;
        private ATCharacterState state;

        /// <summary>
        /// 是否处于防御状态
        /// </summary>
        private bool defensing = false;

        public MDDefence()
        { actionName = "Defence"; layer = 0; }

        public override void Initialize()
        {
            state = manager.Owner.GetAttribute<ATCharacterState>();

            #region 上架势

            regions[0].time = -1f;
            regions[0].actor = manager.Owner;
            regions[0].actionName = "PostureUpDefense";
            regions[0].actionType = "Defense";
            regions[0].EndEvent += () => defensing = false;
            regions[0].GetComponent<ATActionResponse>().JudgeBreak = Defense;
            regions[0].Initialize();

            #endregion 上架势

            #region 前架势

            regions[1].time = -1f;
            regions[1].actor = manager.Owner;
            regions[1].actionName = "PostureFrontDefense";
            regions[1].actionType = "Defense";
            regions[1].EndEvent += () => defensing = false;
            regions[1].GetComponent<ATActionResponse>().JudgeBreak = Defense;
            regions[1].Initialize();

            #endregion 前架势

            #region 下架势

            regions[2].time = -1f;
            regions[2].actor = manager.Owner;
            regions[2].actionName = "PostureDownDefense";
            regions[2].actionType = "Defense";
            regions[2].EndEvent += () => defensing = false;
            regions[2].GetComponent<ATActionResponse>().JudgeBreak = Defense;
            regions[2].Initialize();

            #endregion 下架势
        }

        private bool Defense(ActionInfo info)
        {
            return true;
        }

        public override bool ActionJudge()
        {
            if (state.stamina.Value <= 0) return false;
            return true;
        }

        protected override void StartAction(params string[] keys)
        {
            if (defensing) return;//处于防御状态不执行新的防御
            switch (state.ActPosture)
            {
                case ATCharacterState.Posture.Up:
                    regions[0].Reset();
                    defensing = true;
                    break;

                case ATCharacterState.Posture.Front:
                    regions[1].Reset();
                    defensing = true;
                    break;

                case ATCharacterState.Posture.Down:
                    regions[2].Reset();
                    defensing = true;
                    break;

                default:
                    break;
            }
        }

        protected override void StopAction(params string[] keys)
        {
            regions[0].gameObject.SetActive(false);
            regions[1].gameObject.SetActive(false);
            regions[2].gameObject.SetActive(false);
            defensing = false;
        }

        private void Update()
        {
            if (!defensing) return;
            state.stamina.ModifyValue(-10 * Time.deltaTime, manager.Owner);
            if (state.stamina.Value <= 0) StopAction();
        }
    }
}