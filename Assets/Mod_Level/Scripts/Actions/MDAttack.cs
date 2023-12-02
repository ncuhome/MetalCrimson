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

        private ATPlayerState ownerState;

        private ATPlayerState.Posture posture;
        public MDAttack()
        { actionName = "Attack"; controlType = ControlType.Trigger; }

        public override bool ActionJudge()
        {
            if (state != ActionState.Disable)
            {
                //将此动作加入缓冲区
                manager.AddBufferAction(actionName);
                return false;//处于攻击状态不执行新的攻击
            }
            return true;
        }

        public override void Initialize()
        {
            ownerState = manager.Owner.GetAttribute<ATPlayerState>();
            region.actor = manager.Owner;
            region.actionName = "Attack";
            region.actionType = "Attack";
            region.Initialize();
        }

        protected override void StartAction(params string[] keys)
        {
            manager.CloseMixedLayer((int)AnimationLayer.Move);
            state = ActionState.Waiting;
            posture = ownerState.ActPosture;

        }

        protected override void StopAction(params string[] keys)
        {
            manager.OpenMixedLayer((int)AnimationLayer.Move);
            state = ActionState.Disable;
            Debug.Log("攻击动作完毕");
        }

        private void FuncAttacking()
        {
            if (!ownerState.ControlAct) return;
            ownerState.ControlDir = false;
            state = ActionState.Acting;
            Debug.Log("攻击效果出现!");

            region.infos["damage"] = 15f;
            region.infos["repel_mode"] = ATRepel.RepelMode.CustomDirection;
            Vector2 repel_dir = Vector2.zero;
            switch (ownerState.direction)
            {
                case ATCharacterState.Direction.Left:
                    repel_dir += Vector2.left;
                    break;
                case ATCharacterState.Direction.Right:
                    repel_dir += Vector2.right;
                    break;

            }
            switch (posture)
            {
                case ATPlayerState.Posture.Up:
                    repel_dir += Vector2.down;
                    region.infos["repel_power"] = 20f;
                    region.time = 1;
                    region.hits = 1;
                    region.hitCD = 0.2f;
                    AddRigidBuff(0.3f);
                    break;
                case ATPlayerState.Posture.Front:
                    region.infos["repel_power"] = 15f;
                    region.time = 1;
                    region.hits = 1;
                    region.hitCD = 0.2f;
                    AddRigidBuff(0.3f);
                    break;
                case ATPlayerState.Posture.Down:
                    repel_dir += Vector2.up;
                    region.infos["repel_power"] = 30f;
                    region.time = 1;
                    region.hits = 1;
                    region.hitCD = 0.2f;
                    AddRigidBuff(0.5f);
                    break;
            }
            Debug.Log($"击退方向:{repel_dir}");
            region.infos["repel_dir"] = repel_dir;
            region.Reset();
        }

        private void AddRigidBuff(float time)
        {
            if (region.infos.ContainsKey("buff_count"))
            {
                region.infos["buff_count"] = (int)region.infos["buff_count"] + 1;
                List<BuffSetInfo> bifs = (List<BuffSetInfo>)region.infos["buff_ads"];
                bifs.Add(new BuffSetInfo()
                {
                 buffName = "Rigidity",
                  defTime = time,
                   level = 1,
                    infos = null
                });
            }
            else
            {
                region.infos["buff_count"] = 1;
                List<BuffSetInfo> bifs = new List<BuffSetInfo>();
                bifs.Add(new BuffSetInfo()
                {
                    buffName = "Rigidity",
                    defTime = time,
                    level = 1,
                    infos = null
                });
                region.infos["buff_ads"] = bifs;
            }
        }

        private void FuncStoped()
        {
            if (!ownerState.ControlAct) return;
            ownerState.ControlDir = true;
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
            Debug.Log("攻击动作被中断!");
            manager.OpenMixedLayer((int)AnimationLayer.Move);
            state = ActionState.Disable;
            Debug.Log($"State:{state}");
            ownerState.ControlDir = true;
        }
    }
}