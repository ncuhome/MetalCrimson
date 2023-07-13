// Ignore Spelling: Creat tmp

using ER.Parser;
using System;
using System.Collections.Generic;
using UnityEngine;
using DataType = ER.Parser.DataType;

namespace ER.Items
{
    [Serializable]
    public struct ItemInfo
    {
        /// <summary>
        /// 物品模板ID
        /// </summary>
        public int ID;

        /// <summary>
        /// 物品模板的名称
        /// </summary>
        public string NameTmp;

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

        public ItemInfo(Dictionary<string, int> kvInt, Dictionary<string, float> kvFloat, Dictionary<string, bool> kvBool, Dictionary<string, string> kvTxt, int id = 0, string name = "")
        {
            ID = id;
            NameTmp = name;
            attributeBool = kvBool;
            attributeFloat = kvFloat;
            attributeInt = kvInt;
            attributeText = kvTxt;
            if (ID == 0)
            {
                if (kvInt.TryGetValue("ID", out int value))
                {
                    ID = value;
                }
            }
            if (NameTmp == string.Empty)
            {
                if (kvTxt.TryGetValue("NameTmp", out string value))
                {
                    NameTmp = value;
                }
            }
        }
        /// <summary>
        /// 检查物品信息，并将其中的ID和名称提出出来
        /// </summary>
        public void Check()
        {
            foreach(var pair in attributeText)
            {
                if(pair.Key == "NameTmp")
                {
                    NameTmp = pair.Value;
                    break;
                }
            }
            foreach(var pair in attributeInt)
            {
                if(pair.Key == "ID")
                {
                    ID = pair.Value;
                    return;
                }
            }
        }
    }

    /// <summary>
    /// 物品对象类
    /// </summary>
    public abstract class Item
    {
        #region 物品属性

        /// <summary>
        /// 物品ID;
        /// 指物品的母本ID（模板ID）
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

        /// <summary>
        /// 查询该物品是否包含指定属性
        /// </summary>
        /// <param name="keyName">属性名称</param>
        /// <param name="dataType">属性值类型</param>
        /// <returns></returns>
        public bool Contains(string keyName, DataType dataType)
        {
            switch (dataType)
            {
                case DataType.Integer:
                    if (attributeInt.TryGetValue(keyName, out int value1)) return true;
                    return false;

                case DataType.Boolean:
                    if (attributeBool.TryGetValue(keyName, out bool value2)) return true;
                    return false;

                case DataType.Text:
                    if (attributeText.TryGetValue(keyName, out string value3)) return true;
                    return false;

                case DataType.Double:
                    if (attributeFloat.TryGetValue(keyName, out float value)) return true;
                    return false;

                default:
                    return false;
            }
        }

        /// <summary>
        /// 判断此物品是否有该属性，并匹配是否属性值相等
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="_value"></param>
        /// <returns></returns>
        public bool Contains(string keyName, Data _value)
        {
            switch (_value.Type)
            {
                case DataType.Integer:
                    if (attributeInt.TryGetValue(keyName, out int value1))
                    {
                        if (value1 == (int)_value.Value) return true;
                    }
                    return false;

                case DataType.Boolean:
                    if (attributeBool.TryGetValue(keyName, out bool value2))
                    {
                        if (value2 == (bool)_value.Value) return true;
                    }
                    return false;

                case DataType.Text:
                    if (attributeText.TryGetValue(keyName, out string value3))
                    {
                        if (value3 == (string)_value.Value) return true;
                    }
                    return false;

                case DataType.Double:
                    if (attributeFloat.TryGetValue(keyName, out float value4))
                    {
                        if (MathF.Abs(value4 - (float)_value.Value) < 0.000001f) return true;
                    }
                    return false;

                default:
                    return false;
            }
        }

        /// <summary>
        /// 获取属性（整型）
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public abstract int GetInt(string key);

        /// <summary>
        /// 获取属性（浮点）
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public abstract float GetFloat(string key);

        /// <summary>
        /// 获取属性（布尔）
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public abstract bool GetBool(string key);

        /// <summary>
        /// 获取属性（文本）
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public abstract string GetText(string key);

        /// <summary>
        /// 尝试获取属性（整型），安全方法，获取失败返回false
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public abstract bool TryGetInt(string key, out int value);

        /// <summary>
        /// 尝试获取属性（文本），安全方法，获取失败返回false
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public abstract bool TryGetText(string key, out string value);

        /// <summary>
        /// 尝试获取属性（浮点），安全方法，获取失败返回false
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public abstract bool TryGetFloat(string key, out float value);

        /// <summary>
        /// 尝试获取属性（布尔），安全方法，获取失败返回false
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public abstract bool TryGetBool(string key, out bool value);

        #endregion 尝试获取属性

        #region 创建属性

        /// <summary>
        /// 创建属性（整型）,如果已存在则修改该属性
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public abstract void CreatAttribute(string key, int value);

        /// <summary>
        /// 创建属性（浮点）,如果已存在则修改该属性
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public abstract void CreatAttribute(string key, float value);

        /// <summary>
        /// 创建属性（文本）,如果已存在则修改该属性
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public abstract void CreatAttribute(string key, string value);

