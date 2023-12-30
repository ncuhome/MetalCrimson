using ER.Entity2D;
using System;
using UnityEngine;

namespace Mod_Level
{
    /// <summary>
    /// 耐性
    /// </summary>
    public class ATStamina : ATValue
    {
        private ATCharacterState state;
        public ATStamina()
        {
            AttributeName = nameof(ATStamina);
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
                if (actionInfo.infos.TryGetValue("damage_stamina", out var damage))
                {
                    float dg = (float)damage;
                    dg -= state["Tenacity"]/2;
                    ModifyValue(-dg, actionInfo.actor);
                }
            }
        }
    }
}