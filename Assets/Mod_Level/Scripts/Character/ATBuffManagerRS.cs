using ER.Entity2D;
using System.Collections.Generic;
using UnityEngine;

namespace Mod_Level
{
    public class ATBuffManagerRS : ATBuffManager
    {
        public override void Initialize()
        {
            base.Initialize();
            ATActionResponse response = owner.GetAttribute<ATActionResponse>();
            if (response == null)
            {
                Debug.Log("未获取动作响应器!");
            }
            response.ActionEvent += GetBuff;
        }

        private void GetBuff(ActionInfo actionInfo)
        {
            int count = 0;
            if (actionInfo.infos.TryGetValue("buff_count", out object v))
            {
                count = (int)v;
            }
            if (count > 0)
            {
                Debug.Log($"参数类型:{actionInfo.infos["buff_ads"] != null}, {(actionInfo.infos["buff_ads"]).GetType().Name}");
                BuffSetInfo[] infos = ((List<BuffSetInfo>)actionInfo.infos["buff_ads"]).ToArray();
                foreach (BuffSetInfo info in infos)//遍历添加附加buff
                {
                    AddBuff(info);
                }
            }
        }

        private void AddBuff(BuffSetInfo setInfo)
        {
            MDBuff buff = null;
            switch (setInfo.buffName)
            {
                case "Damage":
                    buff = new BFDamage(setInfo);
                    break;

                case "DamagePercent":
                    buff = new BFDamagePercent(setInfo);
                    break;

                case "Heal":
                    buff = new BFHeal(setInfo);
                    break;

                case "HealPercent":
                    buff = new BFHealPercent(setInfo);
                    break;

                case "Mire":
                    buff = new BFMire(setInfo);
                    break;

                case "Rigidity":
                    buff = new BFRigidity(setInfo);
                    break;

                case "SuperArmor":
                    buff = new BFSuperArmor(setInfo);
                    break;

                case "Vertigo":
                    buff = (new BFVertigo(setInfo));
                    break;
            }
            if (buff == null) return;
            buff.ResetTime();
            Add(buff);
        }
    }
}