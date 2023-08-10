using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace States
{
    /// <summary>
    /// 状态系统管理器
    /// </summary>
    public class StateSystemManager : MonoBehaviour
    {
        #region 单例封装
        private static StateSystemManager instance;
        public static StateSystemManager Instance
        {
            get { return instance; }
        }
        #endregion 单例封装

        #region  属性
        private Dictionary<string, StateSystem> stateSystems = new Dictionary<string, StateSystem>();

        /// <summary>
        /// 状态系统列表
        /// </summary>
        public Dictionary<string, StateSystem> StateSystems { get { return stateSystems; } }

        #endregion 属性

        #region 方法

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        /// <summary>
        /// 执行每个状态中的Update部分
        /// </summary>
        void Update()
        {
            foreach (var stateSystem in stateSystems.Values)
            {
                stateSystem.currentState.OnState();
            }
        }
        /// <summary>
        /// 创建新的状态系统
        /// </summary>
        /// <param name="name"></param>
        /// <param name="size"></param>
        public void CreateStateSystem(string name, int size = 64)
        {
            StateSystem stateSystem = new StateSystem(name, size);
            StateSystems[name] = stateSystem;
        }

        /// <summary>
        /// 删除指定状态系统
        /// </summary>
        /// <param name="name"></param>
        public void DeleteStateSystem(string name)
        {
            if (stateSystems.ContainsKey(name))
            {
                stateSystems.Remove(name);
            }
        }
        /// <summary>
        /// 判断指定系统是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Exist(string name)
        {
            if (stateSystems.ContainsKey(name))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取指定状态仓库
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public StateSystem this[string name]
        {
            get => stateSystems[name];
        }

        #endregion 方法
    }
}
