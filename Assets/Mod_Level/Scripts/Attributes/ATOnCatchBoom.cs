using ER;
using ER.Entity2D;
using UnityEngine;

namespace Mod_Level.Attributes
{
    public class ATOnCatchBoom : MonoAttribute
    {
        [Tooltip("伤害值")]
        [SerializeField]
        private float damage = 1f;//伤害值

        [Tooltip("击退力度")]
        [SerializeField]
        private float repel_power = 50f;//击退力度

        [Tooltip("造成僵直时间")]
        [SerializeField]
        private float rigid_time = 1f;

        [Tooltip("爆炸持续时间")]
        [SerializeField]
        private float leaveTime = 1f;//存活时间
        [Tooltip("爆炸半径")]
        [SerializeField]
        private float radius = 5f;

        private ATRegion region;

        private bool boomed = false;
        public ATRegion Region => region;
        public ATOnCatchBoom()
        {
            AttributeName = nameof(ATOnCatchBoom);
        }
        public override void Initialize()
        {
            region = owner.GetAttribute<ATRegion>();
            region.EnterEvent += Boom;
        }
        public void Boom(Collider2D c)
        {
            if (boomed) return;
            boomed = true;
            GameObject obj = PrefabManager.Instance["DamageObject"];
            obj.transform.position = transform.position;
            ATCustomDamage dm = obj.GetComponent<ATCustomDamage>();
            dm.SetDamageInfo(damage, repel_power, rigid_time);
            dm.SetRange(radius);
            dm.Region.time = leaveTime;
        }
    }
}