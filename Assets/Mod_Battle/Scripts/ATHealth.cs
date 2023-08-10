using ER.Entity2D;

namespace Mod_Battle
{
    public class ATHealth:ATValue
    {
        public ATHealth()
        {
            AttributeName = nameof(ATHealth);
        }

        public override void Initialize()
        {
            ATActionResponse response = owner.GetAttribute<ATActionResponse>();
            if(response != null)
            {
                response.ActionEvent += GetDamage;
                print("添加委托成功");
            }
            else
            {
                owner.CreateDelegation("ATActionResponse", () => { response.ActionEvent += GetDamage; print("添加委托成功"); });
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
                    float dg = (int)damage;
                    ModifyValue(-dg, actionInfo.actor);
                }
            }
        }
    }
}