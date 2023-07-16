using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ER.Items
{
    public class MaterialSystem : MonoBehaviour
    {
        public ItemStore materialItemStore;
        // Start is called before the first frame update
        void Start()
        {
            ItemTemplateStore.Instance.LoadItemsList("Assets/SteamingAssets/材料信息表.csv");
            materialItemStore = new ItemStore();
            materialItemStore.storeName = "materialItemStore";
            
            materialItemStore.AddItem(new ItemVariable(ItemTemplateStore.Instance["RawIron"]));
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
