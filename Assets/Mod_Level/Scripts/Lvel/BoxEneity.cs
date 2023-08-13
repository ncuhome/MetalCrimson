using ER.Entity2D;
using UnityEngine;

namespace Mod_Level
{
    public class BoxEntity:Entity
    {
        [SerializeField]
        private float HP = 100;

        public void Awake()
        {
            CreateDelegation("ATActionResponse",Init);
        }

        private void Init()
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
                            Destroy(gameObject);
                        }
                    }
                }
            };
        }
    }
}