        /// <summary>
        /// 创建属性（布尔）,如果已存在则修改该属性
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public abstract void CreatAttribute(string key, bool value);

        #endregion 创建属性

        #region 其他方法

        /// <summary>
        /// 获取物品属性
        /// </summary>
        /// <returns></returns>
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
            return new ItemInfo(info_int, info_float, info_bool, info_text, ID);
        }

        #endregion 其他方法
    }

    public struct BaseInfo
    {
        public int ID;
        public string Name;
    }

    /// <summary>
    /// 物品模板类（静态的物品类）
    /// </summary>
    public class ItemTemplate : Item
    {
        /// <summary>
        /// 物品名称(系统内部的物品名称，和玩家所见的名称文本不同，不计入物品的拓展属性),属于字符串属性，keyName = "NameTmp"
        /// </summary>
        public string NameTmp { get; protected set; } = "Null";

        public ItemTemplate()
        {
        }

        public ItemTemplate(Item item)
        {
            attributeBool = ((ItemTemplate)item).attributeBool;
            attributeFloat = ((ItemTemplate)item).attributeFloat;
            attributeInt = ((ItemTemplate)item).attributeInt;
            attributeText = ((ItemTemplate)item).attributeText;
            if (attributeText.TryGetValue("NameTmp", out string name))
            {
                NameTmp = name;
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
            NameTmp = info.NameTmp;
            ID = info.ID;
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

        /// <summary>
        /// 获取物品模板属性
        /// </summary>
        /// <returns></returns>
        public new ItemInfo Info()
        {
            ItemInfo info = base.Info();
            info.NameTmp = NameTmp;
            return info;
        }

        public BaseInfo BaseInfo()
        {
            return new BaseInfo()
            {
                ID= ID,
                Name=NameTmp
            };
        }


        #region 物品模板禁止在创建后修改属性

        public override sealed void CreatAttribute(string key, int value)
        {
            Debug.LogError($"<{NameTmp}>物品模板禁止在创建之后修改属性[Int][{key}:{value}]");
        }

        public override sealed void CreatAttribute(string key, bool value)
        {
            Debug.LogError($"<{NameTmp}>物品模板禁止在创建之后修改属性[Bool][{key}:{value}]");
        }

        public override sealed void CreatAttribute(string key, float value)
        {
            Debug.LogError($"<{NameTmp}>物品模板禁止在创建之后修改属性[Float][{key}:{value}]");
        }

        public override sealed void CreatAttribute(string key, string value)
        {
            Debug.LogError($"<{NameTmp}>物品模板禁止在创建之后修改属性[Text][{key}:{value}]");
        }

        #endregion 物品模板禁止在创建后修改属性

        #region 获取属性

        public override int GetInt(string key) => attributeInt[key];

        public override float GetFloat(string key) => attributeFloat[key];

        public override bool GetBool(string key) => attributeBool[key];

        public override string GetText(string key) => attributeText[key];

        public override bool TryGetInt(string key, out int value)
        {
            if (attributeInt.TryGetValue(key, out value))
            {
                return true;
            }
            value = 0;
            return false;
        }

        public override bool TryGetText(string key, out string value)
        {
            if (attributeText.TryGetValue(key, out value))
            {
                return true;
            }
            value = string.Empty;
            return false;
        }

        public override bool TryGetFloat(string key, out float value)
        {
            if (attributeFloat.TryGetValue(key, out value))
            {
                return true;
            }
            value = 0f;
            return false;
        }

        public override bool TryGetBool(string key, out bool value)
        {
            if (attributeBool.TryGetValue(key, out value))
            {
                return true;
            }
            value = false;
            return false;
        }

        #endregion 获取属性
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
            attributeText["Name"] = itemTemplate.NameTmp;
        }

        /// <summary>
        /// 基于一个模板创建一个物品对象
        /// </summary>
        /// <param name="itemTemplate"></param>
        public ItemVariable(int templateID)
        {
            ID = templateID;
            ItemTemplate tmp = ItemTemplateStore.Instance[ID];
            if (tmp != null)
            {
                attributeText["Name"] = tmp.NameTmp;
            }
            attributeText["Name"] = "NULL";
        }

        #endregion 构造函数

        #region 属性

        /// <summary>
        /// 物品的名称
        /// </summary>
        public string Name { get => attributeText["Name"]; set => attributeText["Name"] = value; }

        #endregion 属性

        #region 获取属性

        /// <summary>
        /// 获取整型属性，优先获取模板中的属性
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override int GetInt(string key)
        {
            ItemTemplate tmp = ItemTemplateStore.Instance[key];
            if (tmp != null && tmp.TryGetInt(key, out int value))
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
            if (tmp != null && tmp.TryGetInt(key, out value))
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

        #endregion 获取属性

        #region 创建|修改属性

        public override void CreatAttribute(string key, int value)
        {
            attributeInt.Add(key, value);
        }

        public override void CreatAttribute(string key, float value)
        {
            attributeFloat.Add(key, value);
        }

        public override void CreatAttribute(string key, string value)
        {
            attributeText.Add(key, value);
        }

        public override void CreatAttribute(string key, bool value)
        {
            attributeBool.Add(key, value);
        }

        #endregion 创建|修改属性
    }
}