using ER.Entity2D;
using UnityEngine;

namespace Mod_Level.Attributes
{
    /// <summary>
    /// 接触时死亡:
    /// 依赖: ATCanDestroyed, ATRegion
    /// </summary>
    public class ATOnCatchDead : MonoAttribute
    {
        public ATOnCatchDead() { AttributeName = nameof(ATOnCatchDead); }
        private ATCanDestroyed canDestroyed;
        private ATRegion region;
        public ATRegion Region => region;
        public override void Initialize()
        {
            canDestroyed = owner.GetAttribute<ATCanDestroyed>();
            region = owner.GetAttribute<ATRegion>();
            region.EnterEvent += Die;
        }
        public void Die(Collider2D c)
        {
            if (canDestroyed.Destroyed) return;
            canDestroyed.DestroySelf();
        }

    }
}


