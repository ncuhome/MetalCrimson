using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ER
{
    public class SceneManager : MonoSingleton<SceneManager>
    {
        [Tooltip("目标跳转场景 - 仅编辑器下使用")]
        public string AimScene;

        private Dictionary<string, SceneConfigure> scenes = new();

        [Tooltip("跳转至目标场景 - 仅编辑器下使用")]
        [ContextMenu("跳转至场景")]
        public void SkipScene()
        {
            if(scenes.TryGetValue(AimScene,out var configure))
            {
                LoadScene(configure);
            }
        }

        /// <summary>
        /// 添加新的场景至管理器
        /// </summary>
        /// <param name="configure"></param>
        public void AddScene(SceneConfigure configure)
        {
            scenes[configure.SceneName] = configure;
        }

        /// <summary>
        /// 加载场景; 自动销毁旧场景
        /// </summary>
        /// <param name="transition"></param>
        /// <param name="asyncLoad"></param>
        public void LoadScene(string sceneName,SceneTransition transition = null,bool asyncLoad = false)
        {
            if(!scenes.ContainsKey(sceneName))
            {
                Debug.LogError($"该场景不存在, 无法加载:{sceneName}");
                return;
            }

            //异步加载
            if(asyncLoad)
            {
                if (transition != null)
                {
                    transition.EnterTransition();
                }
                StartCoroutine(LoadSceneAsync(sceneName));
            }
            else
            {
                if(transition != null)
                {
                    transition.EnterTransition();
                }
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
                scenes[sceneName].Initialize();
            }
        }
        /// <summary>
        /// 加载场景; 自动销毁旧场景
        /// </summary>
        /// <param name="transition"></param>
        /// <param name="asyncLoad"></param>
        public void LoadScene(SceneConfigure scene, SceneTransition transition = null, bool asyncLoad = false)
        {
            if (!scenes.ContainsKey(scene.SceneName))
            {
                scenes[scene.SceneName] = scene;
            }

            //异步加载
            if (asyncLoad)
            {
                if (transition != null)
                {
                    transition.EnterTransition();
                }
                StartCoroutine(LoadSceneAsync(scene.SceneName));
            }
            else
            {
                if (transition != null)
                {
                    transition.EnterTransition();
                }
                UnityEngine.SceneManagement.SceneManager.LoadScene(scene.SceneName, LoadSceneMode.Single);
                scenes[scene.SceneName].Initialize();
            }
        }

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="transition"></param>
        /// <returns></returns>
        private IEnumerator LoadSceneAsync(string sceneName, SceneTransition transition = null)
        {
            var opt = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            while(!opt.isDone)//场景是否加载完毕
            {
                //同步加载进度给场景过渡类
                if(transition != null)
                {
                    transition.Progress = opt.progress;
                    yield return null;
                }
            }
            scenes[sceneName].Initialize();
        }
    }

    /// <summary>
    /// 场景初始化配置器
    /// </summary>
    public interface SceneConfigure
    {
        /// <summary>
        /// 场景名称
        /// </summary>
        public string SceneName { get; }

        /// <summary>
        /// 初始化
        /// </summary>
        public abstract void Initialize();
    }
}