using ER.Entity2D;
using System.Collections.Generic;
using UnityEngine;

public class SpikeEntity : Entity
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
    protected override void Initialized()
    {
        ATActionRegion region = GetAttribute<ATActionRegion>();
        Debug.Log($"尖刺初始化:{region != null}");
        region.time = -1;
        region.actor = this;
        region.hits = -1;
        region.actionName = "SpikeAttack";
        region.actionType = "Attack";
        region.infos["damage"] = damage;
        region.infos["repel_mode"] = ATRepel.RepelMode.AutoDirection;
        region.infos["repel_power"] = repel_pwoer;
        AddRigidBuff(region, rigid_time);
        region.Initialize();
        region.Reset();
    }

    private void AddRigidBuff(ATActionRegion region ,float time)
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

}