using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Common
{
    /// <summary>
    /// 虚拟锚点
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
        public Vector2 Point { get;set; }
        /// <summary>
        /// 销毁函数
        /// </summary>
        public void Destroy();
    }

    public class VirtAnchor : Anchor
    {
        private string tag;
        private Vector2 point;

        public string AnchorTag { get => tag; set => tag = value; }
        public Vector2 Point { get => point; set => point = value; }

        public VirtAnchor() { tag = "Unkown";point = Vector2.zero; }
        public VirtAnchor(float x, float y) { tag = "unkown";point = new Vector2(x, y); }
        public VirtAnchor(string tag) { this.tag = tag;  point = Vector2.zero; }
        public VirtAnchor(string tag,float x, float y) { this.tag = tag; point = new Vector2(x, y); }
        public void Destroy() { }
    }

    public class MonoAnchor : MonoBehaviour, Anchor
    {
        public string Ptag;
        public string AnchorTag { get => Ptag; set => Ptag = value; }
        public Vector2 Point { get => transform.position; set => transform.position = value; }

        public void Destroy()
        { 
            Destroy(gameObject);
        }

        private void Awake()
        {
            AnchorManager.Instance.AddAnchor(this);
        }

    }
}
