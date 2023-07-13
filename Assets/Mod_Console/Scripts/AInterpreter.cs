using ER.Items;
using ER.Parser;
using Mod_Rouge;
using Mod_Save;
using System.IO;

namespace Mod_Console
{
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

        private Data CMD_load_item_list(Data[] parameters)
        {
            if (parameters.IsMate(DataType.Text))
            {
                string path = parameters[0].ToString();
                if(File.Exists(path))
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
        private Data CMD_display_item_list()
        {
            BaseInfo[] infos = ItemTemplateStore.Instance.GetBaseInfoList();
            foreach(var bi in infos)
            {
                Print($"[Name = {bi.Name}, ID = {bi.ID}]");
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

                case "load_item_list"://加载物品列表（静态）
                    return CMD_load_item_list(parameters);
                case "display_item_list"://获取物品列表数据
                    return CMD_display_item_list();
                default:
                    return Data.Error;
            }
        }
    }
}