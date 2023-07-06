using ER.Parser;
using Mod_Rouge;
using Mod_Save;

namespace Mod_Console
{
    public class AInterpreter : DefaultInterpreter
    {
        #region ָ���
        private Data CMD_creatmap(Data[] parameters)
        {
            int seed = 0;
            if (parameters.Length > 0)
            {
                if (parameters[0].Type == DataType.Integer)
                {
                    seed = (int)parameters[0].Value;
                }
                else
                {
                    PrintError($"��Ч������<{parameters[0]}>���˲�������Ϊ����");
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
        #endregion

        public override Data EffectuateSuper(string commandName, Data[] parameters)
        {
            switch (commandName)
            {
                case "creatmap":
                    return CMD_creatmap(parameters);
                case "settings":
                    return CMD_settings();
                default:
                    return Data.Error;
            }
        }
    }
}