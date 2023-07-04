// Ignore Spelling: Creat

using System.Collections.Generic;

namespace ER.Items
{
    /// <summary>
    /// 物品信息
    /// </summary>
    public struct ItemInfo
    {
        public int ID;

        /// <summary>
        /// 文本属性（键名：文本所在的标识头）
        /// </summary>
        public Dictionary<string, string> attributeText;

        /// <summary>
        /// 整形属性
        /// </summary>
        public Dictionary<string, int> attributeInt;

        /// <summary>
        /// 浮点属性
        /// </summary>
        public Dictionary<string, float> attributeFloat;

        /// <summary>
        /// 布尔属性
        /// </summary>
        public Dictionary<string, bool> attributeBool;
    }

    /// <summary>
    /// 物品对象类
    /// </summary>
    public class Item
    {
        #region 物品属性

        /// <summary>
        /// 物品ID（系统内部的物品ID，不计入物品的拓展属性）；
        /// 赋值使用标识名称“IDLabel”；
        /// 物品ID只在同一个物品仓库里起作用，脱离仓库使用没有意义
        /// </summary>
        public int ID { get; protected set; } = 0;

        #region 物品的拓展属性

        /// <summary>
        /// 文本属性（键名：文本所在的标识头）
        /// </summary>
        protected Dictionary<string, string> attributeText = new();

        /// <summary>
        /// 整形属性
        /// </summary>
        protected Dictionary<string, int> attributeInt = new();

        /// <summary>
        /// 浮点属性
        /// </summary>
        protected Dictionary<string, float> attributeFloat = new();

        /// <summary>
        /// 布尔属性
        /// </summary>
        protected Dictionary<string, bool> attributeBool = new();

        #endregion 物品的拓展属性

        #endregion 物品属性

        #region 尝试获取属性

        public virtual bool TryGetInt(string key, out int value)
        {
            return attributeInt.TryGetValue(key, out value);
        }

        public virtual bool TryGetText(string key, out string value)
        {
            return attributeText.TryGetValue(key, out value);
        }

        public virtual bool TryGetFloat(string key, out float value)
        {
            return attributeFloat.TryGetValue(key, out value);
        }

        public virtual bool TryGetBool(string key, out bool value)
        {
            return attributeBool.TryGetValue(key, out value);
        }

        #endregion 尝试获取属性

        #region 创建属性

        public virtual void CreatAttribute(string key, int value)
        {
            if (key == "IDLabel")
            {
                ID = value;
            }
            else
            {
                attributeInt[key] = value;
            }
        }

        public virtual void CreatAttribute(string key, float value)
        {
            attributeFloat[key] = value;
        }

        public virtual void CreatAttribute(string key, string value)
        {
            attributeText[key] = value;
        }

        public virtual void CreatAttribute(string key, bool value)
        {
            attributeBool[key] = value;
        }

        #endregion 创建属性

        #region 其他方法

        public ItemInfo Info()
        {
            var info_int = new Dictionary<string, int>();
            var info_float = new Dictionary<string, float>();
            var info_text = new Dictionary<string, string>();
            var info_bool = new Dictionary<string, bool>();
            foreach (var key in attributeInt.Keys)
            {
                info_int[key] = attributeInt[key];
            }
            foreach (var key in attributeFloat.Keys)
            {
                info_float[key] = attributeFloat[key];
            }
            foreach (var key in attributeBool.Keys)
            {
                info_bool[key] = attributeBool[key];
            }
            foreach (var key in attributeText.Keys)
            {
                info_text[key] = attributeText[key];
            }
            return new ItemInfo()
            {
                ID = ID,
                attributeBool = info_bool,
                attributeText = info_text,
                attributeFloat = info_float,
                attributeInt = info_int,
            };
        }

        #endregion 其他方法
    }

    /// <summary>
    /// 物品模板类（静态的物品类）
    /// </summary>
    public class ItemTemplate : Item
    {
        /// <summary>
        /// 物品名称(系统内部的物品名称，和玩家所见的名称文本不同，不计入物品的拓展属性)
        /// </summary>
        public string Name { get; protected set; } = "Null";

        public ItemTemplate()
        {
        }

        public ItemTemplate(string Name, ItemInfo info)
        {
            foreach (var key in info.attributeInt.Keys)
            {
                attributeInt[key] = info.attributeInt[key];
            }
            foreach (var key in info.attributeFloat.Keys)
            {
                attributeFloat[key] = info.attributeFloat[key];
            }
            foreach (var key in info.attributeBool.Keys)
            {
                attributeBool[key] = info.attributeBool[key];
            }
            foreach (var key in info.attributeText.Keys)
            {
                attributeText[key] = info.attributeText[key];
            }
            if (info.attributeText.TryGetValue("NameLabel", out string name))
            {
                Name = name;
            }
        }

        /// <summary>
        /// 获取一个自身的深拷贝对象
        /// </summary>
        /// <returns></returns>
        public ItemTemplate Clone()
        {
            ItemInfo info = Info();
            ItemTemplate clone = new();
            clone.attributeBool = info.attributeBool;
            clone.attributeText = info.attributeText;
            clone.attributeFloat = info.attributeFloat;
            clone.attributeInt = info.attributeInt;
            return clone;
        }
    }

    /// <summary>
    /// 动态属性（可任意添加新的属性）
    /// </summary>
    public class ItemVariable : Item
    {
        /// <summary>
        /// 模板ID
        /// </summary>
        public int TemplateID = 0;
    }
}