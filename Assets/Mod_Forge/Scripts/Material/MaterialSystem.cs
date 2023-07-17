using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ER.Items
{
    public class MaterialSystem : MonoBehaviour
    {
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

        public Transform materialsParentTrans = null;
        public GameObject materialPrefab = null;
        public ItemStore materialsItemStore = null;
        private GameObject[] materials = null;
        // Start is called before the first frame update
        void Start()
        {
            ItemTemplateStore.Instance.LoadItemsList(@"Assets/StreamingAssets/材料信息表.csv");
            ItemStoreManager.Instance.Creat("materialItemStore");
            materialsItemStore = ItemStoreManager.Instance.Stores["materialItemStore"];
            materials = new GameObject[64];

            AddNormalMaterial("RawIron");
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void AddNormalMaterial(string NameTmp)
        {
            for (int i = 0; i < materialsItemStore.Count; i++)
            {
                if (materialsItemStore[i].GetBool("IsForged"))
                {
                    continue;
                }
                if (materialsItemStore[i].GetText("NameTmp") == NameTmp)
                {
                    materialsItemStore[i].CreateAttribute("Num", materialsItemStore[i].GetInt("Num") + 1);
                    return;
                }
            }

            materialsItemStore.AddItem(new ItemVariable(ItemTemplateStore.Instance[NameTmp], true));
            ItemVariable newMaterialItem = materialsItemStore[materialsItemStore.Count - 1];

            newMaterialItem.CreateAttribute("Num", 1);
            newMaterialItem.CreateAttribute("IsForged", false);

            GameObject newMaterialObject = materials[materialsItemStore.Count - 1];
            newMaterialObject = Instantiate(materialPrefab);
            newMaterialObject.transform.SetParent(materialsParentTrans);

            MaterialScript newMaterialScript = newMaterialObject.GetComponent<MaterialScript>();
            newMaterialScript.MaterialItem = newMaterialItem;
        }

        public void AddNormalMaterial(int id)
        {
            for (int i = 0; i < materialsItemStore.Count; i++)
            {
                if (materialsItemStore[i].GetBool("IsForged"))
                {
                    continue;
                }
                if (materialsItemStore[i].GetInt("ID") == id)
                {
                    materialsItemStore[i].CreateAttribute("Num", materialsItemStore[i].GetInt("Num") + 1);
                    return;
                }
            }

            materialsItemStore.AddItem(new ItemVariable(ItemTemplateStore.Instance[id], true));
            ItemVariable newMaterialItem = materialsItemStore[materialsItemStore.Count - 1];

            newMaterialItem.CreateAttribute("Num", 1);
            newMaterialItem.CreateAttribute("IsForged", false);

            GameObject newMaterialObject = materials[materialsItemStore.Count - 1];
            newMaterialObject = Instantiate(materialPrefab);
            newMaterialObject.transform.SetParent(materialsParentTrans);

            MaterialScript newMaterialScript = newMaterialObject.GetComponent<MaterialScript>();
            newMaterialScript.MaterialItem = newMaterialItem;
        }
    }
}
