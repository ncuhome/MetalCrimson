using ER;
using ER.Entity2D;
using System;
using UnityEngine;

namespace Mod_Level.Attributes
{
    /// <summary>
    /// 可被摧毁的
    /// 依赖 ATActionResponse
    /// </summary>
    public class ATCanDestroyed : MonoAttribute
    {
        [Tooltip("生命值")]
        [SerializeField]
        private float HP = 1f;
        [Tooltip("是否开启摧毁 - 如果关闭物体会以隐藏代替摧毁")]
        [SerializeField]
        private bool destroy = false;
        [Tooltip("复活时间 - 小于零则表示不复活(该效果作用前提是 destroy = false)")]
        [SerializeField]
        private float revive_time = -1;
        /// <summary>
        /// 物体被摧毁(隐藏)时触发的事件
        /// </summary>
        public event Action OnDestroyEvent;

        private bool destroyed = false;
        /// <summary>
        /// 是否已经被销毁
        /// </summary>
        public bool Destroyed =>destroyed;

        public ATCanDestroyed()
        {
            AttributeName = nameof(ATCanDestroyed);
        }
        public override void Initialize()
        {
            ATActionResponse response = owner.GetAttribute<ATActionResponse>();
            response.ActionEvent += delegate (ActionInfo info)
            {
                if (info.type == "Attack")
                {
                    if (info.infos.TryGetValue("damage", out object value))
                    {
                        HP -= (float)value;
                        if (HP <= 0)
                        {
                            DestroySelf();
                        }
                    }
                }
            };
        }
        /// <summary>
        /// 重生
        /// </summary>
        private void Revive()
        {
            Debug.Log("方块重生");
            gameObject.SetActive(true);
            destroyed = false;
            HP = 1f;
        }
        public void DestroySelf()
        {
            OnDestroyEvent?.Invoke();
            destroyed = true;
            if (destroy)
            {
                Destroy(gameObject);
              
            }
            else
            {
                gameObject.SetActive(false);
                if (revive_time > 0)
                {
                    TimerManager.Instance.RegisterTimer(ERTimer.GetBriefTimer("candestoryed" + ERinbone.RandomNumber(), Revive, revive_time));
                }
            }
        }
    }
}