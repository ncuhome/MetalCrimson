using ER.Entity2D;
using UnityEngine;

namespace Mod_Level
{
    /// <summary>
    /// 沼泽块
    /// </summary>
    public class Mire:MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Entity entity = collision.GetComponent<Entity>();
            if (entity == null) return;
            ATBuffManager buffManager = entity.GetAttribute<ATBuffManager>();
            if(buffManager == null)return;
            buffManager.Add(new BFMire());
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            Entity entity = collision.GetComponent<Entity>();
            if (entity == null) return;
            ATBuffManager buffManager = entity.GetAttribute<ATBuffManager>();
            if (buffManager == null) return;
        }
    }
}