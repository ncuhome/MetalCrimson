using System;
using System.Collections.Generic;
using UnityEngine;
using static ER.Entity2D.ActionInfo;

namespace ER.Entity2D
{
    public class ActionInfo
    {
        public enum JudgeType
        { Single, Multiple }

        /// <summary>
        /// 判定次数类型
        /// </summary>
        public JudgeType judgeType = JudgeType.Single;

        /// <summary>
        /// 动作所属者
        /// </summary>
        public Entity actor = null;

        /// <summary>
        /// 动作类型
        /// </summary>
        public string type = "Unknown";

        /// <summary>
        /// 动作名称
        /// </summary>
        public string name = "Unknown";

        /// <summary>
        /// 动作持续时间，小于0表示无限
        /// </summary>
        public float time = 1;

        /// <summary>
        /// 剩余时间
        /// </summary>
        public float remainTime = 1;

        /// <summary>
        /// 判定次数，小于0表示无限
        /// </summary>
        public int hits = 1;

        /// <summary>
        /// 剩余判定次数
        /// </summary>
        public int remainHits = 1;

        /// <summary>
        /// 判定间隔
        /// </summary>
        public float hitCD = 1;

        /// <summary>
        /// 失效时是否销毁，如果不销毁自动隐藏
        /// </summary>
        public bool destroy = false;

        /// <summary>
        /// 动作的其他信息
        /// </summary>
        public Dictionary<string, object> infos = new();

        /// <summary>
        /// 动作发生位置
        /// </summary>
        public Vector2 position;

        /// <summary>
        /// 自身的深拷贝
        /// </summary>
        /// <returns></returns>
        public ActionInfo Copy()
        {
            ActionInfo info = new ActionInfo();
            info.actor = actor;
            info.time = time;
            info.remainTime = remainTime;
            Dictionary<string, object> _infos = new();
            foreach (KeyValuePair<string, object> pair in infos)
            {
                _infos[pair.Key] = pair.Value;
            }
            info.infos = _infos;
            return info;
        }
    }

    /// <summary>
    /// 动作区域特征，对区域检测特征的进一步封装，需要结合 ATActionResponse 使用
    /// </summary>
    public class ATActionRegion : MonoAttribute
    {
        /// <summary>
        /// 单元计时器
        /// </summary>
        private class Timer
        {
            public float time;
            public bool enter;
        }

        #region 初始化

        public ATActionRegion()
        {
            AttributeName = nameof(ATActionRegion);
        }

        public override void Initialize()
        {
            if (regionType == RegionType.Single)
            {
                if (region == null) { Debug.LogError("动作区域体 缺少绑定 单体区域体"); }
                regions = null;
                region.EnterEvent += Enter;
                region.ExitEvent += Exit;
            }
            else
            {
                if (regions == null) { Debug.LogError("动作区域体 缺少绑定 复合区域体"); }
                region = null;
                regions.OrAllEvent += Enter;
                regions.AndAllEvent += Enter;
                regions.NotAllEvent += Exit;
            }
        }

        #endregion 初始化

        #region 事件
        /// <summary>
        /// 预判定发生时触发的事件
        /// </summary>
        public event Action<ATActionResponse> HitPreEvent;
        /// <summary>
        /// 判定成功时触发的事件
        /// </summary>
        public event Action<ATActionResponse> HitEvent;

        /// <summary>
        /// 区域消失时触发的事件
        /// </summary>
        public event Action EndEvent;

        #endregion 事件

        #region 属性

        /// <summary>
        /// 缓存区，判定生效需要在下一帧执行
        /// </summary>
        private List<ATActionResponse> temp = new();

        [SerializeField]
        [Tooltip("复合判定区域")]
        private ATMultiRegion regions;

        [SerializeField]
        [Tooltip("判定区域")]
        private ATRegion region;

        [SerializeField]
        [Tooltip("判定区域类型")]
        private RegionType regionType = RegionType.Single;

        /// <summary>
        /// 计时器
        /// </summary>
        private Dictionary<ATActionResponse, Timer> timers = new();

        #region 动作属性

        /// <summary>
        /// 动作所属者
        /// </summary>
        public Entity actor = null;

        /// <summary>
        /// 动作类型
        /// </summary>
        public string actionType = "Unknown";

        /// <summary>
        /// 动作名称
        /// </summary>
        public string actionName = "Unknown";

        /// <summary>
        /// 动作持续时间，小于0表示无限
        /// </summary>
        public float time = 1;

        /// <summary>
        /// 剩余时间
        /// </summary>
        public float remainTime = 1;

        /// <summary>
        /// 判定次数类型
        /// </summary>
        public JudgeType judgeType = JudgeType.Single;

        /// <summary>
        /// 判定次数，小于0表示无限
        /// </summary>
        [SerializeField]
        public int hits = 1;

        /// <summary>
        /// 剩余判定次数
        /// </summary>
        public int remainHits = 1;

        /// <summary>
        /// 判定间隔
        /// </summary>
        public float hitCD = 1;

        /// <summary>
        /// 失效时是否销毁，如果不销毁自动隐藏
        /// </summary>
        public bool destroy = false;

        /// <summary>
        /// 动作的其他信息
        /// </summary>
        public Dictionary<string, object> infos = new();

        #endregion 动作属性

        /// <summary>
        /// 判定区域类型
        /// </summary>
        public enum RegionType
        { Single, Multiple }

        /// <summary>
        /// 判定区域类型
        /// </summary>
        public RegionType _RegionType { get => regionType; }

