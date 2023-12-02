using ER.Entity2D;
using ER.UI;
using System;
using UnityEngine;

namespace Mod_Level
{
    /// <summary>
    /// 对受击响应的拓展
    /// </summary>
    public class ActionResponseEX:MonoAttribute
    {

        private RigidbodyFollowCamera camera;
        public override void Initialize()
        {
            ATActionResponse response = owner.GetAttribute<ATActionResponse>();
            if (response == null) { Debug.Log("获取动作响应器失败!");return; }

            camera = Camera.main.GetComponent<RigidbodyFollowCamera>();
            response.ActionEvent += GetCameraEffect;
        }
        private void GetCameraEffect(ActionInfo actionInfo)
        {
            float power = -1;
            float time = -1;
            if(actionInfo.infos.TryGetValue("camera_shake_power",out object v))
            {
                power = (float)v;
            }
            if (actionInfo.infos.TryGetValue("camera_shake_time", out object v2))
            {
                time = (float)v2;
            }
            if(power > 0 && time >0 )
            {
                camera.Shake(power,time);
            }
        }



    }
}
