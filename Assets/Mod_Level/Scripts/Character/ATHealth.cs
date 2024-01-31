﻿using ER.Entity2D;
using System;
using UnityEngine;

namespace Mod_Level
{
    public class ATHealth : ATValue
    {
        private ATCharacterState state;
        [Tooltip("角色主贴图父物体")]
        [SerializeField]
        private RedFlashing charactor;

        public ATHealth()
        {
            AttributeName = nameof(ATHealth);
        }

        public override void Initialize()
        {
            Action<IAttribute> action = (IAttribute response) =>
            {
                ((ATActionResponse)response).ActionEvent += GetDamage;
                //print("添加委托成功");
            };
            ATActionResponse response = null;
            if (owner.TryGetAttribute("ATActionResponse", ref response, action))
            {
                response.ActionEvent += GetDamage;
                //print("添加委托成功");
            }

            state = owner.GetAttribute<ATCharacterState>();


        }

        private void GetDamage(ActionInfo actionInfo)
        {
            //print($"接受判定:{actionInfo.type}");
            if (actionInfo.type == "Attack")
            {
                //print("接受攻击");
                if (actionInfo.infos.TryGetValue("damage", out var damage))
                {
                    float dg = (float)damage;
                    dg *= state["DefenceMultiply"];
                    if (state["Defence"] > 0)
                    {
                        dg -= state["Defence"];
                    }
                    ModifyValue(-dg, actionInfo.actor);
                    if(charactor!= null)
                        charactor.Flash();
                }
            }
        }
    }
}