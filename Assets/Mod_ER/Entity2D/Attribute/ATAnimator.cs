using UnityEngine;

namespace ER.Entity2D
{
    public class ATAnimator : MonoAttribute
    {
        private Animator animator;
        public Animator Animator { get { return animator; } }
        public ATAnimator() { AttributeName = nameof(ATAnimator); }
        public override void Initialize()
        {
            animator = GetComponent<Animator>();
        }
    }
}