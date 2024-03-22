using ER;
using ER.ItemStorage;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Mod_Forge
{
    public class Forge:Singleton<Forge>
    {
        private static string savePath;

        private Dictionary<string, int> raw_materials;//原材料
        private ItemContainer materials;//加工材料
        private ItemContainer components;//部件

        /// <summary>
        /// 原材料库
        /// </summary>
        public Dictionary<string,int> RawMaterials { get => raw_materials; }
        /// <summary>
        /// 材料库
        /// </summary>
        public ItemContainer Materials { get => materials; }
        /// <summary>
        /// 组件库
        /// </summary>
        public ItemContainer Components{ get=> components; }
     
        public void Load()
        {
            string str = File.ReadAllText(savePath);
            Dictionary<string, object> infos =  JsonConvert.DeserializeObject<Dictionary<string, object>>(str);
            raw_materials = (Dictionary<string, int>)infos["raw_materials"];
            materials.Deserialize((ObjectUIDInfo)infos["materials"]);
            components.Deserialize((ObjectUIDInfo)infos["components"]);
        }

        public void Save()
        {
            Dictionary<string,object> infos = new Dictionary<string,object>();
            ObjectUIDInfo info = materials.Serialize();
            infos.Add("materials", info);
            info = components.Serialize();
            infos.Add("components", info);
            infos.Add("raw_materials", raw_materials);

            string str = JsonConvert.SerializeObject(infos);
            File.WriteAllText(savePath, str);

        }

        public Forge()
        {
            Dictionary<string,string> settings = (Dictionary<string, string>)GameSettings.Instance.GetSettings("forge");
            savePath = settings["savePath"];
            materials = new ItemContainer();
            components = new ItemContainer();
        }
    }
}