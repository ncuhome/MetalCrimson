using ER.Items;
using ER.Parser;
using Mod_Rouge;
using Mod_Save;
using System.IO;

namespace Mod_Console
{
    /// <summary>
    /// 指令解释器A
    /// </summary>
    public class AInterpreter : DefaultInterpreter
    {
        #region 指令函数

        private Data CMD_creatmap(Data[] parameters)
        {
            int seed = 0;

            if (!parameters.IsEmpty())
            {
                if (parameters.IsMate(DataType.Integer))
                {
                    seed = (int)parameters[0].Value;
                }
                else
                {
                    PrintError($"无效参数：<{parameters[0]}>，此参数必须为整型");
                }
            }
            Map map = Map.Creat(seed);
            return Data.Empty;
        }

        private Data CMD_settings()
        {
            string settings = SettingsManager.Instance.GetSettingsTxt();
            ConsolePanel.Print(settings);
            return new Data(settings, DataType.Text);
        }

        private Data CMD_itemlist_load(Data[] parameters)
        {
            if (parameters.IsMate(DataType.Text))
            {
                string path = parameters[0].ToString();
                if (File.Exists(path))
                {
                    ItemTemplateStore.Instance.LoadItemsList(path);
                    Print($"正在从{path}中读取数据...");
                }
                else
                {
                    PrintError($"无效路径：{path}");
                }
            }
            else
            {
                PrintError("指令参数类型不匹配，或者指令参数为空");
                PrintError("正确格式：itemlist_load [FilePath]");
            }

            return Data.Empty;
        }

        private Data CMD_itemlist_display()
        {
            BaseInfo[] infos = ItemTemplateStore.Instance.GetBaseInfoList();
            foreach (var bi in infos)
            {
                Print($"[Name = {bi.Name}, ID = {bi.ID}]");
            }
            return Data.Empty;
        }

        private Data CMD_savestore(Data[] parameters)
        {
            string saveName = "新的存档";
            if (!parameters.IsEmpty())
            {
                saveName = parameters[0].ToString();
            }
            SaveManager.Instance.Save(saveName);
            return Data.Empty;
        }

        private Data CMD_savelist()
        {
            SaveManager.Instance.UpdateList();
            foreach (var save in SaveManager.Instance.saves)
            {
                Print(Path.GetFileNameWithoutExtension(save.Name));
            }
            return Data.Empty;
        }

        private Data CMD_saveload(Data[] parameters)
        {
            if (!parameters.IsEmpty())
            {
                SaveManager.Instance.Load(parameters[0].ToString());
                Print("存档加载完毕");
            }
            else
            {
                PrintError("缺少存档路径参数");
            }
            return Data.Empty;
        }

        private Data CMD_savedisplay()
        {
            if (SaveWrapper.Instance.Data != null)
            {
                SaveWrapper.Instance.Data.PrintInfo();
            }
            else
            {
                PrintError("当前存档为空");
            }
            return Data.Empty;
        }

        private Data CMD_itemstore_creat(Data[] parameters)
        {
            if (parameters.Length >= 2)
            {
                if (parameters.IsMate(DataType.Text, DataType.Integer))
                {
                    ItemStoreManager.Instance.Creat(parameters[0].ToString(), (int)parameters[1].Value);
                    Print($"仓库创建成功:{parameters[0].ToString()}");
                }
                else
                {
                    PrintError("参数错误");
                }
            }
            else if (parameters.Length == 1)
            {
                ItemStoreManager.Instance.Creat(parameters[0].ToString());
                Print($"仓库创建成功:{parameters[0].ToString()}");
            }
            else
            {
                PrintError("缺少参数：仓库名称");
            }
            return Data.Empty;
        }

        private Data CMD_itemstore_list()
        {
            foreach (var pair in ItemStoreManager.Instance.Stores)
            {
                Print($"[{pair.Key}]");
            }
            return Data.Empty;
        }

        private Data CMD_itemstore_display(Data[] parameters)
        {
            if (!parameters.IsEmpty())
            {
                string name = parameters[0].ToString();
                if (ItemStoreManager.Instance.Exist(name))
                {
                    ItemStore store = ItemStoreManager.Instance[name];
                    store.Print();
                }
                else
                {
                    PrintError($"指定仓库不存在:{name}");
                }
            }
            else
            {
                PrintError("缺少参数：仓库名称");
            }

            return Data.Empty;
        }

