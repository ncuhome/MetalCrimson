using ER.Entity2D;
using System.Globalization;
using UnityEngine;

namespace Mod_Level
{
    /// <summary>
    /// 可交互物体
    /// </summary>
    public class InteractObject : MonoBehaviour, IInteractive
    {
        [SerializeField]
        private ATRegion region;

        private void Awake()
        {
            region.EnterEvent += OnPlayerDisplay;
            region.ExitEvent += ExitPlayerDisappear;
        }
        /// <summary>
        /// 玩家靠近时显示UI
        /// </summary>
        /// <param name="collider"></param>
        protected void OnPlayerDisplay(Collider2D collider)
        {
            Entity entity = collider.GetComponent<Entity>();
            if (entity == null) return;
            if(entity.gameObject.tag == GameTagText.L_PLAYER)//如果是玩家的话
            {
                
            }
        }

        /// <summary>
        /// 玩家离开时关闭UI
        /// </summary>
        /// <param name="collider"></param>
        protected void ExitPlayerDisappear(Collider2D collider)
        {

        }
        public void EnterInteract()
        {
            throw new System.NotImplementedException();
        }

        public void ExitInteract()
        {
            throw new System.NotImplementedException();
        }


    }
}