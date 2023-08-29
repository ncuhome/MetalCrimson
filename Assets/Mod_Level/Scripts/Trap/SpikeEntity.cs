using ER.Entity2D;
using UnityEngine;

public class SpikeEntity : Entity
{
    protected override void Initialized()
    {
        ATActionRegion region=GetAttribute<ATActionRegion>();
        Debug.Log($"尖刺初始化:{region != null}");
        region.time = -1;
        region.actor = this;
        //region.hits = -1;
        region.actionName = "SpikeAttack";
        region.actionType = "Attack";
        region.infos["damage"] = 15f;
        region.infos["repel_mode"] = ATRepel.RepelMode.AutoDirection;
        region.infos["repel_power"] = 25f;
        region.Initialize();
        region.Reset();
    }
}
