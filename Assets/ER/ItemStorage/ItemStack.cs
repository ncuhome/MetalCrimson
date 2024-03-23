using ER.Resource;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace ER.ItemStorage
{
    public class ItemStack : IItemStack
    {
        private UID uuid;
        private IItemResource resource;
        private string displayName;//物品堆的显示名称
        private DescriptionInfo[] descriptions;//物品堆的描述信息
        private int amount;//当前堆叠数量
        private bool stackable;//物品堆是否可堆叠
        private int amountMax;//物品堆的堆叠上限
        private Dictionary<string, object> infos;


        public IItemResource Resource
        {
            get => resource;
            set
            {
                resource = value;
                displayName = resource.DisplayName;
                stackable = resource.Stackable;
                amountMax = resource.AmountMax;
                descriptions = resource.Descriptions;
            }
        }
        public int Amount { get => amount; set => amount = value; }
        public string DisplayName { get => displayName; set => displayName = value; }
        public DescriptionInfo[] Descriptions { get => descriptions; set => descriptions = value; }

        public bool Stackable { get => stackable; set => stackable = value; }

        public int AmountMax { get => amountMax; set => amountMax = value; }

        public UID UUID => uuid;

        public string ClassName => nameof(ItemStack);

        public Dictionary<string, object> Infos { get => infos; set => infos = value; }

        public void Deserialize(ObjectUIDInfo data)
        {
            uuid = new UID(data.uuid);
            if (data.data.TryGetValue("resource", out object registryName))
            {
                resource = GR.Get<IItemResource>((string)registryName);
            }
            if (data.data.TryGetValue("displayName", out object _displayName))
            {
                displayName = (string)_displayName;
            }
            if (data.data.TryGetValue("descriptions", out object _descriptions))
            {
                descriptions = _descriptions as DescriptionInfo[];
            }
            if (data.data.TryGetValue("amount", out object _amount))
            {
                amount = (int)_amount;
            }
            if (data.data.TryGetValue("stackable", out object _stackable))
            {
                stackable = (bool)_stackable;
            }
            if (data.data.TryGetValue("amountMax", out object _amountMax))
            {
                amountMax = (int)_amountMax;
            }

        }

        public ObjectUIDInfo Serialize()
        {
            ObjectUIDInfo data = new ObjectUIDInfo(uuid);
            data.data["resource"] = resource.RegistryName;//源物品资源的注册名
            data.data["displayName"] = displayName;
            data.data["descriptions"] = descriptions;
            data.data["amount"] = amount;
            data.data["stackable"] = stackable;
            data.data["amountMax"] = amountMax;
            return data;
        }

        public IItemStack Copy()
        {
            DescriptionInfo[] des = new DescriptionInfo[descriptions.Length];
            for (int i = 0; i < des.Length; i++)
            {
                des[i] = descriptions[i];
            }
            Dictionary<string, object> _infos = new Dictionary<string, object>();
            foreach (var info in infos)
            {
                _infos.Add(info.Key, info.Value);
            }
            return new ItemStack()
            {
                resource = resource,
                displayName = displayName,
                descriptions = des,
                amount = amount,
                stackable = stackable,
                infos = _infos
            };
        }

        public ItemStack()
        {
            uuid = new UID(ClassName, GetHashCode());
        }
    }
}