        /// <summary>
        /// 动作信息
        /// </summary>
        public ActionInfo Info
        {
            get
            {
                Dictionary<string, object> _infos = new();
                foreach (KeyValuePair<string, object> pair in infos)
                {
                    _infos[pair.Key] = pair.Value;
                }
                ActionInfo info = new ActionInfo
                {
                    actor = actor,
                    type = actionType,
                    name = actionName,
                    time = time,
                    remainTime = remainTime,
                    judgeType = judgeType,
                    hits = hits,
                    remainHits = remainHits,
                    hitCD = hitCD,
                    position = region.transform.position,
                    destroy = destroy,
                    infos = _infos
                };
                return info;
            }
            set
            {
                ActionInfo info = value;
                actor = info.actor;
                actionType = info.type;
                actionName = info.name;
                time = info.time;
                remainTime = info.remainTime;
                judgeType = info.judgeType;
                hits = info.hits;
                remainHits = info.remainHits;
                hitCD = info.hitCD;
                destroy = info.destroy;
                infos = info.infos;
            }
        }

        #endregion 属性

        #region 区域检测

        public void Reset()
        {
            //Debug.Log($"攻击次数:{hits}, 剩余:{remainHits}");
            remainHits = hits;
            remainTime = time;
            timers = new();
            temp = new();
            gameObject.SetActive(true);
        }

        /// <summary>
        /// 设置为单体区域判定
        /// </summary>
        /// <param name="region">区域对象</param>
        public void SetSingleRegion(ATRegion region)
        {
            this.region = region;
            if (regions != null)
            {
                regions.OrAllEvent -= Enter;
                regions.AndAllEvent -= Enter;
                regions.NotAllEvent -= Exit;
                regions = null;
            }
            regionType = RegionType.Single;
            region.EnterEvent += Enter;
            region.ExitEvent += Exit;
        }

        /// <summary>
        /// 设置为复合区域判定
        /// </summary>
        /// <param name="regions">复合区域对象</param>
        public void SetMultipleRegion(ATMultiRegion regions)
        {
            this.regions = regions;
            if (region != null)
            {
                region.EnterEvent -= Enter;
                region.ExitEvent -= Exit;
                region = null;
            }
            regionType = RegionType.Multiple;
            regions.OrAllEvent += Enter;
            regions.AndAllEvent += Enter;
            regions.NotAllEvent += Exit;
        }

        private void Action(ATActionResponse response)
        {
            HitEvent?.Invoke(response);
            response.ActionResponse(Info);//响应此动作
            //Debug.Log($"攻击次数:{hits}, 剩余:{remainHits}");
            if (hits > 0)
            {
                remainHits--;
                if (remainHits <= 0)
                {
                    End();
                }
            }
        }

        /// <summary>
        /// 进入判定区域
        /// </summary>
        /// <param name="collision"></param>
        private void Enter(Collider2D collision)
        {
            ATActionResponse response = collision.gameObject.GetComponent<ATActionResponse>();
            //Debug.Log($"动作接触:{actionName}  对象:{response != null}, {collision.gameObject.name}");
            if (response != null)//必须是 response 封装的Collider才算有效判定
            {
                if (timers.ContainsKey(response))//非初次接触对象
                {
                    timers[response].enter = true;
                }
                else//初次接触对象
                {
                    temp.Add(response);//添加至缓存区
                    timers[response] = new Timer() { time = 0, enter = true };//添加计时器
                }
            }
        }

        /// <summary>
        /// 离开判定区域
        /// </summary>
        /// <param name="collision"></param>
        private void Exit(Collider2D collision)
        {
            ATActionResponse response = collision.gameObject.GetComponent<ATActionResponse>();
            if (response != null)
            {
                if (timers.ContainsKey(response))//非初次接触对象
                {
                    timers[response].enter = false;
                }
                else//初次接触对象
                {
                    temp.Add(response);//添加至缓存区
                    timers[response] = new Timer() { time = 0, enter = true };//添加计时器
                }
            }
        }

        /// <summary>
        /// 区域生命周期结束
        /// </summary>
        private void End()
        {
            if (EndEvent != null) EndEvent();
            if (destroy)
            {
                Destroy();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// 执行缓存区
        /// </summary>
        private void UpdateTemp()
        {
            for (int i = 0; i < temp.Count; i++)
            {
                if (temp[i] == null)
                {
                    continue;
                }
                Debug.Log($"动作预响应: {temp[i].gameObject.name}");
                HitPreEvent?.Invoke(temp[i]);
                if (temp[i].PreResponse(Info))
                {
                    Debug.Log("动作终止");
                    End();
                    return;
                }
            }
            for (int i = 0; i < temp.Count; i++)
            {
                Action(temp[i]);
            }
            temp.Clear();
        }

        #endregion 区域检测

        #region Unity

        private void Update()
        {
            if (time > 0)
            {
                remainTime -= Time.deltaTime;
                if (remainTime <= 0)
                {
                    End();
                }//超时销毁自身
            }
            foreach (var timer in timers)
            {
                timer.Value.time += Time.deltaTime;
                if (timer.Value.enter && timer.Value.time >= hitCD)//目标处于判断区域内且超过冷却CD
                {
                    //响应动作，并重置计时器
                    temp.Add(timer.Key);
                    timer.Value.time = 0;
                }
            }
        }

        private void LateUpdate()
        {
            UpdateTemp();
        }

        #endregion Unity
    }
}