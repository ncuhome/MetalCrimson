// Ignore Spelling: Atk

using ER.Entity2D;
using UnityEngine;

namespace Mod_Level
{
    public class MDAttack : MDAction
    {
        //public List<ATActionRegion> regions;
        [SerializeField]
        [Tooltip("武器的伤害区域")]
        private ATActionRegion region;

        private ATCharacterState ownerState;

        public MDAttack()
        { actionName = "Attack"; controlType = ControlType.Trigger; }

        public override bool ActionJudge()
        {
            return true;
        }

        public override void Initialize()
        {
            ownerState = manager.Owner.GetAttribute<ATCharacterState>();
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
            if (state != ActionState.Disable) return;//处于攻击状态不执行新的攻击
            manager.CloseMixedLayer((int)AnimationLayer.Move);
            state = ActionState.Waiting;
        }

        protected override void StopAction(params string[] keys)
        {
            Debug.Log("退出攻击");
            manager.OpenMixedLayer((int)AnimationLayer.Move);
            state = ActionState.Disable;
        }

        private void FuncAttacking()
        {
            state = ActionState.Acting;
            region.Reset();
        }

        private void FuncStoped()
        {
            state = ActionState.Stoped;
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
            state = ActionState.Disable;
        }
    }
}