using ER.Parser;
using System;
using System.Collections.Generic;

namespace ER.Items
{
    /// <summary>
    /// 物品系统(静态)，储存物品模板，相当于是一个物品图鉴;(只读)
    /// </summary>
    public class ItemSystem
    {
        #region 单例封装

        private static ItemSystem instance;

        public ItemSystem Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ItemSystem();
                }
                return instance;
            }
        }

        private ItemSystem()
        { }

        #endregion 单例封装

        private Dictionary<string, ItemTemplate> items = new();
        private Dictionary<int, ItemTemplate> items_ID = new();

        /// <summary>
        /// 加载物品信息表
        /// </summary>
        public void LoadItemsList(string path)
        {
            List<string[]> datas = CSVParser.ParseCSVText(path);

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
                ItemTemplate item = new();

                #region 填入物品信息

                for (int k = 0; k < heads.Length; k++)
                {
                    string head = heads[k];
                    DataType type = types[k];
                    if (head == string.Empty) continue;//表头为空直接跳过

                    #region 封装物品的信息

                    if (k >= itemInfo.Length)//超出数据范围，填入空值
                    {
                        switch (type)
                        {
                            case DataType.Integer:
                                item.CreatAttribute(head, 0);
                                break;

                            case DataType.Text:
                                item.CreatAttribute(head, string.Empty);
                                break;

                            case DataType.Boolean:
                                item.CreatAttribute(head, false);
                                break;

                            case DataType.Double:
                                item.CreatAttribute(head, 0f);
                                break;

                            default:
                                item.CreatAttribute(head, string.Empty);
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
                                    item.CreatAttribute(head, value);
                                }
                                else
                                {
                                    item.CreatAttribute(head, 0);
                                }
                                break;

                            case DataType.Text:
                                item.CreatAttribute(head, info);
                                break;

                            case DataType.Boolean:
                                if (info.ToLower() == "true")
                                {
                                    item.CreatAttribute(head, true);
                                }
                                else
                                {
                                    item.CreatAttribute(head, false);
                                }
                                break;

                            case DataType.Double:
                                if (float.TryParse(info, out float value2))
                                {
                                    item.CreatAttribute(head, value2);
                                }
                                else
                                {
                                    item.CreatAttribute(head, 0f);
                                }
                                break;

                            default:
                                item.CreatAttribute(head, info);
                                break;
                        }
                    }

                    #endregion 封装物品的信息
                }

                #endregion 填入物品信息

                items.Add(item.Name, item);
                items_ID.Add(item.ID, item);
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
        /// 获取物品信息
        /// </summary>
        /// <param name="id">物品ID</param>
        /// <returns></returns>
        public Item this[int id, bool deepCopy = false]
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
    }
}