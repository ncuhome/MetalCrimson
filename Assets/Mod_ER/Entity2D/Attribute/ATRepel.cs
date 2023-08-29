using System;
using UnityEngine;

namespace ER.Entity2D
{
    /// <summary>
    /// 击退箱子, 挂载在实体上, 即可接受击退动作;
    /// 特有参数: repel_mode(ATRepel.RepelMode):击退类型; repel_power(float):击退力度; repel_dir(Vector2):击退方向(仅在 repel_mode 为 CustomDirection 时有效)
    /// </summary>
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

        public ATRepel()
        { AttributeName = nameof(ATRepel); }

        public override void Initialize()
        {
            Action<IAttribute> action = (IAttribute attribute) =>
            {
                ((ATActionResponse)attribute).ActionEvent += Repeled;
            };

            ATActionResponse response = owner.GetAttribute<ATActionResponse>();
            if (response == null)
            {
                owner.CreateDelegation("ATActionResponse", action);
            }
            else
            {
                action(response);
            }
        }

        #endregion 初始化

        #region 功能

        private void Repeled(ActionInfo info)
        {
            RepelMode mode = RepelMode.Off;
            if (info.infos.TryGetValue("repel_mode", out object _mode))
            {
                mode = (RepelMode)_mode;
            }
            if (mode != RepelMode.Off)
            {
                float power = 0;
                Vector2 dir = Vector2.zero;
                if (info.infos.TryGetValue("repel_power", out object pwoer))
                {
                    power = (float)pwoer;
                }
                switch (mode)
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

        #endregion 功能
    }
}