using ER.Entity2D;
using ER.Items;
using UnityEngine;

namespace Mod_Level
{
    /// <summary>
    /// 物品收集光环
    /// </summary>
    public class ATCollect : MonoAttribute
    {
        #region 初始化

        public ATCollect()
        { AttributeName = nameof(ATCollect); }

        public override void Initialize()
        {
            collectRegion.EnterEvent += Collect;
            getRegion.EnterEvent += Get;
            itemStore = ItemStoreManager.Instance.Create(storeName);
        }

        #endregion 初始化

        [SerializeField]
        [Tooltip("收集光环")]
        private ATRegion collectRegion;

        [SerializeField]
        [Tooltip("获取光环")]
        private ATRegion getRegion;

        /// <summary>
        /// 背包
        /// </summary>
        private ItemStore itemStore;

        [SerializeField]
        [Tooltip("动态背包名称")]
        private string storeName;

        private void Collect(Collider2D c)
        {
            //Debug.Log("吸引物品");
            CollectibleItem item = c.GetComponent<CollectibleItem>();
            if (item != null)
            {
                item.Follow(transform, 10);
            }
        }

        private void Get(Collider2D c)
        {
            //Debug.Log("拾取物品");
            CollectibleItem item = c.GetComponent<CollectibleItem>();
            if (item != null)
            {
                if (item.Item != null)
                {
                    ItemVariable iv = item.Item as ItemVariable;
                    if (iv != null)
                    {
                        itemStore.AddItem(iv);
                    }
                    else
                    {
                        itemStore.AddItem(new ItemVariable(iv.StoreName, iv.ID));
                    }
                }
            }
            item.Destroy();
        }
    }
}