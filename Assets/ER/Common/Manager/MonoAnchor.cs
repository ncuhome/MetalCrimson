using UnityEngine;

namespace ER
{
    /// <summary>
    /// 锚点
    /// </summary>
    public interface Anchor
    {
        /// <summary>
        /// 锚点标签
        /// </summary>
        public string AnchorTag { get; set; }

        /// <summary>
        /// 锚点位置
        /// </summary>
        public Vector3 Point { get; set; }

        /// <summary>
        /// 锚点的所有者
        /// </summary>
        public object Owner { get; }

        /// <summary>
        /// 销毁函数
        /// </summary>
        public void Destroy();
    }

    /// <summary>
    /// 虚拟钩子
    /// </summary>
    public class VirtualAnchor : Anchor
    {
        private string tag;
        private Vector3 point;
        private object owner;
        public object Owner { get => owner; set => owner = value; }

        public string AnchorTag { get => tag; set => tag = value; }
        public Vector3 Point { get => point; set => point = value; }

        public VirtualAnchor(float x = 0, float y = 0, float z = 0)
        { tag = "Unknown"; point = new Vector3(x, y, z); }

        public VirtualAnchor(string tag, float x = 0, float y = 0, float z = 0)
        { this.tag = tag; point = new Vector3(x, y, z); }

        public void Destroy()
        { }
    }

    /// <summary>
    /// 组件钩子
    /// </summary>
    public class MonoAnchor : MonoBehaviour, Anchor
    {
        public string _tag;
        public string AnchorTag { get => _tag; set => _tag = value; }
        public Vector3 Point { get => transform.position; set => transform.position = value; }

        public object Owner { get => gameObject; }

        public void Destroy()
        {
            Destroy(this);
        }

        private void Awake()
        {
            AM.AddAnchor(this);
        }
    }
}