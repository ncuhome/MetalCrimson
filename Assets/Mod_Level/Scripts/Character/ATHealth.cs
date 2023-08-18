using ER.Entity2D;
using System;

namespace Mod_Level
{
    public class ATHealth:ATValue
    {
        public ATHealth()
        {
            AttributeName = nameof(ATHealth);
        }

        public override void Initialize()
        {
            Action<IAttribute> action = (IAttribute response) =>
            {
                ((ATActionResponse)response).ActionEvent += GetDamage;
                print("添加委托成功");
            };
            ATActionResponse response = owner.GetAttribute<ATActionResponse>();
            if(response != null)
            {
                action(response);
            }
            else
            {
                owner.CreateDelegation("ATActionResponse", action);
            }
        }

        private void GetDamage(ActionInfo actionInfo)
        {
            print($"接受判定:{actionInfo.type}");
            if(actionInfo.type == "Attack")
            {
                print("接受攻击");
                if (actionInfo.infos.TryGetValue("damage",out var damage))
                {
                    float dg = (float)damage;
                    ModifyValue(-dg, actionInfo.actor);
                }
            }
        }
    }
}