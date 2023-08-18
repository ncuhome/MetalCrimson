using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace ER.Entity2D
{
    public class ATRepel : MonoAttribute
    {
        public enum RepelMode 
        { 
            /// <summary>
            /// 不启用击退
            /// </summary>
            Off,
            /// <summary>
            /// 自动确定击退方向
            /// </summary>
            AutoDirection,
            /// <summary>
            /// 自定义击退方向
            /// </summary>
            CustomDirection,
        }

        #region 初始化
        public ATRepel() { AttributeName = nameof(ATRepel); }
        public override void Initialize()
        {
            Action<IAttribute> action = (IAttribute attribute) =>
            {
                ((ATActionResponse)attribute).ActionEvent += Repeled;
            };

            ATActionResponse response = owner.GetAttribute<ATActionResponse>();
            if(response == null)
            {
                owner.CreateDelegation("ATActionResponse",action);
            }
            else
            {
                action(response);
            }
        }
        #endregion

        #region 功能
        private void Repeled(ActionInfo info)
        {
            RepelMode mode = RepelMode.Off;
            if (info.infos.TryGetValue("repel_mode", out object _mode))
            {
                mode = (RepelMode)_mode;
            }
            if(mode!= RepelMode.Off)
            {
                float power = 0;
                Vector2 dir = Vector2.zero;
                if (info.infos.TryGetValue("repel_power", out object pwoer))
                {
                    power = (float)pwoer;
                }
                switch(mode)
                {
                    case RepelMode.AutoDirection:
                        dir = ((Vector2)owner.transform.position - info.position).normalized;
                        break;
                    case RepelMode.CustomDirection:

                        if (info.infos.TryGetValue("repel_dir", out object direction))
                        {
                            dir = ((Vector2)direction).normalized;
                        }
                        break;
                }
                owner.GetComponent<Rigidbody2D>().velocity += dir * power;
            }
        }
        #endregion
    }
}