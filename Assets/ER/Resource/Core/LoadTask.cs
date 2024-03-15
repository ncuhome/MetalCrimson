using System;
using UnityEngine;

namespace ER.Resource
{
    /// <summary>
    /// 加载任务
    /// </summary>
    public class LoadTask
    {
        /// <summary>
        /// 加载情况:  0:load   1:load_force
        /// </summary>
        public LoadProgress progress_load;
        public LoadProgress progress_load_force;
        /// <summary>
        /// 是否清除之前的资源 0:不清除 1:清除 2:强制清除
        /// </summary>
        public int clear;
        public string[] unload;//需要卸载的资源
        public string[] load;//需要加载的资源
        public string[] load_force;//需要强制加载的资源

        public LoadTask() { }
        public LoadTask(LoadTaskInfo info)
        {
            clear = info.clear;
            load = info.load;
            unload = info.unload;
            load_force = info.load_force;
        }
    }

    public struct LoadTaskInfo
    {
        /// <summary>
        /// 是否清除之前的资源 0:不清除 1:清除 2:强制清除
        /// </summary>
        public int clear;
        public string[] unload;//需要卸载的资源
        public string[] load;//需要加载的资源
        public string[] load_force;//需要强制加载的资源
    }

    public class LoadProgress
    {
        public int loaded;
        public int count;
        public Action callback;
        public bool done;

        public void AddProgress()
        {
            loaded++;
            if (loaded >= count)
            {
                done = true;
                callback?.Invoke();
            }
        }

        public static LoadProgress Done
        {
            get=>new LoadProgress()
            {
                loaded = 0,
                count = 0,
                callback = null,
                done = true
            };
        }
    }
}
