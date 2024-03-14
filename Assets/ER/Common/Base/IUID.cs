// Ignore Spelling: Deserialize UID obj uuid IUID

using System;
using System.Collections.Generic;

namespace ER
{
    /// <summary>
    /// uid标记
    /// </summary>
    public class UID
    {
        /// <summary>
        /// 类型名称
        /// </summary>
        private string className;

        /// <summary>
        /// 对象hashcode, 注意这个值可能不等于对象实际的 hashcode
        /// </summary>
        private int hashCode;

        /// <summary>
        /// 对象时间戳, 注意这个值可能不等于对象实际创建时的时间戳
        /// </summary>
        private long timeCode;

        public string ClassName => className;
        public int HashCode=>hashCode;
        public long TimeCode => timeCode;

        public static bool operator ==(UID left, UID right)
        {
            if (left.ClassName != right.ClassName) return false;
            if (left.HashCode != right.HashCode) return false;
            return left.TimeCode == right.TimeCode;
        }

        public static bool operator !=(UID left, UID right)
        {
            return !(left == right);
        }

        public UID(string className, int hashCode, long timeCode = -1)
        {
            this.className = className;
            this.hashCode = hashCode;
            if (timeCode < 0)
            {
                this.timeCode = ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeMilliseconds();
            }
            else 
            {
                this.timeCode = timeCode;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uid_str">uid字符串</param>
        public UID(string uid_str)
        {
            string[] parts = uid_str.Split(':');
            if(parts.Length != 3)
            {
                throw new Exception($"非法 uuid 字符串:{uid_str}, 无法解析为 UID 对象");
            }
            className = parts[0];
            hashCode = int.Parse(parts[1]);
            timeCode = long.Parse(parts[2]);
        }
        public static UID Parse(string uid_str)
        {
            return new UID(uid_str);
        }
        public override string ToString()
        {
            return $"{className}:{hashCode}:{timeCode}";
        }
        public override int GetHashCode() { return base.GetHashCode(); }
        public override bool Equals(object obj) { return base.Equals(obj); }
    }
    /// <summary>
    /// 持久化存储数据信息
    /// </summary>
    public struct ObjectUIDInfo
    {
        /// <summary>
        /// 对象占用的uuid
        /// </summary>
        public string uuid;
        /// <summary>
        /// 其他数据
        /// </summary>
        public Dictionary<string, object> data;
        /// <summary>
        /// 判断是否为空信息
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return uuid == null && data == null;
        }
        /// <summary>
        /// 一个空的无效的信息
        /// </summary>
        public static ObjectUIDInfo Empty => new ObjectUIDInfo() { uuid=null,data=null};

        public ObjectUIDInfo(string _uuid)
        {
            uuid = _uuid;
            data = new Dictionary<string, object>();
        }
        public ObjectUIDInfo(UID uid)
        {
            uuid = uid.ToString();
            data = new Dictionary<string, object>();
        }

    }

    /// <summary>
    /// 唯一标识符接口:
    /// 标识符规则:
    /// 类型标识:对象哈希值:创建对象时的时间戳
    /// 
    /// - 如果你的对象需要使用 uid 进行管理请实现该接口
    /// - 你可以使用 this.Registry() 将自身对象注册进 UID管理器 (拓展方法)
    /// </summary>
    public interface IUID
    {
        /// <summary>
        /// 类型名称(用于uid管理)
        /// </summary>
        public string ClassName { get; }
        /// <summary>
        /// 唯一标识符
        /// </summary>
        public UID UUID { get; }
        /// <summary>
        /// 获取该对象序列化文本
        /// </summary>
        /// <returns></returns>
        public ObjectUIDInfo Serialize();
        /// <summary>
        /// 根据文本反序列化
        /// </summary>
        /// <param name="data"></param>
        public void Deserialize(ObjectUIDInfo data);
    }
}