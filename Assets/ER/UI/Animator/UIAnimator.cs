using System.Collections.Generic;
using UnityEngine;

namespace ER.UI.Animator
{
    public class UIAnimator : MonoSingletonAutoCreate<UIAnimator>
    {
        private Dictionary<string, UIAnimationCD> cds = new Dictionary<string, UIAnimationCD>();//碟子字典

        private HashSet<UIAnimationCD> playing_cds = new HashSet<UIAnimationCD>();//正在播放的碟子

        private HashSet<UIAnimationCD> wait_cds = new HashSet<UIAnimationCD>();//停止播放的碟子

        private Dictionary<string, IUIPlayer> players = new Dictionary<string, IUIPlayer>();//播放器


        private List<UIAnimationCD> dest = new List<UIAnimationCD>();//待销毁的cd
        private List<UIAnimationCD> move = new List<UIAnimationCD>();//待移动回 wait_cds 的cd
        /// <summary>
        /// 获取一个CD的注册键
        /// </summary>
        /// <param name="rtf"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static string GetKey(UIAnimationCD cd)
        {
            return cd.Owner.GetHashCode().ToString()+":"+cd.Tag;
        }

        /// <summary>
        /// 将碟子注册入表
        /// </summary>
        /// <param name="cd"></param>
        public void Register(UIAnimationCD cd)
        {
            cds[GetKey(cd)] = cd;
            wait_cds.Add(cd);
        }

        /// <summary>
        /// 将制定碟子移到播放区
        /// </summary>
        /// <param name="cd"></param>
        public void StartPlay(UIAnimationCD cd)
        {
            if (!cds.ContainsValue(cd))
            {
                Debug.LogWarning($"该cd未注册无法直接播放:{cd.Tag}");
                return;
            }
            if (wait_cds.Contains(cd))
            {
                wait_cds.Remove(cd);
            }
            playing_cds.Add(cd);
        }
        /// <summary>
        /// 注销指定动画碟子
        /// </summary>
        /// <param name="cd_tag"></param>
        private void Unregister(UIAnimationCD cd)
        {
            cds.Remove(GetKey(cd));
        }
        /// <summary>
        /// 添加播放器
        /// </summary>
        /// <param name="player"></param>
        public void AddPlayer(IUIPlayer player)
        {
            players[player.Type] = player;
        }
        /// <summary>
        /// 移除播放器
        /// </summary>
        /// <param name="tag"></param>
        public void RemovePlayer(string tag)
        {
            players.Remove(tag);
        }
        /// <summary>
        /// 移除所有播放器
        /// </summary>
        public void ClearPlayer()
        {
            players.Clear();
        }


        private void LateUpdate()
        {
            dest.Clear();
            move.Clear();
            float delta = Time.deltaTime;

            Debug.Log("cd数:"+playing_cds.Count);
            foreach (var cd in playing_cds)
            {
                Debug.Log("正在播放:" + cd.Tag);
                if (players.TryGetValue(cd.Type, out IUIPlayer player))
                {
                    if (!player.Update(cd, delta)) return;
                    //已结束
                    if (cd.auto_destroy)
                        dest.Add(cd);
                    else
                        move.Add(cd);
                }
                else
                {
                    Debug.LogError($"未找到制定UI动画播放器:{cd.Type}");
                }
            }

            for(int i=0;i<dest.Count;i++)
            {
                Unregister(dest[i]);
                playing_cds.Remove(dest[i]);
            }
            for (int i = 0; i < move.Count; i++)
            {
                wait_cds.Add(move[i]);
                playing_cds.Remove(move[i]);
            }
        }
    }
}