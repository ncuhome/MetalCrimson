using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ER.Entity
{
    public abstract class DynamicAttribute : MonoBehaviour,IAttribute
    {
        #region 字段
        protected string attributeName = "动态属性";
        protected Entity owner;
        #endregion

        #region 属性
        public virtual string Name { get => attributeName; set => attributeName = value; }
        public virtual Entity Owner { get => owner; set => owner = value; }
        #endregion

        #region 功能函数
        public virtual void Destroy() 
        {
            Destroy(this);
        }

        public abstract void Initialization();
        #endregion
    }
}
