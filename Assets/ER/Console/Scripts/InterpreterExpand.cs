using ER.Parser;

namespace ER
{
    public static class InterpreterExpand
    {
        /// <summary>
        /// 用于判断一个 Data 数组是否为空数组
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static bool IsEmpty(this Data[] parameters)
        {
            if (parameters.Length == 0) return true;
            return false;
        }

        /// <summary>
        /// 检查 Data 数据类型是否和指定类型匹配
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static bool IsMate(this Data[] parameters, params DataType[] types)
        {
            if (parameters.Length < types.Length) return false;
            for (int i = 0; i < types.Length; i++)
            {
                if (types[i] != DataType.Text && parameters[i].Type != types[i]) return false;
            }
            return true;
        }
    }
}