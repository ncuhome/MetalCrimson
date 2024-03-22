using ER;
using System.Collections.Generic;
using UnityEngine;

namespace Mod_Forge
{

    public class PGroup : Water
    {
        private List<Water> water = new List<Water>();

        public bool IsFull()
        {
            return  water.Count>=ComponentMaker.lay_max;
        }

        /// <summary>
        /// 向该容器添加元素
        /// </summary>
        /// <param name="w"></param>
        public void Add(Water w)
        {
            Debug.Log("添加新元素");
            if (IsFull()) return;
            water.Add(w);
            w.transform.SetParent(transform);
        }
        public void Clear()
        {
            for(int i = 0; i < water.Count; i++)
            {
                water[i].Destroy();
            }
            water.Clear();
        }

        public Water this[int index]
        {
            get=> water[index];
        }

        public override void ResetState()
        {
            Clear();
        }

        protected override void OnHide()
        {
            Clear();
        }

        private void Awake()
        {
            ResetState();
        }
    }
}