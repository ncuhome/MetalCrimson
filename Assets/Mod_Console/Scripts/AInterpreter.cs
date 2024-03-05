using ER.Items;
using ER.Parser;
using ER.Save;
using Mod_Rouge;
using System.IO;

namespace Mod_Console
{
    /// <summary>
    /// 指令解释器A
    /// </summary>
    public partial class AInterpreter : DefaultInterpreter
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
            //Map map = Map.Create(seed);
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
                    TemplateStoreManager.Instance["Item"].LoadItemsList(path);
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
            BaseInfo[] infos = TemplateStoreManager.Instance["Item"].GetBaseInfoList();
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
                    ItemStoreManager.Instance.Create(parameters[0].ToString(), (int)parameters[1].Value);
                    Print($"仓库创建成功:{parameters[0].ToString()}");
                }
                else
                {
                    PrintError("参数错误");
                }
            }
            else if (parameters.Length == 1)
            {
                ItemStoreManager.Instance.Create(parameters[0].ToString());
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
                        if (TemplateStoreManager.Instance["Item"].Exist(id))
                        {
                            if (store.AddItem(new ItemVariable("Item", id)))
                            {
                                Print($"添加物品[{TemplateStoreManager.Instance["Item"][id].NameTmp}]成功");
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
            else if (parameters.IsMate(DataType.Text, DataType.Text))
            {
                string name = (string)parameters[0].Value;
                if (ItemStoreManager.Instance.Exist(name))
                {
                    ItemStore store = ItemStoreManager.Instance[name];
                    string tmpName = (string)parameters[1].Value;
                    if (TemplateStoreManager.Instance["Item"].Exist(tmpName))
                    {
                        ItemTemplate item = TemplateStoreManager.Instance["Item"][tmpName];
                        if (store.AddItem(new ItemVariable("Item", item.ID)))
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

        private Data CMD_mapconfig_load(Data[] parameters)
        {
            if (!parameters.IsEmpty())
            {
                Print("正在加载地图配置");
                string path = (string)parameters[0].Value;
                if (File.Exists(path))
                {
                    RougeMap.Instance.LoadConfig(path);
                    Print("加载地图配置完毕");
                }
                else
                {
                    PrintError("指定路径不存在");
                }
            }
            PrintError("路径参数不可为空");
            return Data.Error;
        }

        private Data CMD_mapconfig()
        {
            Print($"地图最大层数: {RougeMap.Instance.level_settings}");
            Print($"地图最大长度: {RougeMap.Instance.maxLength_settings}");
            Print($"地图最小长度: {RougeMap.Instance.minLength_settings}");

            float[][] settings = RougeMap.Instance.probabilities_settings;//房间概率值
            Print("房间概率值:");
            if (settings == null) PrintError("配置未初始化");
            else
            {
                for (int i = 0; i < settings.Length; i++)
                {
                    Print($"{(RoomType)i}:[", false);
                    for (int k = 0; k < settings[i].Length; k++)
                    {
                        Print($" {settings[i][k]} ", false);
                    }
                    Print("]");
                }
            }

            int[][] settings2 = RougeMap.Instance.maxCount_settings;//房间最大值
            Print("房间最大值:");
            if (settings == null) PrintError("配置未初始化");
            else
            {
                for (int i = 0; i < settings2.Length; i++)
                {
                    Print($"{(RoomType)i}:[", false);
                    for (int k = 0; k < settings2[i].Length; k++)
                    {
                        Print($" {settings2[i][k]} ", false);
                    }
                    Print("]");
                }
            }

            int[][] settings3 = RougeMap.Instance.minCount_settings;//房间最小值
            Print("房间最小值:");
            if (settings == null) PrintError("配置未初始化");
            else
            {
                for (int i = 0; i < settings3.Length; i++)
                {
                    Print($"{(RoomType)i}:[", false);
                    for (int k = 0; k < settings3[i].Length; k++)
                    {
                        Print($" {settings3[i][k]} ", false);
                    }
                    Print("]");
                }
            }

            return Data.Empty;
        }

        private Data CMD_mapinfo()
        {
            RougeMap.Instance.PrintInfo();
            return Data.Empty;
        }

        private Data CMD_map_start()
        {
            Print("------------------------------------------");
            Room room = RougeMap.Instance.Start();
            room.Print();
            Print("Exit:");
            RougeMap.Instance.PrintExits();
            RougeMap.Instance.PrintInfo();
            Print("------------------------------------------");

            return Data.Empty;
        }

        private Data CMD_map_select(Data[] parameters)
        {
            if (parameters.IsMate(DataType.Integer))
            {
                Print("------------------------------------------");
                int index = (int)parameters[0].Value;
                Room room = RougeMap.Instance.SelectRoom(index);
                if (room != null)
                {
                    room.Print();
                    Print("Exit:");
                    RougeMap.Instance.PrintExits();
                    RougeMap.Instance.PrintInfo();
                }
                Print("------------------------------------------");
            }
            return Data.Empty;
        }

        private Data CMD_forge_addItem(Data[] parameters)
        {
            if (parameters.IsMate(DataType.Integer))
            {
                int index = (int)parameters[0].Value;//材料在仓库中的索引
                bool state = false;
                if (HammeringSystem.Instance.AddMaterialJudgement(MaterialSystem.Instance.GetMaterialScript(index), out state))
                {
                    Print("添加材料成功");
                }
                else
                {
                    PrintError("添加材料失败");
                }
                if (!state)
                {
                    PrintError("指定索引物品不存在");
                }
            }
            return Data.Empty;
        }

        private Data CMD_forge_removeItem(Data[] parameters)
        {
            if (parameters.IsMate(DataType.Integer))
            {
                int index = (int)parameters[0].Value;//材料在炉子中的索引
                if (index >= 0 && index <= 2)
                {
                    if (!HammeringSystem.Instance.MoveBackMaterial(index))
                    {
                        PrintError("指定索引物品不存在");
                    }
                }
                else
                {
                    PrintError($"索引参数错误：{index}");
                    PrintError("索引必须在0~2");
                }
            }
            return Data.Empty;
        }

        private Data CMD_forge_temperature()
        {
            Print($"当前炉子的温度：{HammeringSystem.Instance.temperature}");
            return Data.Empty;
        }

        private Data CMD_forge_temperature_set(Data[] parameters)
        {
            if (parameters.IsMate(DataType.Double))
            {
                //Print(parameters[0].Type.ToString());
                //Print(parameters[0].Value.ToString());
                float tmp = (float)(parameters[0].Value);
                //Print($"temp:{tmp}");
                if (tmp < 0) { tmp = 0; }
                //Print($"temp:{tmp}");
                HammeringSystem.Instance.temperature = tmp;
            }
            else if (parameters.IsMate(DataType.Integer))
            {
                int tmp = (int)(parameters[0].Value);
                //Print($"temp:{tmp}");
                if (tmp < 0) { tmp = 0; }
                //Print($"temp:{tmp}");
                HammeringSystem.Instance.temperature = tmp;
            }
            return Data.Empty;
        }

        private Data CMD_forge_end()
        {
            QTE.Instance.FinishQTE();
            HammeringSystem.Instance.FinishHammering();
            return Data.Empty;
        }

        private Data CMD_forge(Data[] parameters)
        {
            if (parameters.IsMate(DataType.Integer))
            {
                int index = (int)parameters[0].Value;
                int times = 1;
                if (parameters.IsMate(DataType.Integer, DataType.Integer))
                {
                    times = (int)parameters[1].Value;
                    if (times <= 0) times = 1;
                }

                for (int i = 0; i < times; i++)
                {
                    switch (index)
                    {
                        case 1:
                            HammeringSystem.Instance.HammerMaterial(0.5f);
                            break;

                        case 2:
                            HammeringSystem.Instance.HammerMaterial(0.9f);
                            break;

                        case 3:
                            HammeringSystem.Instance.HammerMaterial(1.0f);
                            break;
                    }
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

                case "mapconfig_load"://加载地图生成配置
                    return CMD_mapconfig_load(parameters);

                case "mapconfig"://打印地图配置信息
                    return CMD_mapconfig();

                case "mapinfo"://打印当前肉鸽地图信息
                    return CMD_mapinfo();

                case "map_start"://开始肉鸽关卡；初始化地图
                    return CMD_map_start();

                case "map_select"://模拟肉鸽地图选择房间
                    return CMD_map_select(parameters);

                case "forge_addItem"://材料加工：往炉子里加物品
                    return CMD_forge_addItem(parameters);

                case "forge_removeItem"://材料加工：从炉子内移除物品
                    return CMD_forge_removeItem(parameters);

                case "forge_temperature"://材料加工：显示炉子温度
                    return CMD_forge_temperature();

                case "forge_temperature_set"://材料加工：控制炉子温度
                    return CMD_forge_temperature_set(parameters);

                case "forge_end":
                    return CMD_forge_end();

                case "forge":
                    return CMD_forge(parameters);

                default:
                    return Data.Error;
            }
        }
    }
}