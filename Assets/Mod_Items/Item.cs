// Ignore Spelling: Creat tmp

using System;
using System.Collections.Generic;
using UnityEngine;
using ER.Parser;

namespace ER.Items
{
    [Serializable]
    /// <summary>
    /// 物品信息
    /// </summary>
    public struct ItemInfo
    {
        /// <summary>
        /// 物品所在的仓库ID
        /// </summary>
        public int ID;
        /// <summary>
        /// 物品模板的名称
        /// </summary>
        public string Name;

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

        public virtual int GetInt(string key) => attributeInt[key];
        public virtual float GetFloat(string key) => attributeFloat[key];
        public virtual bool GetBool(string key) => attributeBool[key];
        public virtual string GetText(string key) => attributeText[key];

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
                Name = string.Empty
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
        /// 物品名称(系统内部的物品名称，和玩家所见的名称文本不同，不计入物品的拓展属性),属于字符串属性，keyName = "NameLabel"
        /// </summary>
        public string Name { get; protected set; } = "Null";

        public ItemTemplate()
        {
        }

        public ItemTemplate(Item item)
        {
            attributeBool = ((ItemTemplate)item).attributeBool;
            attributeFloat = ((ItemTemplate)item).attributeFloat;
            attributeInt = ((ItemTemplate)item).attributeInt;
            attributeText = ((ItemTemplate)item).attributeText;
            if (attributeText.TryGetValue("NameLabel", out string name))
            {
                Name = name;
            }
        }

        public ItemTemplate(ItemInfo info)
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
            Name = info.Name;
            if (Name != string.Empty) return;
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

        public new ItemInfo Info()
        {
            ItemInfo info = base.Info();
            info.Name = Name;
            return info;
        }

        public override void CreatAttribute(string key,int value)
        {
            Debug.LogError($"<{Name}>物品模板禁止在创建之后修改属性[Int][{key}:{value}]");
        }
        public override void CreatAttribute(string key, bool value)
        {
            Debug.LogError($"<{Name}>物品模板禁止在创建之后修改属性[Bool][{key}:{value}]");
        }
        public override void CreatAttribute(string key, float value)
        {
            Debug.LogError($"<{Name}>物品模板禁止在创建之后修改属性[Float][{key}:{value}]");
        }
        public override void CreatAttribute(string key, string value)
        {
            Debug.LogError($"<{Name}>物品模板禁止在创建之后修改属性[Text][{key}:{value}]");
        }
    }

    /// <summary>
    /// 动态物品（可任意添加新的属性）
    /// </summary>
    public class ItemVariable : Item
    {
        //ID表示的使用的模板物品的ID
        #region 构造函数
        public ItemVariable()
        {
            ID = 0;
        }
        /// <summary>
        /// 基于一个模板创建一个物品对象
        /// </summary>
        /// <param name="itemTemplate"></param>
        public ItemVariable(ItemTemplate itemTemplate) 
        {
            ID = itemTemplate.ID;
        }
        /// <summary>
        /// 基于一个模板创建一个物品对象
        /// </summary>
        /// <param name="itemTemplate"></param>
        public ItemVariable(int templateID)
        {
            ID = templateID;
        }
        #endregion

        #region 尝试获取属性
        /// <summary>
        /// 获取整型属性，优先获取模板中的属性
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override int GetInt(string key)
        {
            ItemTemplate tmp = ItemTemplateStore.Instance[key];
            if(tmp != null && tmp.TryGetInt(key,out int value))
            {
                return value;
            }
            return attributeInt[key];
        }
        /// <summary>
        /// 获取浮点型属性，优先获取模板中的属性
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override float GetFloat(string key)
        {
            ItemTemplate tmp = ItemTemplateStore.Instance[key];
            if (tmp != null && tmp.TryGetFloat(key, out float value))
            {
                return value;
            }
            return attributeFloat[key];
        }
        /// <summary>
        /// 获取布尔型的属性，优先获取模板中的属性
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override bool GetBool(string key)
        {
            ItemTemplate tmp = ItemTemplateStore.Instance[key];
            if (tmp != null && tmp.TryGetBool(key, out bool value))
            {
                return value;
            }
            return attributeBool[key];
        }
        /// <summary>
        /// 获取字符串属性，优先获取模板中的属性
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override string GetText(string key)
        {
            ItemTemplate tmp = ItemTemplateStore.Instance[key];
            if (tmp != null && tmp.TryGetText(key, out string value))
            {
                return value;
            }
            return attributeText[key];
        }
        /// <summary>
        /// 尝试获取整形属性，优先获取模板中的属性
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override bool TryGetInt(string key, out int value)
        {
            ItemTemplate tmp = ItemTemplateStore.Instance[key];
            if (tmp != null && tmp.TryGetInt(key,out value))
            {
                return true;
            }
            return attributeInt.TryGetValue(key, out value);
        }
        /// <summary>
        /// 尝试获取字符串属性，优先获取模板中的属性
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override bool TryGetText(string key, out string value)
        {
            ItemTemplate tmp = ItemTemplateStore.Instance[key];
            if (tmp != null && tmp.TryGetText(key, out value))
            {
                return true;
            }
            return attributeText.TryGetValue(key, out value);
        }
        /// <summary>
        /// 尝试获取浮点型属性，优先获取模板中的属性
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override bool TryGetFloat(string key, out float value)
        {
            ItemTemplate tmp = ItemTemplateStore.Instance[key];
            if (tmp != null && tmp.TryGetFloat(key, out value))
            {
                return true;
            }
            return attributeFloat.TryGetValue(key, out value);
        }
        /// <summary>
        /// /// <summary>
        /// 尝试获取整形属性，优先获取模板中的属性
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// </summary>
        public override bool TryGetBool(string key, out bool value)
        {
            ItemTemplate tmp = ItemTemplateStore.Instance[key];
            if (tmp != null && tmp.TryGetBool(key, out value))
            {
                return true;
            }
            return attributeBool.TryGetValue(key, out value);
        }

        #endregion 尝试获取属性
    }
}