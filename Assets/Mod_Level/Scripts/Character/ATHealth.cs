using ER.Entity2D;
using System;

namespace Mod_Level
{
    public class ATHealth : ATValue
    {
        private ATCharacterState state;
        private ATBuffManager buffManager;

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

            owner.TryGetAttribute("ATBuffManager", ref buffManager, (IAttribute attribute) =>
            {
                buffManager = (ATBuffManager)attribute;
            });
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
                    ModifyValue(-dg, actionInfo.actor);
                    BFRigidity bf = new BFRigidity();
                    bf.defTime = 1f;

                    buffManager.Add(bf);
                }
            }
        }
    }
}