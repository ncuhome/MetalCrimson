// Ignore Spelling: Atk

using ER.Entity2D;
using System.Collections.Generic;
using UnityEngine;

namespace Mod_Level
{
    public class MDAttack : MDAction
    {
        public List<ATActionRegion> regions;
        private ATCharacterState state;

        public enum AttackState
        { Waiting, Attacking, Stoped, Disable }

        private AttackState atstate = AttackState.Disable;
        public AttackState AtkState { get => atstate; }

        public MDAttack()
        { actionName = "Attack"; layer = 0; }

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
            regions[0].infos["damage"] = 15f;
            regions[0].Initialize();

            #endregion 上架势

            #region 前架势

            regions[1].time = 0.5f;
            regions[1].actor = manager.Owner;
            regions[1].actionName = "PostureFrontAttack";
            regions[1].actionType = "Attack";
            regions[1].infos["damage"] = 10f;
            regions[1].Initialize();

            #endregion 前架势

            #region 下架势

            regions[2].time = 0.5f;
            regions[2].actor = manager.Owner;
            regions[2].actionName = "PostureDownAttack";
            regions[2].actionType = "Attack";
            regions[2].infos["damage"] = 10f;
            regions[2].Initialize();

            #endregion 下架势
        }

        protected override void StartAction(params string[] keys)
        {
            if (atstate != AttackState.Disable) return;//处于攻击状态不执行新的攻击
            manager.CloseMixedLayer("Move");
            atstate = AttackState.Waiting;
        }

        protected override void StopAction(params string[] keys)
        {
            Debug.Log("退出攻击");
            manager.OpenMixedLayer("Move");
            atstate = AttackState.Disable;
        }

        private void FuncAttacking()
        {
            atstate = AttackState.Attacking;
            switch (state.ActPosture)
            {
                case ATCharacterState.Posture.Up:
                    regions[0].Reset();
                    break;

                case ATCharacterState.Posture.Front:
                    regions[1].Reset();
                    break;

                case ATCharacterState.Posture.Down:
                    regions[2].Reset();
                    break;

                default:
                    break;
            }
        }

        private void FuncStoped()
        {
            atstate = AttackState.Stoped;
            switch (state.ActPosture)
            {
                case ATCharacterState.Posture.Up:
                    regions[0].gameObject.SetActive(false);
                    break;

                case ATCharacterState.Posture.Front:
                    regions[1].gameObject.SetActive(false);
                    break;

                case ATCharacterState.Posture.Down:
                    regions[2].gameObject.SetActive(false);
                    break;

                default:
                    break;
            }
        }

        public override void ActionFunction(string key)
        {
            switch (key)
            {
                case "Attack":
                    FuncAttacking();
                    break;

                case "Stop":
                    FuncStoped();
                    break;

                default:
                    Debug.Log("未知参数");
                    break;
            }
        }
    }
}