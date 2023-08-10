using ER.Entity2D;
using System.Collections.Generic;

namespace Mod_Battle
{
    public class MDDefense : MDAction
    {
        public List<ATActionRegion> regions;
        private ATCharacterState state;
        /// <summary>
        /// 是否处于防御状态
        /// </summary>
        private bool defensing = false;
        public MDDefense() { actionName = "Defense"; }
        public override void Initialize()
        {
            state = manager.Owner.GetAttribute<ATCharacterState>();

            #region 上架势
            regions[0].time = 0.5f;
            regions[0].actor = manager.Owner;
            regions[0].actionName = "PostureUpDefense";
            regions[0].actionType = "Defense";
            regions[0].EndEvent += () => defensing = false;
            regions[0].GetComponent<ATActionResponse>().JudgeBreak = Defense;
            regions[0].Initialize();
            #endregion

            #region 前架势
            regions[1].time = 0.5f;
            regions[1].actor = manager.Owner;
            regions[1].actionName = "PostureFrontDefense";
            regions[1].actionType = "Defense";
            regions[1].EndEvent += () => defensing = false;
            regions[1].GetComponent<ATActionResponse>().JudgeBreak = Defense;
            regions[1].Initialize();
            #endregion

            #region 下架势
            regions[2].time = 0.5f;
            regions[2].actor = manager.Owner;
            regions[2].actionName = "PostureDownDefense";
            regions[2].actionType = "Defense";
            regions[2].EndEvent += () => defensing = false;
            regions[2].GetComponent<ATActionResponse>().JudgeBreak = Defense;
            regions[2].Initialize();
            #endregion
        }

        private bool Defense(ActionInfo info)
        {
            return true;
        }
        public override void StartAction()
        {
            if (defensing) return;//处于防御状态不执行新的攻击
            switch (state.posture)
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
        public override void StopAction()
        {

        }
    }
}