using ER.Entity2D;
using System;
using UnityEngine;

namespace Mod_Level
{
    public class BoxEntity:Entity
    {
        [SerializeField]
        private float HP = 1;


        protected override void Initialized()
        {
            GetAttribute<ATActionResponse>().ActionEvent += delegate (ActionInfo info)
            {
                if(info.type == "Attack")
                {
                    if(info.infos.TryGetValue("damage",out object value))
                    {
                        HP-=(float)value;
                        if(HP<=0)
                        {
                            Destroy();
                        }
                    }
                }
            };
        }

        private void Destroy()
        {
            Destroy(gameObject);
        }
    }
}