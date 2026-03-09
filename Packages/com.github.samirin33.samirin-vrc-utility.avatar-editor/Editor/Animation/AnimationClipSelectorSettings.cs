#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Samirin33.SamirinVRCUtility.AvatarEditor
{
    [Serializable]
    public class ControllerClipEntry
    {
        public string controllerPath;
        public AnimationClip clip;
    }

    /// <summary>
    /// Animation Clip Selector の表示設定とAnimator毎の最後に表示したクリップを保存するアセット。
    /// </summary>
    public class AnimationClipSelectorSettings : ScriptableObject
    {
        [SerializeField] private float _itemSpacing = 2f;
        [SerializeField] private List<ControllerClipEntry> _lastDisplayedClipPerController = new List<ControllerClipEntry>();
        [SerializeField] private List<AnimationClip> _ignoreClips = new List<AnimationClip>();
        [SerializeField] private List<string> _defaultIgnoreGUIDs = new List<string> { "4de039275b65be24c8f0a641d7a44924" };

        public float ItemSpacing
        {
            get => _itemSpacing;
            set => _itemSpacing = Mathf.Clamp(value, 0f, 16f);
        }

        /// <summary>競合警告を出さない AnimationClip 一覧。</summary>
        public IReadOnlyList<AnimationClip> IgnoreClips => _ignoreClips;

        /// <summary>指定クリップが競合警告の対象外かどうか。</summary>
        public bool IsIgnoredClip(AnimationClip clip)
        {
            if (clip == null) return false;
            if (_ignoreClips.Contains(clip)) return true;
            var path = AssetDatabase.GetAssetPath(clip);
            if (string.IsNullOrEmpty(path)) return false;
            var guid = AssetDatabase.AssetPathToGUID(path);
            return !string.IsNullOrEmpty(guid) && _defaultIgnoreGUIDs.Contains(guid);
        }

        public AnimationClip GetLastDisplayedClip(string controllerPath)
        {
            if (string.IsNullOrEmpty(controllerPath)) return null;
            var entry = _lastDisplayedClipPerController.Find(e => e.controllerPath == controllerPath);
            return entry?.clip;
        }

        public void SetLastDisplayedClip(string controllerPath, AnimationClip clip)
        {
            if (string.IsNullOrEmpty(controllerPath)) return;
            var entry = _lastDisplayedClipPerController.Find(e => e.controllerPath == controllerPath);
            if (entry != null)
            {
                entry.clip = clip;
            }
            else
            {
                _lastDisplayedClipPerController.Add(new ControllerClipEntry { controllerPath = controllerPath, clip = clip });
            }
        }
    }
}
#endif
