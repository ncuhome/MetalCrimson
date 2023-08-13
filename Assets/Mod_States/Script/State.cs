
using System;
using System.Collections.Generic;
using UnityEngine;
namespace States
{
    [Serializable]
    public class State
    {
        #region 状态属性
        /// <summary>
        /// 状态ID
        /// </summary>
        public int ID { get; private set; } = 0;
        /// <summary>
        /// 状态名称
        /// </summary>
        public string stateName;
        /// <summary>
        /// 出口ID与对应判定bool
        /// </summary>
        public Dictionary<int, bool> exitID = new Dictionary<int, bool>();
        /// <summary>
        /// 状态中的Update事件
        /// </summary>
        private Action OnStateAction;
        /// <summary>
        /// 进入状态事件
        /// </summary>
        private Action OnEnterStateAction;
        /// <summary>
        /// 出状态事件，输入前往状态的ID
        /// </summary>
        private Action<int> OnExitStateAction;
        /// <summary>
        /// 状态所属系统
        /// </summary>
        public StateSystem stateSystem;
        #endregion 属性

        #region 构造

        public State(int id)
        {
            ID = id;
        }

        public State(int id, string name)
        {
            ID = id;
            stateName = name;
        }

        #endregion 构造

        #region 方法
        /// <summary>
        /// 进入状态时的函数
        /// </summary>
        public void OnEnterState()
        {
            OnEnterStateAction?.Invoke();
        }
        /// <summary>
        /// 状态中Update函数
        /// </summary>
        public void OnState()
        {
            OnStateAction?.Invoke();
            ExitJudgement();
        }
        /// <summary>
        /// 出口判定，如果有出口为true则前往
        /// </summary>
        public void ExitJudgement()
        {
            foreach (var kvp in exitID)
            {
                if (kvp.Value == true)
                {
                    OnExitState(kvp.Key);
                }
            }
        }
        /// <summary>
        /// 退出状态时的函数
        /// </summary>
        /// <param name="id"></param>
        public void OnExitState(int id)
        {
            Debug.Log("Exit to state " + id);

            OnExitStateAction?.Invoke(id);


            stateSystem.states[id].OnEnterState();
            stateSystem.currentState = stateSystem.states[id];
            stateSystem.lastState = this;

            ChangeExitJudgement(id, false);
        }
        /// <summary>
        /// 改变自定义状态循环函数
        /// </summary>
        /// <param name="action"></param>
        public void ChangeStateAction(Action action)
        {
            OnStateAction = action;
        }
        /// <summary>
        /// 改变自定义出口函数
        /// </summary>
        /// <param name="action"></param>
        public void ChangeExitAction(Action<int> action)
        {
            OnExitStateAction = action;
        }
        /// <summary>
        /// 改变自定义入口函数
        /// </summary>
        /// <param name="action"></param>
        public void ChangeEnterAction(Action action)
        {
            OnEnterStateAction = action;
        }
        /// <summary>
        /// 改变或添加出口判定的值
        /// </summary>
        /// <param name="id">出口ID</param>
        /// <param name="value">出口判定值</param>
        public void ChangeExitJudgement(int id, bool value)
        {
            exitID[id] = value;
        }
        #endregion
    }
}