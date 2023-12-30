using ER.Entity2D;
using System;
using UnityEngine;

namespace Mod_Level
{
    public class ATRepelBox : ATRepel
    {
        private ATCharacterState state;
        public override void Initialize()
        {
            Action<IAttribute> action = (IAttribute attribute) =>
            {
                ((ATActionResponse)attribute).ActionEvent += GetRepel;
            };

            ATActionResponse response = owner.GetAttribute<ATActionResponse>();
            state = owner.GetAttribute<ATCharacterState>();
            if (response == null)
            {
                owner.CreateDelegation("ATActionResponse", action);
            }
            else
            {
                action(response);
            }
        }
        private void GetRepel(ActionInfo info)
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
                if (power > state["Weight"])
                {
                    float corect = MathF.Abs(state["Weight"] - power);
                    power = 0.0002f * corect * corect + 0.03f * corect + 2f;
                }
                else
                {
                    power = 1.5f;
                }
                owner.GetComponent<Rigidbody2D>().velocity += dir * power;
            }
        }
    }
}