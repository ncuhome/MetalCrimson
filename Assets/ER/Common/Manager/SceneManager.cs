using ER.Template;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ER
{
    ///场景管理器
    public class SceneManager : MonoSingleton<SceneManager>,MonoInit
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
        /// 加载场景; 自动销毁旧场景
        /// </summary>
        /// <param name="transition"></param>
        /// <param name="asyncLoad"></param>
        public void LoadScene(string sceneName,SceneTransition transition = null,bool asyncLoad = false)
        {
            //异步加载
            if(asyncLoad)
            {
                if (transition != null)
                {
                    transition.EnterTransition();
                }
                StartCoroutine(LoadSceneAsync(sceneName, LoadSceneMode.Single, transition));
            }
            else
            {
                if(transition != null)
                {
                    transition.EnterTransition();
                }
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            }
        }
        /// <summary>
        /// 加载场景; 自动销毁旧场景
        /// </summary>
        /// <param name="transition"></param>
        /// <param name="asyncLoad"></param>
        public void LoadScene(SceneConfigure scene, SceneTransition transition = null, bool asyncLoad = false)
        {
            //异步加载
            if (asyncLoad)
            {
                scenes[scene.SceneName] = scene;
                if (transition != null)
                {
                    transition.EnterTransition();
                }
                StartCoroutine(LoadSceneAsync(scene.SceneName, LoadSceneMode.Single, transition));
            }
            else
            {
                if (transition != null)
                {
                    transition.EnterTransition();
                }
                UnityEngine.SceneManagement.SceneManager.LoadScene(scene.SceneName, LoadSceneMode.Single);
                scene.Initialize();
            }
        }
        /// <summary>
        /// 叠加场景;保留旧场景
        /// </summary>
        public void CoverScene(string sceneName, SceneTransition transition = null, bool asyncLoad = false)
        {
            //异步加载
            if (asyncLoad)
            {
                if (transition != null)
                {
                    transition.EnterTransition();
                }
                StartCoroutine(LoadSceneAsync(sceneName, LoadSceneMode.Additive, transition));
            }
            else
            {
                if (transition != null)
                {
                    transition.EnterTransition();
                }
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            }
        }
        /// <summary>
        /// 叠加场景;保留旧场景
        /// </summary>
        public void CoverScene(SceneConfigure scene, SceneTransition transition = null, bool asyncLoad = false)
        {
            //异步加载
            if (asyncLoad)
            {
                scenes[scene.SceneName] = scene;
                if (transition != null)
                {
                    transition.EnterTransition();
                }
                StartCoroutine(LoadSceneAsync(scene.SceneName, LoadSceneMode.Additive, transition));
            }
            else
            {
                if (transition != null)
                {
                    transition.EnterTransition();
                }
                UnityEngine.SceneManagement.SceneManager.LoadScene(scene.SceneName, LoadSceneMode.Additive);
                scene.Initialize();
            }
        }

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="transition"></param>
        /// <returns></returns>
        private IEnumerator LoadSceneAsync(string sceneName, LoadSceneMode loadMode = LoadSceneMode.Single, SceneTransition transition = null)
        {
            var opt = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, loadMode);
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

        public void Init()
        {
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);
            if (!enabled)
                enabled = true;
            MonoLoader.InitCallback();
        }
    }

    /// <summary>
    /// 场景初始化配置器, 在加载完场景后执行初始化函数
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