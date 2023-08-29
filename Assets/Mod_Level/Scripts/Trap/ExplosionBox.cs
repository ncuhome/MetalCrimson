using UnityEngine;

namespace ER.Entity2D
{
    public class ExplosionBox : Entity
    {
        [SerializeField]
        private float HP = 1;

        [SerializeField]
        private bool catchBoom = false;

        [SerializeField]
        private float damage = 1f;//伤害值

        [SerializeField]
        private float repel_power = 50f;//击退力度

        [SerializeField]
        private float leaveTime = 1f;//存活时间

        private ATActionRegion action;

        protected override void Initialized()
        {
            GetAttribute<ATActionResponse>().ActionEvent += delegate (ActionInfo info)
            {
                if (info.type == "Attack")
                {
                    Debug.Log($"爆炸箱子收到攻击:{info.infos["damage"]}");
                    if (info.infos.TryGetValue("damage", out object value))
                    {
                        HP -= (float)value;
                        if (HP <= 0)
                        {
                            Boom();
                        }
                    }
                }
            };
            if (catchBoom)
            {
                GetAttribute<ATRegion>().EnterEvent += delegate (Collider2D c)
                {
                    if (c.GetComponent<Entity>() != null)
                    {
                        Boom();
                    }
                };
            }
            action = GetAttribute<ATActionRegion>();
            action.time = -1;
            //action.hits = -1;
            action.actionName = "Explosion";
            action.actionType = "Attack";
            action.actor = this;
            action.infos["damage"] = damage;
            action.infos["repel_mode"] = ATRepel.RepelMode.AutoDirection;
            action.infos["repel_power"] = repel_power;
        }

        private void Boom()
        {
            action.Reset();
            Invoke("Destroy", leaveTime);
        }

        private void Destroy()
        {
            Destroy(gameObject);
        }
    }
}