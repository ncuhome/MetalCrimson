using ER.Entity2D;
using ER.UI;
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
                    if(owner.CompareTag(GameTagText.L_PLAYER))
                    {
                        Debug.Log($"伤害量:{dg},阈值:{Max / 3},{Value / 2}");
                        if (dg >= Max / 3 || dg >= Value / 2)
                        {
                            Debug.Log("震动摄像头:强");
                            RigidbodyFollowCamera.Instance.Shake(1.2f, 0.2f);
                        }
                        else
                        {
                            Debug.Log("震动摄像头:弱");
                            RigidbodyFollowCamera.Instance.Shake(0.2f, 0.4f);
                        }
                    }
                    
                    ModifyValue(-dg, actionInfo.actor);
                    if(charactor!= null)
                        charactor.Flash();
                }
            }
        }
    }
}