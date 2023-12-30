using ER.Entity2D;
using UnityEngine;

namespace Mod_Level
{
    /// <summary>
    /// 沼泽块
    /// </summary>
    public class Mire:MonoBehaviour
    {

        [Tooltip("毒药效果")]
        public bool poison = false;
        public float poisonDamage = 10f;
        private float timer = 0.5f;
        public float resetTime = 0.5f;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Entity entity = collision.GetComponent<Entity>();
            if (entity == null) return;
            ATBuffManager buffManager = entity.GetAttribute<ATBuffManager>();
            if(buffManager == null)return;
            BFMire bFMire = new BFMire();
            bFMire.defTime = -1;
            bFMire.resetTime = resetTime;
            bFMire.poison = poison;
            bFMire.poisonDamage = poisonDamage;
            buffManager.Add(bFMire);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            Entity entity = collision.GetComponent<Entity>();
            if (entity == null) return;
            ATBuffManager buffManager = entity.GetAttribute<ATBuffManager>();
            if (buffManager == null) return;
            buffManager.Remove("Mire");
        }
    }
}