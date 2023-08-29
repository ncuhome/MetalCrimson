using ER.Entity2D;
using System;
using System.Collections.Generic;

namespace Mod_Level
{
    /// <summary>
    /// 修正委托
    /// </summary>
    public class CorrectValueDelegate
    {
        /// <summary>
        /// 优先级
        /// </summary>
        public int level;

        /// <summary>
        /// 数值修正委托
        /// </summary>
        public Func<float, float> correct;
    }

    /// <summary>
    /// 技能的基类
    /// </summary>
    public abstract class MDSkill : MDAction
    {
        public MDSkill()
        { actionName = "Skill"; controlType = ControlType.Trigger; }

        /// <summary>
        /// 基础魔力消耗
        /// </summary>
        protected float baseMP = 0;

        /// <summary>
        /// 基础魔力消耗
        /// </summary>
        public float BaseMP { get => baseMP; }

        /// <summary>
        /// 技能魔力消耗
        /// </summary>
        protected float mp;

        /// <summary>
        /// 技能魔力消耗
        /// </summary>
        public float MP { get => mp; }

        /// <summary>
        /// 基础技能CD
        /// </summary>
        protected float baseCD = 0;

        /// <summary>
        /// 技能CD
        /// </summary>
        protected float cd;

        /// <summary>
        /// 基础技能CD
        /// </summary>
        public float BaseCD { get => baseCD; }

        /// <summary>
        /// 技能CD
        /// </summary>
        public float CD { get => cd; }

        /// <summary>
        /// CD数值修正委托
        /// </summary>
        private List<CorrectValueDelegate> corrects_cd;

        /// <summary>
        /// MP数值修正委托
        /// </summary>
        private List<CorrectValueDelegate> corrects_mp;

        /// <summary>
        /// 重新计算 魔力和CD值 的修正函数
        /// </summary>
        public void Correct()
        {
            cd = baseCD;
            foreach (var coreect in corrects_cd)
            {
                cd = coreect.correct(cd);
            }

            mp = baseMP;
            foreach (var coreect in corrects_mp)
            {
                mp = coreect.correct(mp);
            }
        }

        /// <summary>
        /// 添加CD修正
        /// </summary>
        /// <param name="correct"></param>
        public void AddCDCorrect(CorrectValueDelegate correct)
        {
            if (correct.correct == null) return;
            for (int i = 0; i < corrects_cd.Count; i++)
            {
                if (correct.level >= corrects_cd[i].level)
                {
                    corrects_cd.Insert(i, correct);
                }
            }
            Correct();
        }

        /// <summary>
        /// 移除指定CD修正
        /// </summary>
        /// <param name="correct"></param>
        public void RemoveCDCorrect(CorrectValueDelegate correct)
        {
            if (corrects_cd.Contains(correct))
            {
                corrects_cd.Remove(correct);
            }
            Correct();
        }

        /// <summary>
        /// 添加MP修正
        /// </summary>
        /// <param name="correct"></param>
        public void AddMPCorrect(CorrectValueDelegate correct)
        {
            if (correct.correct == null) return;
            for (int i = 0; i < corrects_mp.Count; i++)
            {
                if (correct.level >= corrects_mp[i].level)
                {
                    corrects_mp.Insert(i, correct);
                }
            }
            Correct();
        }

        /// <summary>
        /// 移除指定MP修正
        /// </summary>
        /// <param name="correct"></param>
        public void RemoveMPCorrect(CorrectValueDelegate correct)
        {
            if (corrects_mp.Contains(correct))
            {
                corrects_mp.Remove(correct);
            }
            Correct();
        }

        public abstract override bool ActionJudge();

        public override void Initialize()
        {
            throw new System.NotImplementedException();
        }

        protected override void StartAction(params string[] keys)
        {
            throw new System.NotImplementedException();
        }

        protected override void StopAction(params string[] keys)
        {
            throw new System.NotImplementedException();
        }
    }
}