        private Data CMD_itemstore_item_add(Data[] parameters)
        {
            if (parameters.IsMate(DataType.Text, DataType.Function) || parameters.IsMate(DataType.Text, DataType.Integer))
            {
                string name = (string)parameters[0].Value;
                if (ItemStoreManager.Instance.Exist(name))
                {
                    ItemStore store = ItemStoreManager.Instance[name];

                    if (int.TryParse(parameters[1].Value.ToString(), out int id))
                    {
                        if(ItemTemplateStore.Instance.Exist(id))
                        {

                            if (store.AddItem(new ItemVariable(id)))
                            {
                                Print($"添加物品[{ItemTemplateStore.Instance[id].NameTmp}]成功");
                                Print($"现在仓库中有{store.Count}");
                            }
                            else
                            {
                                Print("添加物品失败，背包已满");
                                Print($"现在仓库[{store.storeName}]中有{store.Count}");
                            }
                        }
                        else
                        {
                            PrintError("指定物品不存在");
                        }
                    }
                    else
                    {
                        PrintError("传入物品数据有误");
                    }
                }
                else
                {
                    PrintError("指定动态仓库不存在");
                }
            }
            else if(parameters.IsMate(DataType.Text, DataType.Text))
            {
                string name = (string)parameters[0].Value;
                if (ItemStoreManager.Instance.Exist(name))
                {
                    ItemStore store = ItemStoreManager.Instance[name];
                    string tmpName = (string)parameters[1].Value;
                    if(ItemTemplateStore.Instance.Exist(tmpName))
                    {
                        ItemTemplate item = ItemTemplateStore.Instance[tmpName];
                        if (store.AddItem(new ItemVariable(item.ID)))
                        {
                            Print($"添加物品[{tmpName}]成功");
                            Print($"现在仓库中有{store.Count}");
                        }
                        else
                        {
                            Print("添加物品失败，背包已满");
                            Print($"现在仓库[{store.storeName}]中有{store.Count}");
                        }
                    }
                    else
                    {
                        PrintError("指定物品不存在");
                    }
                }
                else
                {
                    PrintError("指定动态仓库不存在");
                }
            }
            else
            {
                PrintError("参数错误");
            }
            return Data.Empty;
        }

        private Data CMD_itemstore_item_del(Data[] parameters)
        {
            if (parameters.IsMate(DataType.Text, DataType.Integer))
            {
                string name = (string)parameters[0].Value;
                if (ItemStoreManager.Instance.Exist(name))
                {
                    ItemStore store = ItemStoreManager.Instance[name];

                    if (int.TryParse(parameters[0].Value.ToString(), out int index))
                    {
                        if (store.RemoveItem(index))
                        {
                            Print("移除物品成功");
                        }
                        else
                        {
                            Print("指定物品不存在");
                        }
                    }
                    else
                    {
                        PrintError("传入物品数据有误");
                    }
                }
                else
                {
                    PrintError("指定动态仓库不存在");
                }
            }
            return Data.Empty;
        }

        private Data CMD_itemstore_item_clear(Data[] parameters)
        {
            if (parameters.IsMate(DataType.Text))
            {
                string name = (string)parameters[0].Value;
                if (ItemStoreManager.Instance.Exist(name))
                {
                    ItemStore store = ItemStoreManager.Instance[name];
                    store.Clear();
                    Print($"已清空指定仓库：{name}");
                }
                else
                {
                    PrintError("指定动态仓库不存在");
                }
            }
            return Data.Empty;
        }

        #endregion 指令函数

        public override Data EffectuateSuper(string commandName, Data[] parameters)
        {
            switch (commandName)
            {
                case "creatmap"://创建肉鸽地图
                    return CMD_creatmap(parameters);

                case "settings"://显示设置配置
                    return CMD_settings();

                case "itemlist_load"://加载物品列表（静态）
                    return CMD_itemlist_load(parameters);

                case "itemlist_display"://获取物品列表数据
                    return CMD_itemlist_display();

                case "savestore"://保存存档
                    return CMD_savestore(parameters);

                case "savelist"://显示当前存档文件夹下的所有存档
                    return CMD_savelist();

                case "saveload"://加载指定存档文件
                    return CMD_saveload(parameters);

                case "savedisplay"://显示当前存档信息
                    return CMD_savedisplay();

                case "itemstore_creat"://创建一个动态仓库
                    return CMD_itemstore_creat(parameters);

                case "itemstore_list"://查看所有动态仓库列表
                    return CMD_itemstore_list();

                case "itemstore_display"://展示指定动态仓库
                    return CMD_itemstore_display(parameters);

                case "itemstore_item_add"://向指定动态仓库添加物品
                    return CMD_itemstore_item_add(parameters);

                case "itemstore_item_del"://删除指定动态仓库中的指定物品
                    return CMD_itemstore_item_del(parameters);

                case "itemstore_item_clear"://清空指定动态仓库
                    return CMD_itemstore_item_clear(parameters);

                default:
                    return Data.Error;
            }
        }
    }
}