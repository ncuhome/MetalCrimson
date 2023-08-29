using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ER
{
    /// <summary>
    /// 动画片段切割器(用于模拟动画遮罩, 剔除不需要动画部分)(注意: 仅在编辑器模式下使用)
    /// </summary>
    public class AnimationClipSplitter : MonoBehaviour
    {
        public enum MaskType
        { WhiteList, BlackList }

        [Tooltip("遮罩类型")]
        [SerializeField]
        private MaskType maskType;

        [Tooltip("遮罩 - 物体路径")]
        [SerializeField]
        public List<string> maskPath;

        [Tooltip("源动画片段")]
        public AnimationClip originalClip;

        public AnimationClip newClip;

        [ContextMenu("切割动画片段")]
        public void SplitClip()
        {
            string name = newClip.name;
            EditorUtility.CopySerialized(originalClip, newClip);//拷贝源动画片段
            newClip.name = name;
            RemoveCurves(newClip);
        }

        private void RemoveCurves(AnimationClip clip)
        {
            EditorCurveBinding[] bindings = AnimationUtility.GetCurveBindings(clip);

            foreach (EditorCurveBinding binding in bindings)
            {
                //string propertyName = binding.propertyName;
                string path = binding.path;
                Debug.Log($"正在检查动画曲线绑定:{path}");
                if (maskType == MaskType.WhiteList)
                {
                    if (!maskPath.Contains(path))
                    {
                        AnimationUtility.SetEditorCurve(clip, binding, null);
                    }
                }
                else
                {
                    if (maskPath.Contains(path))
                    {
                        AnimationUtility.SetEditorCurve(clip, binding, null);
                    }
                }
            }

            AssetDatabase.SaveAssets();
        }
    }
}