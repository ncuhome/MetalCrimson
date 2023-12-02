using System.Collections;
using UnityEngine;

namespace Mod_Level
{
    /// <summary>
    /// 玩家属性
    /// </summary>
    public class ATPlayerState : ATCharacterState
    {
        #region 初始化

        public ATPlayerState()
        { AttributeName = nameof(ATPlayerState); }

        public override void Initialize()
        {
            base.Initialize();
        }

        #endregion 初始化



        /// <summary>
        /// 架势类型
        /// </summary>
        public enum Posture
        { Up, Front, Down }

        /// <summary>
        /// 交互状态
        /// </summary>
        public enum InteractState
        {
            /// <summary>
            /// 无交互
            /// </summary>
            None,

            /// <summary>
            /// 请求响应
            /// </summary>
            Wait,

            /// <summary>
            /// 正在交互
            /// </summary>
            Interacting,

            /// <summary>
            /// 交互结束
            /// </summary>
            Cancel,
        }


        private Posture posture = Posture.Front;

        private Coroutine stopTag;//协程标记(用于关闭协程)
        /// <summary>
        /// 交互状态
        /// </summary>
        public InteractState interact = InteractState.None;
        public Posture ActPosture
        {
            get => posture;
            set
            {
                posture = value;
                if (stopTag != null)
                    StopCoroutine(stopTag);
                switch (value)
                {
                    case Posture.Up:
                        stopTag = StartCoroutine(PostureChange(animator.GetFloat("posture"), 3));
                        break;

                    case Posture.Front:
                        stopTag = StartCoroutine(PostureChange(animator.GetFloat("posture"), 2));
                        break;

                    case Posture.Down:
                        stopTag = StartCoroutine(PostureChange(animator.GetFloat("posture"), 1));
                        break;

                    default:
                        break;
                }
            }
        }
        private IEnumerator PostureChange(float start, float end)
        {
            float timer = 0;
            while (true)
            {
                timer += Time.deltaTime * postureSpeed;
                animator.SetFloat("posture", Mathf.Lerp(start, end, timer));
                yield return 0;

                if (timer >= 1) yield break;
            }
        }
    }
}