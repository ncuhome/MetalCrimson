using ER.Parser;
using ER.Resource;
using ER.ResourcePacker;
using ER.Template;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ER
{
    /// <summary>
    /// 声音管理器, 已接入 GameResouce 系统
    /// </summary>
    public class SoundManager: MonoSingleton<SoundManager>,MonoInit
    {
        public GameObject CompositePlayerPrefab;
        [Tooltip("是否使用资源索引器: 如果启用, 那么url会通过 GameResource 获取, 而不是使用默认的路径")]
        public bool UseResourceIndexer = false;

        private CompositePlayer bgmPlayer;//背景音乐播放器

        [SerializeField]
        private ObjectPool pool;

        //private List<AudioPlayer> players = new List<AudioPlayer>();//取出的播放器对象

        /// <summary>
        /// 初始化读取配置文件
        /// </summary>
        public void Init()
        {
            if (!gameObject.activeSelf)
                gameObject.SetActive(true);
            if (!enabled)
                enabled = true;
            TryCreateBGMPlayer();
        }
        private AudioPlayer GetAudioPlayer()
        {
            return (AudioPlayer)pool.GetObject(true);
        }

        /// <summary>
        /// 尝试在主摄像机上添加BGM播放器
        /// </summary>
        public void TryCreateBGMPlayer()
        {
            Transform player = Camera.main.transform.Find("BGMPlayer");//检测是否存在播放器物体
            if (player == null)
            {
                
                GameObject obj = GameObject.Instantiate(CompositePlayerPrefab, Camera.main.transform);
                obj.name = "BGMPlayer";
                bgmPlayer = obj.GetComponent<CompositePlayer>();
            }
            else
            {
                bgmPlayer = player.GetComponent<CompositePlayer>();
            }
            bgmPlayer.Init();
        }

        /// <summary>
        /// 播放背景音乐
        /// </summary>
        public void PlayBGM()
        {
            bgmPlayer.Play();
        }
        /// <summary>
        /// 暂停播放背景音乐
        /// </summary>
        public void PauseBGM()
        {
            bgmPlayer.Pause();
        }
        /// <summary>
        /// 停止播放背景音乐
        /// </summary>
        public void StopBGM()
        {
            bgmPlayer.Stop();
        }
        /// <summary>
        /// 播放背景音乐
        /// </summary>
        /// <param name="registryName"></param>
        public void PlayBGM(string registryName)
        {
            AudioResource resource = GR.Get<AudioResource>(registryName);
            Debug.Log("null:"+(resource == null));
            bgmPlayer.Play(resource.Value);
        }
        /// <summary>
        /// 创建一个播放器
        /// </summary>
        /// <param name="registryName">音频资源名称</param>
        /// <param name="playMode">播放模式</param>
        /// <param name="effect">效果</param>
        /// <returns></returns>
        public AudioPlayer CreatePlayer(string registryName,AudioPlayer.PlayMode playMode = AudioPlayer.PlayMode.SingleHide, AudioPlayer.Effect effect = AudioPlayer.Effect.None)
        {
            AudioPlayer player = GetAudioPlayer();
            player.Clip = GR.Get<AudioResource>(registryName).Value;
            player.Mode = playMode;
            player.AudioEffect = effect;
            return player;
        }
        /// <summary>
        /// 播放一次音效, 并自动销毁
        /// </summary>
        /// <returns></returns>
        public AudioPlayer Shoot(string registryName)
        {
            AudioPlayer player = GetAudioPlayer();
            player.Clip = GR.Get<AudioResource>(registryName).Value;
            player.Mode = AudioPlayer.PlayMode.SingleHide;
            player.AudioEffect = AudioPlayer.Effect.None;
            player.Play();
            return player;
        }

        /// <summary>
        /// 播放音频
        /// </summary>
        /// <param name="registryName"></param>
        public void Play(string registryName)
        {

        }
        /// <summary>
        /// 填充唱片组, 根据唱片组提供的资源名称填充具体的资源对象
        /// </summary>
        public void FillAudioClipGroup(AudioClipGroup group)
        {
            for (int i=0;i<group.infos.Length;i++)
            {
                group.infos[i].clip = GR.Get<AudioResource>(group.infos[i].registryName).Value;
            }
        }
        /// <summary>
        /// 获取指定资源的对象
        /// </summary>
        /// <param name="registryName"></param>
        /// <returns></returns>
        public AudioClip GetClip(string registryName)
        {
            return GR.Get<AudioResource>(registryName).Value;
        }
        public SoundManager() 
        { 
            
        }
    }
}
