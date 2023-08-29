// Ignore Spelling: Atk

using ER.Entity2D;
using System.Collections.Generic;
using UnityEngine;

namespace Mod_Level
{
    public class MDAttack : MDAction
    {
        //public List<ATActionRegion> regions;
        [SerializeField]
        [Tooltip("武器的伤害区域")]
        private ATActionRegion region;
        private ATCharacterState state;

        public enum AttackState
        { Waiting, Attacking, Stoped, Disable }

        private AttackState atstate = AttackState.Disable;
        public AttackState AtkState { get => atstate; }

        public MDAttack()
        { actionName = "Attack"; controlType = ControlType.Trigger; }

        public override bool ActionJudge()
        {
            return true;
        }

        public override void Initialize()
        {
            state = manager.Owner.GetAttribute<ATCharacterState>();
            region.time = 1;
            region.hits = 1;
            region.actor = manager.Owner;
            region.actionName = "Attack";
            region.actionType = "Attack";
            region.infos["damage"] = 15f;
            region.infos["repel_mode"] = ATRepel.RepelMode.AutoDirection;
            region.infos["repel_power"] = 10f;
            region.Initialize();
            
        }

        protected override void StartAction(params string[] keys)
        {
            if (atstate != AttackState.Disable) return;//处于攻击状态不执行新的攻击
            manager.CloseMixedLayer((int)AnimationLayer.Move);
            atstate = AttackState.Waiting;
        }

        protected override void StopAction(params string[] keys)
        {
            Debug.Log("退出攻击");
            manager.OpenMixedLayer((int)AnimationLayer.Move);
            atstate = AttackState.Disable;
        }

        private void FuncAttacking()
        {
            atstate = AttackState.Attacking;
            region.Reset();
           
        }

        private void FuncStoped()
        {
            atstate = AttackState.Stoped;
            region.gameObject.SetActive(false);
            
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

        protected override void BreakAction(params string[] keys)
        {
            manager.OpenMixedLayer((int)AnimationLayer.Move);
            atstate = AttackState.Disable;
        }
    }
}