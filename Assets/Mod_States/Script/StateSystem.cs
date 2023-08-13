
using System;
using System.Collections.Generic;
using UnityEngine;
namespace States
{
    [Serializable]
    public class StateSystem
    {
        #region 属性
        private int size;
        /// <summary>
        /// 状态系统名称
        /// </summary>
        public string systemName;
        /// <summary>
        /// 状态系统容量
        /// </summary>
        public int Size
        {
            get => size;
            set
            {
                size = value;
                if (size < 0) size = 0;
            }
        }
        /// <summary>
        /// 状态ID与对应状态的词典
        /// </summary>
        public Dictionary<int, State> states = new();
        /// <summary>
        /// 当前状态
        /// </summary>
        public State currentState, lastState;

        #endregion 属性

        #region 构造
        public StateSystem()
        {

        }

        public StateSystem(string name, int size = 64)
        {
            systemName = name; Size = size;
        }
        #endregion 构造

        #region 方法
        /// <summary>
        /// 添加状态
        /// </summary>
        /// <param name="state">需添加的状态</param>
        /// <returns></returns>
        public bool AddState(State state)
        {
            if (states.Count < size)
            {
                states[state.ID] = state;
                state.stateSystem = this;
                Debug.Log(state.ID + " " + state.stateName);
                if (currentState == null)
                {
                    currentState = state;
                }
                return true;
            }
            return false;
        }

        public bool DeleteState(int id)
        {
            if (!states.ContainsKey(id)) { return false; }
            states.Remove(id);
            return true;
        }

        public State this[int id]
        {
            get => states[id];
        }

        #endregion 方法
    }
}
