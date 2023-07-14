using ER.Parser;
using System.Collections.Generic;
using DataType = ER.Parser.DataType;

namespace ER.Items
{
    /// <summary>
    /// 物品系统(静态)，储存物品模板，相当于是一个物品图鉴;(只读)
    /// </summary>
    public class ItemTemplateStore
    {
        #region 单例封装

        private static ItemTemplateStore instance;

        public static ItemTemplateStore Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ItemTemplateStore();
                }
                return instance;
            }
        }

        private ItemTemplateStore()
        { }

        #endregion 单例封装

        private Dictionary<string, ItemTemplate> items = new();
        private Dictionary<int, ItemTemplate> items_ID = new();

        /// <summary>
        /// 加载物品信息表
        /// </summary>
        public void LoadItemsList(string path)
        {
            List<string[]> datas = CSVParser.ParseCSV(path);

            string[] type0 = datas[0];//数据类型(表外表头)
            DataType[] types = new DataType[type0.Length];
            for (int i = 0; i < types.Length; i++)//整理数据类型
            {
                string ty = type0[i].ToLower();
                switch (ty)
                {
                    case "int":
                        types[i] = DataType.Integer;
                        break;

                    case "string":
                        types[i] = DataType.Text;
                        break;

                    case "float":
                        types[i] = DataType.Double;
                        break;

                    case "bool":
                        types[i] = DataType.Boolean;
                        break;

                    default:
                        types[i] = DataType.Unknown;
                        break;
                }
            }

            string[] heads = datas[1];//表头

            for (int i = 2; i < datas.Count; i++)
            {
                string[] itemInfo = datas[i];
                if (itemInfo.isEmpty()) continue;//如果为数据空行直接跳过物品封装

                Dictionary<string, int> ints = new();
                Dictionary<string, float> floats = new();
                Dictionary<string, string> strings = new();
                Dictionary<string, bool> bools = new();

                #region 填入物品信息

                for (int k = 0; k < heads.Length; k++)
                {
                    string head = heads[k];
                    DataType type = types[k];
                    if (head == string.Empty) continue;//数据对应的表头为空直接跳过

                    #region 封装物品的信息

                    if (k >= itemInfo.Length)//超出数据范围，填入空值
                    {
                        switch (type)
                        {
                            case DataType.Integer:
                                ints[head] = 0;
                                break;

                            case DataType.Text:
                                strings[head] = string.Empty;
                                break;

                            case DataType.Boolean:
                                bools[head] = false;
                                break;

                            case DataType.Double:
                                floats[head] = 0f;
                                break;

                            default:
                                strings[head] = string.Empty;
                                break;
                        }
                    }
                    else
                    {
                        string info = itemInfo[k];
                        switch (type)
                        {
                            case DataType.Integer:
                                if (int.TryParse(info, out int value))
                                {
                                    ints[head] = value;
                                }
                                else
                                {
                                    ints[head] = 0;
                                }
                                break;

                            case DataType.Text:
                                strings[head] = info;
                                break;

                            case DataType.Boolean:
                                if (info.ToLower() == "true")
                                {
                                    bools[head] = true;
                                }
                                else
                                {
                                    bools[head] = false;
                                }
                                break;

                            case DataType.Double:
                                if (float.TryParse(info, out float value2))
                                {
                                    floats[head] = value2;
                                }
                                else
                                {
                                    floats[head] = 0f;
                                }
                                break;

                            default:
                                strings[head] = info;
                                break;
                        }
                    }

                    #endregion 封装物品的信息
                }

                #endregion 填入物品信息

                ItemInfo infos = new(ints, floats, bools, strings);
                infos.Check();
                ItemTemplate item = new(infos);

                items[item.NameTmp] = item;
                items_ID[item.ID] = item;
            }
        }

        /// <summary>
        /// 清空加载表
        /// </summary>
        public void Clear()
        {
            items.Clear();
        }

        /// <summary>
        /// 获取物品模板
        /// </summary>
        /// <param name="nameLabel">名称标签</param>
        /// <param name="deepCopy">是否是深拷贝</param>
        /// <returns></returns>
        public ItemTemplate this[string nameLabel, bool deepCopy = false]
        {
            get
            {
                if (items.TryGetValue(nameLabel, out ItemTemplate tmp))
                {
                    if (deepCopy)
                    {
                        return tmp.Clone();
                    }
                    return tmp;
                }
                return null;
            }
        }

        /// <summary>
        /// 查找拥有指定属性的物品
        /// </summary>
        /// <param name="key">键名</param>
        /// <param name="dataType">属性类型</param>
        /// <returns></returns>
        public List<ItemTemplate> Find(string key, DataType dataType)
        {
            if (dataType != DataType.Integer || dataType != DataType.Double || dataType != DataType.Text || dataType != DataType.Boolean) return null;
            List<ItemTemplate> tmps = new List<ItemTemplate>();
            foreach (int tmp in items_ID.Keys)
            {
                ItemTemplate item = items_ID[tmp];
                if (item.Contains(key, dataType))
                {
                    tmps.Add(item);
                }
            }
            return tmps;
        }

        /// <summary>
        /// 查找拥有指定属性的物品，同时匹配目标属性值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public List<ItemTemplate> Find(string key, Data value)
        {
            if (value.Type != DataType.Integer || value.Type != DataType.Double || value.Type != DataType.Text || value.Type != DataType.Boolean) return null;
            List<ItemTemplate> tmps = new List<ItemTemplate>();
            foreach (int tmp in items_ID.Keys)
            {
                ItemTemplate item = items_ID[tmp];
                if (item.Contains(key, value))
                {
                    tmps.Add(item);
                }
            }
            return tmps;
        }

        /// <summary>
        /// 查询指定属性包含指定值的所有物品对象
        /// </summary>
        /// <param name="key">属性名</param>
        /// <param name="value">需要包含的属性值</param>
        /// <param name="spc">属性值分割符</param>
        /// <returns></returns>
        public List<ItemTemplate> FindContainsPart(string key, string value, char spc)
        {
            List<ItemTemplate> tmps = new List<ItemTemplate>();
            foreach (int tmp in items_ID.Keys)
            {
                ItemTemplate item = items_ID[tmp];
                if (item.ContainsSPT(key, spc, value))
                {
                    tmps.Add(item);
                }
            }
            return tmps;
        }

        /// <summary>
        /// 获取物品模板
        /// </summary>
        /// <param name="id">物品ID</param>
        /// <param name="deepCopy">是否是深拷贝</param>
        /// <returns></returns>
        public ItemTemplate this[int id, bool deepCopy = false]
        {
            get
            {
                if (items_ID.TryGetValue(id, out ItemTemplate tmp))
                {
                    if (deepCopy)
                    {
                        return tmp.Clone();
                    }
                    return tmp;
                }
                return null;
            }
        }

        /// <summary>
        /// 获取模板的基础信息列表
        /// </summary>
        /// <returns></returns>
        public BaseInfo[] GetBaseInfoList()
        {
            BaseInfo[] infos = new BaseInfo[items.Count];
            int i = 0;
            foreach (string key in items.Keys)
            {
                infos[i] = items[key].BaseInfo();
                i++;
            }
            return infos;
        }
    }
}