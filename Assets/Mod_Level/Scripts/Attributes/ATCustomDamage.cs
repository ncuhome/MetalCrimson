using ER.Entity2D;
using System.Collections.Generic;
using UnityEngine;

namespace Mod_Level.Attributes
{
    /// <summary>
    /// 基本的可定义伤害方块属性(接触伤害
    /// 依赖: ATActionRegion
    /// </summary>
    public class ATCustomDamage:MonoAttribute
    {
        [Tooltip("伤害")]
        [SerializeField]
        private float damage = 5f;
        [Tooltip("击退力")]
        [SerializeField]
        private float repel_pwoer = 5f;
        [Tooltip("造成僵直时间")]
        [SerializeField]
        private float rigid_time = 0.25f;

        private ATActionRegion region;
        public ATActionRegion Region=>region;
        public ATCustomDamage()
        {
            AttributeName = nameof(ATCustomDamage);
        }

        public override void Initialize()
        {
            region =owner.GetAttribute<ATActionRegion>();
            region.infos["damage"] = damage;
            region.infos["repel_mode"] = ATRepel.RepelMode.AutoDirection;
            region.infos["repel_power"] = repel_pwoer;
            AddRigidBuff(region, rigid_time);
        }
        public static void AddRigidBuff(ATActionRegion region, float time)
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
        public void SetDamageInfo(float _damage, float _repel_power, float _rigid_time)
        {
            damage= _damage;
            repel_pwoer= _repel_power;
            rigid_time= _rigid_time;
            region.infos["damage"] = damage;
            region.infos["repel_mode"] = ATRepel.RepelMode.AutoDirection;
            region.infos["repel_power"] = repel_pwoer;
            AddRigidBuff(region, rigid_time);
        }
        /// <summary>
        /// 设置伤害半径(仅为圆形Collider时有效)
        /// </summary>
        /// <param name=""></param>
        public void SetRange(float radius)
        {
            CircleCollider2D collider = region.GetComponent<CircleCollider2D>();
            collider.radius = radius;   
        }
        [ContextMenu("重设属性")]
        private void ResetStatus()
        {
            region.infos["damage"] = damage;
            region.infos["repel_mode"] = ATRepel.RepelMode.AutoDirection;
            region.infos["repel_power"] = repel_pwoer;
            AddRigidBuff(region, rigid_time);
        }
    }
}