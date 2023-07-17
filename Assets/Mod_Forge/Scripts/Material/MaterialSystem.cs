using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ER.Items
{
    public class MaterialSystem : MonoBehaviour
    {
        public Transform materialsParentTrans = null;
        public GameObject materialPrefab = null;
        private ItemStore materialsItemStore = null;
        private GameObject[] materials = null;
        // Start is called before the first frame update
        void Start()
        {
            ItemTemplateStore.Instance.LoadItemsList("Assets/SteamingAssets/材料信息表.csv");
            ItemStoreManager.Instance.Creat("materialItemStore");
            materialsItemStore = ItemStoreManager.Instance.Stores["materialItemStore"];
            materials = new GameObject[64];
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
                    materialsItemStore[i].CreatAttribute("Count", materialsItemStore[i].GetInt("Count") + 1);
                    return;
                }
            }

            materialsItemStore.AddItem(new ItemVariable(ItemTemplateStore.Instance[NameTmp]));
            ItemVariable newMaterial = materialsItemStore[materialsItemStore.Count - 1];

            newMaterial.CreatAttribute("NameTmp", newMaterial.GetText("NameTmp"));
            newMaterial.CreatAttribute("Name", newMaterial.GetText("Name"));
            newMaterial.CreatAttribute("Tags", newMaterial.GetText("Tags"));
            newMaterial.CreatAttribute("Density", newMaterial.GetInt("Density"));
            newMaterial.CreatAttribute("Flexability", newMaterial.GetInt("Flexability"));
            newMaterial.CreatAttribute("Toughness", newMaterial.GetInt("Toughness"));
            newMaterial.CreatAttribute("AntiSolution", newMaterial.GetInt("AntiSolution"));
            newMaterial.CreatAttribute("HeatPassage", newMaterial.GetInt("HeatPassage"));
            newMaterial.CreatAttribute("HeatContain", newMaterial.GetInt("HeatContain"));
            newMaterial.CreatAttribute("Pretty", newMaterial.GetInt("Pretty"));
            newMaterial.CreatAttribute("ForgeTemp", newMaterial.GetInt("ForgeTemp"));
            newMaterial.CreatAttribute("MeltTemp", newMaterial.GetInt("MeltTemp"));
            newMaterial.CreatAttribute("HeatPreference", newMaterial.GetFloat("HeatPreference"));
            newMaterial.CreatAttribute("Pressability", newMaterial.GetFloat("Pressability"));
            newMaterial.CreatAttribute("Stubborn", newMaterial.GetFloat("Stubborn"));
            newMaterial.CreatAttribute("AtsGrowth", newMaterial.GetFloat("AtsGrowth"));

            newMaterial.CreatAttribute("Count", 1);
            newMaterial.CreatAttribute("IsForged", false);

            materials[materialsItemStore.Count - 1] = Instantiate(materialPrefab);
            materials[materialsItemStore.Count - 1].transform.SetParent(materialsParentTrans);
        }
    }
}
