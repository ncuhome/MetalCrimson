
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ER.Entity
{

    public enum Direction { left, upLeft, downLeft, right ,upRight,downRight}

    /// <summary>
    /// 自带状态管理器的实体基类
    /// </summary>
    public class ExciteEntity : Entity
    {
        #region 组件
        /// <summary>
        /// 自身贴图组件
        /// </summary>
        public SpriteRenderer sprite;
        public TouchLand leftTouch;
        public TouchLand upTouch;
        public TouchLand downTouch;
        public TouchLand rightTouch;
        #endregion

        #region 字段
        public ATSpaceManager spaceManager;
        protected Dictionary<string, BHBase> bhs = new Dictionary<string, BHBase>();//行为状态（名称，对象）
        protected BHBase nowBh;//当前行为
        protected BHBase defBh;//默认行为状态
        /// <summary>
        /// 自身动画器
        /// </summary>
        protected Animator animator;
        /// <summary>
        /// 当前实体朝向
        /// </summary>
        public Direction direction;
        #endregion

        #region 公开属性
        public Direction FaceDirection
        {
            get => direction;
            set
            {
                direction = value;
                if(sprite==null)return;
                if(isFaceLeft())
                {
                    sprite.flipX = true;
                }
                else
                {
                    sprite.flipX = false;
                }
            }
        }
        /// <summary>
        /// 当前行为状态
        /// </summary>
        public BHBase NowBehaviourStatus { get => nowBh; }
        /// <summary>
        /// 实体拥有的行为状态属性
        /// </summary>
        public Dictionary<string, BHBase> BHs { get => bhs; }
        #endregion

        #region 功能函数
        #region 行为
        /// <summary>
        /// 设置默认行为状态
        /// </summary>
        public void SetDefBehaviour(BHBase bhbs)
        {
            if (!attributes.Contains(bhbs)){ AddAttribute(bhbs); }
            defBh = bhbs;
            BackDefBH();
        }
        /// <summary>
        /// 改变行为状态
        /// </summary>
        /// <param name="name">行为状态名称</param>
        public virtual void ChangeStateBH(string name)
        {
            if (bhs.Keys.Contains(name))//存在指定行为
            {
                BHBase bh = bhs[name];
                Debug.Log($"目标状态：{bh.Name}");
                if (bh is BHSenior)
                {
                    if (nowBh != defBh)
                    {
                        if (((BHSenior)nowBh).Breakers != null)//若Breakers为null，则无视中断对象限定
                        {
                            if (!((BHSenior)nowBh).Breakers.Contains(name)) { return; }//判定是否满足中断条件
                        }
                        Debug.Log($"切换状态：name={bh.Name},中断可行");
                    }
                    Debug.Log($"22目标状态：{bh.Name}");
                    if (!((BHSenior)bh).Prerequisite()) { return; }//判定是否符合释放条件
                    Debug.Log($"切换状态：name={bh.Name},条件可行");
                }
                nowBh.Break();//当前状态中断
                animator.SetTrigger("break");
                nowBh = bh;
                Debug.Log($"切换状态：name={bh.Name},index={bh.AnimationIndex}");
                animator.SetInteger("behaviour", nowBh.AnimationIndex);//切换动画机状态
            }
            else
            {
                Debug.LogWarning("指定行为不存在!");
            }
        }
        /// <summary>
        /// 执行当前行为的逻辑
        /// </summary>
        /// <param name="index">行为状态索引</param>
        public virtual void Behaviour(int index)
        {
            Debug.Log("当前状态："+nowBh.Name);
            if (nowBh != null) { nowBh.Behaviour(index); }
        }
        /// <summary>
        /// 当前行为状态结束，并回到默认行为状态
        /// </summary>
        public virtual void BackDefBH()
        {
            nowBh = defBh;
            animator.SetInteger("behaviour", nowBh.AnimationIndex);//切换动画机状态
        }
        #endregion
        #region 属性管理
        /// <summary>
        /// 添加新的属性对象
        /// </summary>
        public override void AddAttribute(IAttribute attribute)
        {
            if (attribute == null) return;
            if (attributes.Contains(attribute)) return;
            attributes.Add(attribute);
            BHBase bh = attribute as BHBase;
            if (bh != null)//添加行为模块
            {
                bhs.Add(bh.Name, bh);
            }
            attribute.Owner = this;
            Debug.Log(attributes.Count);
        }
        /// <summary>
        /// 从这个实体中移除指定属性（同时销毁组件）
        /// </summary>
        /// <param name="attribute"></param>
        public override void RemoveAttribute(IAttribute attribute)
        {
            if (attributes.Contains(attribute))
            {
                attributes.Remove(attribute);
                BHBase bh = attribute as BHBase;
                if (bh != null)
                {
                    bhs.Remove(bh.Name);
                }
                attribute.Destroy();
            }
        }
        /// <summary>
        /// 查看是否处于指定空间状态
        /// </summary>
        /// <param name="name">空间状态名称</param>
        /// <returns></returns>
        public virtual bool SpaceConform(string name)
        {
            if(spaceManager.NowSpaceInfo().name == name)
            {
                return true;
            }
            return false;
        }
        #endregion
        #region 方向判定
        /// <summary>
        /// 实体是否朝向左
        /// </summary>
        /// <returns></returns>
        public bool isFaceLeft()
        {
            if (direction == Direction.left || direction == Direction.downLeft || direction == Direction.upLeft) return true;
            return false;
        }
        /// <summary>
        /// 实体是否朝向右
        /// </summary>
        /// <returns></returns>
        public bool isFaceRight()
        {
            if (direction == Direction.right || direction == Direction.upRight || direction == Direction.downRight) return true;
            return false;
        }
        /// <summary>
        /// 实体是否朝向上
        /// </summary>
        /// <returns></returns>
        public bool isFaceUp()
        {
            if (direction == Direction.upLeft || direction == Direction.upRight) return true;
            return false;
        }
        /// <summary>
        /// 实体是否朝向下
        /// </summary>
        /// <returns></returns>
        public bool isFaceDown()
        {
            if (direction == Direction.downLeft || direction == Direction.downRight) return true;
            return false;
        }
        #endregion
        #endregion

        #region 内部函数
        /// <summary>
        /// 切换空间状态时附加的逻辑
        /// </summary>
        private void ChangeSpaceState(ATSpaceManager.SpaceInfo info)
        {
            animator.SetInteger("space", info.index);
        }
        #endregion

        #region Unity
        protected override void Awake()
        {
            base.Awake();
            spaceManager = new ATSpaceManager(this,downTouch);
            spaceManager.SpaceChangeEvent += ChangeSpaceState;
            damageTag = "ExciteEntity";
            animator = GetComponent<Animator>();
        }
        protected override void Start()
        {
            base.Start();
            Debug.Log("组件数量：" + attributes.Count);
            foreach(IAttribute attribute in attributes)
            {
                attribute.Initialization();
            }
        }

        private void Update()
        {
        }
        #endregion
    }
}
