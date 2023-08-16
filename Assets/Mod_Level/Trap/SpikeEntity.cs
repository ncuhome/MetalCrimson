using ER.Entity2D;
using UnityEngine;

public class SpikeEntity : Entity
{
    protected override void Initialized()
    {
        ATActionRegion region=GetAttribute<ATActionRegion>();
        region.time = -1;
        region.actor = this;
        region.actionName = "SpikeAttack";
        region.actionType = "Attack";
        region.infos["damage"] = 15f;
        region.infos["repel"] = 10f;
        region.Initialize();
    }
}
