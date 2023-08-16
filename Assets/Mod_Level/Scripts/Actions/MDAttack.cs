using ER.Entity2D;
using System.Collections.Generic;

namespace Mod_Level
{
    public class MDAttack : MDAction
    {
        public List<ATActionRegion> regions;
        private ATCharacterState state;
        /// <summary>
        /// 是否处于攻击状态
        /// </summary>
        private bool attacking = false;
        public MDAttack() { actionName = "Attack"; }

        public override bool ActionJudge()
        {
            return true;
        }

        public override void Initialize()
        {
            state = manager.Owner.GetAttribute<ATCharacterState>();

            #region 上架势
            regions[0].time = 0.5f;
            regions[0].actor = manager.Owner;
            regions[0].actionName = "PostureUpAttack";
            regions[0].actionType = "Attack";
            regions[0].EndEvent += ()=>attacking = false;
            regions[0].infos["damage"] = 15;
            regions[0].Initialize();
            #endregion

            #region 前架势
            regions[1].time = 0.5f;
            regions[1].actor = manager.Owner;
            regions[1].actionName = "PostureFrontAttack";
            regions[1].actionType = "Attack";
            regions[1].EndEvent += () => attacking = false;
            regions[1].infos["damage"] = 10;
            regions[1].Initialize();
            #endregion

            #region 下架势
            regions[2].time = 0.5f;
            regions[2].actor = manager.Owner;
            regions[2].actionName = "PostureDownAttack";
            regions[2].actionType = "Attack";
            regions[2].EndEvent += () => attacking = false;
            regions[2].infos["damage"] = 10;
            regions[2].Initialize();
            #endregion
        }
        public override void StartAction()
        {
            if (attacking) return;//处于攻击状态不执行新的攻击
            switch (state.posture)
            {
                case ATCharacterState.Posture.Up:
                    regions[0].Reset();
                    attacking = true;
                    break;
                case ATCharacterState.Posture.Front:
                    regions[1].Reset();
                    attacking = true;
                    break;
                case ATCharacterState.Posture.Down:
                    regions[2].Reset();
                    attacking = true;
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