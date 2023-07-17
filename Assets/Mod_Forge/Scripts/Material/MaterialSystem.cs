using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ER.Items
{
    public class MaterialSystem : MonoBehaviour
    {
        /// <summary>
        /// 构建单例模式
        /// </summary>
        private static MaterialSystem instance;

        public static MaterialSystem Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MaterialSystem();
                }
                return instance;
            }
        }
        /// <summary>
        /// 材料父物体Transform组件
        /// </summary>
        public Transform materialsParentTrans = null;
        /// <summary>
        /// 材料预制件
        /// </summary>
        public GameObject materialPrefab = null;
        /// <summary>
        /// 材料物品库
        /// </summary>
        public ItemStore materialsItemStore = null;
        /// <summary>
        /// 材料物体组
        /// </summary>
        private GameObject[] materials = null;
        // Start is called before the first frame update
        void Start()
        {
            InitMaterialItemStore();
        }

        // Update is called once per frame
        void Update()
        {

        }
        /// <summary>
        /// 初始化材料物品库设置
        /// </summary>
        private void InitMaterialItemStore()
        {
            ItemTemplateStore.Instance.LoadItemsList(@"Assets/StreamingAssets/材料信息表.csv");
            ItemStoreManager.Instance.Creat("materialItemStore");
            materialsItemStore = ItemStoreManager.Instance.Stores["materialItemStore"];
            materials = new GameObject[64];

            for (int i = 0; i < 6; i++)
            {
                AddNormalMaterial("RawIron");
            }
        }
        /// <summary>
        /// 通过NameTmp添加原料,添加成功返回true，否则返回false
        /// </summary>
        public bool AddNormalMaterial(string NameTmp)
        {
            ItemTemplateStore.Instance.LoadItemsList(@"Assets/StreamingAssets/材料信息表.csv");

            if (ItemTemplateStore.Instance[NameTmp] == null) { return false; } // 如果没找到返回false

            // 如果已经在库中，则数量+1
            for (int i = 0; i < materialsItemStore.Count; i++)
            {
                if (materialsItemStore[i].GetBool("IsForged"))
                {
                    continue;
                }
                if (materialsItemStore[i].GetText("NameTmp") == NameTmp)
                {
                    materialsItemStore[i].CreateAttribute("Num", materialsItemStore[i].GetInt("Num") + 1);
                    return true;
                }
            }

            //创建新的物品，并初始化
            materialsItemStore.AddItem(new ItemVariable(ItemTemplateStore.Instance[NameTmp], true));
            ItemVariable newMaterialItem = materialsItemStore[materialsItemStore.Count - 1];

            newMaterialItem.CreateAttribute("Num", 1);
            newMaterialItem.CreateAttribute("IsForged", false);
            newMaterialItem.CreateAttribute("Name", newMaterialItem.GetText("Name", false));

            GameObject newMaterialObject = materials[materialsItemStore.Count - 1];
            newMaterialObject = Instantiate(materialPrefab);
            newMaterialObject.transform.SetParent(materialsParentTrans);

            MaterialScript newMaterialScript = newMaterialObject.GetComponent<MaterialScript>();
            newMaterialScript.MaterialItem = newMaterialItem;

            return true;
        }
        /// <summary>
        /// 通过ID添加原料,添加成功返回true，否则返回false
        /// </summary>
        public bool AddNormalMaterial(int id)
        {
            ItemTemplateStore.Instance.LoadItemsList(@"Assets/StreamingAssets/材料信息表.csv");

            if (ItemTemplateStore.Instance[id] == null) { return false; }// 如果没找到返回false

            // 如果已经在库中，则数量+1
            for (int i = 0; i < materialsItemStore.Count; i++)
            {
                if (materialsItemStore[i].GetBool("IsForged"))
                {
                    continue;
                }
                if (materialsItemStore[i].GetInt("ID") == id)
                {
                    materialsItemStore[i].CreateAttribute("Num", materialsItemStore[i].GetInt("Num") + 1);
                    return true;
                }
            }

            //创建新的物品，并初始化
            materialsItemStore.AddItem(new ItemVariable(ItemTemplateStore.Instance[id], true));
            ItemVariable newMaterialItem = materialsItemStore[materialsItemStore.Count - 1];

            newMaterialItem.CreateAttribute("Num", 1);
            newMaterialItem.CreateAttribute("IsForged", false);
            newMaterialItem.CreateAttribute("Name", newMaterialItem.GetText("Name", false));

            GameObject newMaterialObject = materials[materialsItemStore.Count - 1];
            newMaterialObject = Instantiate(materialPrefab);
            newMaterialObject.transform.SetParent(materialsParentTrans);

            MaterialScript newMaterialScript = newMaterialObject.GetComponent<MaterialScript>();
            newMaterialScript.MaterialItem = newMaterialItem;
            return true;
        }
    }
}
