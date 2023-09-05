using ER;
using ER.Entity2D;
using ER.Items;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Mod_Level
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

    public class Box: Entity
    {
        [SerializeField]
        private float HP = 1;
        [Tooltip("物品掉落配置")]
        public List<CollectableInfo> infos = new List<CollectableInfo>();

        protected override void Initialized()
        {
            GetAttribute<ATActionResponse>().ActionEvent += delegate (ActionInfo info)
            {
                if (info.type == "Attack")
                {
                    if (info.infos.TryGetValue("damage", out object value))
                    {
                        HP -= (float)value;
                        if (HP <= 0)
                        {
                            Destroy();
                        }
                    }
                }
            };
        }

        private void Destroy()
        {
            Random random = new Random((int)(Time.time * 1000));
            for (int i=0;i<infos.Count;i++)
            {

                //如果概率指针在 掉落概率外(表明判定失败) 则不生成掉落物
                if (random.NextDouble() >= infos[i].probability) continue;

                CollectibleItem Item = ObjectPoolManager.Instance.GetObject("CollectibleItem") as CollectibleItem;
                if(Item == null)
                {
                    Debug.LogError("对象池获取掉落物品失败");
                    return;
                }
                Item.UpdateItemInfo(TemplateStoreManager.Instance["Item"][infos[i].ItemName]);
                
            }
            
            Destroy(gameObject);
        }
    }
}