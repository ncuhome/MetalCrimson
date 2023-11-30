// Ignore Spelling: collider

using ER.Entity2D;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mod_Level
{
    /// <summary>
    /// 视野区域, 如果视野中有敌人则进入战斗状态
    /// </summary>
    public class ATEye : ATRegion
    {
        private ATCharacterState state;

        [SerializeField]
        protected List<Entity> record = new List<Entity>();

        public ATEye()
        { AttributeName = nameof(ATEye); }

        public override void Initialize()
        {
            state = owner.GetAttribute<ATCharacterState>();
        }

        #region 事件

        /// <summary>
        /// 当目下存在实体时触发的事件
        /// </summary>
        public event Action eyeEvent;

        /// <summary>
        /// 当目下实体个数发生改变时触发的事件(新加入/新移除的实体对象)
        /// </summary>
        public event Action<Entity> modifyEyeEvent;

        /// <summary>
        /// 当目下没有实体时触发的事件
        /// </summary>
        public event Action notEyeEvent;

        #endregion 事件

        protected bool Judge(Collider2D collider, out Entity entity)
        {
            //如果是 Entity 则返回 true
            entity = collider.GetComponent<Entity>();
            return base.Judge(collider);
        }

        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            if (Judge(collision.collider, out Entity entity))
            {
                EnterAction(collision.collider);
                VirtualEnter(collision.collider);

                if (entity == null) return;
                if (!record.Contains(entity))
                {
                    if (record.Count == 0)
                        eyeEvent?.Invoke();
                    record.Add(entity);
                    modifyEyeEvent?.Invoke(entity);
                }
            }
        }

        protected override void OnCollisionStay2D(Collision2D collision)
        {
            if (Judge(collision.collider, out Entity entity))
            {
                StayAction(collision.collider);
                VirtualStay(collision.collider);

                if (entity == null) return;
                if (!record.Contains(entity))
                {
                    if (record.Count == 0)
                        eyeEvent?.Invoke();
                    record.Add(entity);
                    modifyEyeEvent?.Invoke(entity);
                }
            }
        }

        protected override void OnCollisionExit2D(Collision2D collision)
        {
            if (Judge(collision.collider, out Entity entity))
            {
                ExitAction(collision.collider);
                VirtualExit(collision.collider);

                if (entity == null) return;
                if (record.Contains(entity))
                {
                    record.Remove(entity);
                    if (record.Count == 0)
                        notEyeEvent?.Invoke();
                    modifyEyeEvent?.Invoke(entity);
                }
            }
        }

        protected override void OnTriggerEnter2D(Collider2D collider)
        {
            Debug.Log("collider接触" + collider.gameObject.tag);
            if (Judge(collider, out Entity entity))
            {
                EnterAction(collider);
                VirtualEnter(collider);

                if (entity == null) return;
                if (!record.Contains(entity))
                {
                    if (record.Count == 0)
                        eyeEvent?.Invoke();
                    record.Add(entity);
                    modifyEyeEvent?.Invoke(entity);
                }
            }
        }

        protected override void OnTriggerStay2D(Collider2D collider)
        {
            if (Judge(collider, out Entity entity))
            {
                StayAction(collider);
                VirtualStay(collider);

                if (entity == null) return;
                if (!record.Contains(entity))
                {
                    if (record.Count == 0)
                        eyeEvent?.Invoke();
                    record.Add(entity);
                    modifyEyeEvent?.Invoke(entity);
                }
            }
        }

        protected override void OnTriggerExit2D(Collider2D collider)
        {
            if (Judge(collider, out Entity entity))
            {
                ExitAction(collider);
                VirtualExit(collider);

                if (entity == null) return;
                if (record.Contains(entity))
                {
                    record.Remove(entity);
                    if (record.Count == 0)
                        notEyeEvent?.Invoke();
                    modifyEyeEvent?.Invoke(entity);
                }
            }
        }
    }
}