using ER;
using ER.Entity2D;
using ER.Items;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mod_Level.Attributes
{
    /// <summary>
    /// 可收集物品信息
    /// </summary>
    [Serializable]
    public class CollectableInfo
    {
        [Tooltip("物品模板名称")]
        public string ItemName;
        [Tooltip("掉落概率")]
        public float probability;
    }
    /// <summary>
    /// 死亡时掉落物品:
    /// 依赖: ATCanDestroyed
    /// </summary>
    public class ATOnDeadDroppedItems : MonoAttribute
    {
        [Tooltip("物品掉落配置")]
        public List<CollectableInfo> infos = new List<CollectableInfo>();

        private ATCanDestroyed canDestroyed;
        public ATCanDestroyed CanDestroyed => canDestroyed;
        public ATOnDeadDroppedItems()
        {
            AttributeName = nameof(ATOnDeadDroppedItems);
        }
        public override void Initialize()
        {
            canDestroyed = owner.GetAttribute<ATCanDestroyed>();
            canDestroyed.OnDestroyEvent += DropItems;

        }

        private void DropItems()
        {
            System.Random random = new System.Random((int)(Time.time * 1000));
            for (int i = 0; i < infos.Count; i++)
            {

                //如果概率指针在 掉落概率外(表明判定失败) 则不生成掉落物
                if (random.NextDouble() >= infos[i].probability) continue;

                CollectibleItem Item = ObjectPoolManager.Instance.GetObject("CollectibleItem") as CollectibleItem;
                if (Item == null)
                {
                    Debug.LogError("对象池获取掉落物品失败");
                    return;
                }
                Item.UpdateItemInfo(TemplateStoreManager.Instance["Item"][infos[i].ItemName]);

            }
        }
    }
}