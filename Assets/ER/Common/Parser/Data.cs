using System;

namespace ER.Parser
{
    /// <summary>
    /// 数据类型
    /// </summary>
    public enum DataType
    {
        /// <summary>
        /// 未知类型，默认类型
        /// </summary>
        Unknown,

        /// <summary>
        /// 整型
        /// </summary>
        Integer,

        /// <summary>
        /// 浮点型
        /// </summary>
        Double,

        /// <summary>
        /// 布尔型
        /// </summary>
        Boolean,

        /// <summary>
        /// 文本型
        /// </summary>
        Text,

        /// <summary>
        /// 指令
        /// </summary>
        Function,

        /// <summary>
        /// 错误类型
        /// </summary>
        Error,
    }

    /// <summary>
    /// 解析数据，包含一个数据本体(object)，以及它的真实数据类型
    /// </summary>
    public struct Data
    {
        #region 属性

        public object Value { get; private set; }
        public DataType Type { get; private set; }

        #endregion 属性

        #region 静态

        /// <summary>
        /// 通知委托
        /// </summary>
        public static event Action<string> Output = message => Console.WriteLine(message);

        /// <summary>
        /// 获取指定字符创的数据解析
        /// </summary>
        /// <param name="dataString"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Data ParseTo(string dataString, DataType type = DataType.Unknown)
        {
            Data data = new Data();
            data.Parse(dataString, type);
            return data;
        }

        /// <summary>
        /// 获取一个表示空的解析数据
        /// </summary>
        public static Data Empty => new Data(null, DataType.Unknown);

        /// <summary>
        /// 获取一个表示错误的解析数据
        /// </summary>
        public static Data Error => new Data(null, DataType.Error);

        public static DataType Parse(string dataString, out object Value, DataType type = DataType.Unknown)
        {
            switch (type)
            {
                case DataType.Unknown:
                    if (dataString.TryParseInt(out int iv))
                    {
                        Value = iv;
                        return DataType.Integer;
                    }
                    else if (dataString.TryParseDouble(out double dv))
                    {
                        Value = dv;
                        return DataType.Double;
                    }
                    else if (dataString.TryParseBoolean(out bool bv))
                    {
                        Value = bv;
                        return DataType.Boolean;
                    }
                    Value = dataString;
                    return DataType.Text;

                case DataType.Integer:
                    if (dataString.TryParseInt(out int iv0))
                    {
                        Value = iv0;
                        return DataType.Integer;
                    }
                    Output("格式错误，转换Int类型失败");
                    Value = null;
                    return DataType.Error;

                case DataType.Double:
                    if (dataString.TryParseDouble(out double dv0))
                    {
                        Value = dv0;
                        return DataType.Double;
                    }
                    Output("格式错误，转换Double类型失败");
                    Value = null;
                    return DataType.Error;

                case DataType.Boolean:
                    if (dataString.TryParseBoolean(out bool bv0))
                    {
                        Value = bv0;
                        return DataType.Boolean;
                    }
                    Output("格式错误，转换Boolean类型失败");
                    Value = null;
                    return DataType.Error;

                case DataType.Text:
                    Value = dataString;
                    return DataType.Text;

                case DataType.Function:
                    Value = dataString;
                    return DataType.Function;

                default:
                    Output("类型枚举出错！转化失败！");
                    Value = null;
                    return DataType.Error;
            }
        }

        #endregion 静态

        public Data(object value, DataType type)
        {
            Value = value;
            Type = type;
        }

        #region 方法

        public int ToInt() => (int)Value;

        public float ToFloat() => (float)Value;

        public double ToDouble() => (double)Value;

        public new string ToString() => Value.ToString();

        public bool ToBoolean() => (bool)Value;

        /// <summary>
        /// 此数据是否为空数据
        /// </summary>
        /// <returns></returns>
        public bool isEmpty() => Value == null;

        /// <summary>
        /// 此数据是否为错误数据
        /// </summary>
        /// <returns></returns>
        public bool isError() => Type == DataType.Error;

        /// <summary>
        /// 解析字符串，更新为解析数据
        /// </summary>
        /// <param name="dataString"></param>
        /// <param name="type"></param>
        /// <returns>知否解析成功</returns>
        public bool Parse(string dataString, DataType type = DataType.Unknown)
        {
            object v;
            Type = Parse(dataString, out v, type);
            if (isError())
            {
                Value = null;
                return false;
            }
            Value = v;
            return true;
        }

        /// <summary>
        /// 输出解析数据信息
        /// </summary>
        public void Print()
        {
            if (Value != null)
            {
                Output($"[{Type}]: {Value}");
            }
            else
            {
                Output($"[{Type}]:");
            }
        }

        #endregion 方法
    }
}