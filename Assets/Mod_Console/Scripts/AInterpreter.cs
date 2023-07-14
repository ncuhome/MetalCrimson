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
            ConsolePanel.Instance.Print(settings);
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
                PrintError("正确格式：load_item_list [FilePath]");
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

                default:
                    return Data.Error;
            }
        }
    }
}