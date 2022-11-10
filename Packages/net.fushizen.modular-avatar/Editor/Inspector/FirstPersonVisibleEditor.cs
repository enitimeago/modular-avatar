﻿using System;
using UnityEditor;
using UnityEngine;

namespace net.fushizen.modular_avatar.core.editor
{
    [CustomEditor(typeof(ModularAvatarVisibleHeadAccessory))]
    public class FirstPersonVisibleEditor : Editor
    {
        private VisibleHeadAccessoryProcessor _processor;

        private void OnEnable()
        {
            var target = (ModularAvatarVisibleHeadAccessory) this.target;
            var avatar = RuntimeUtil.FindAvatarInParents(target.transform);

            if (avatar != null) _processor = new VisibleHeadAccessoryProcessor(avatar);
        }

        public override void OnInspectorGUI()
        {
            var target = (ModularAvatarVisibleHeadAccessory) this.target;

            LogoDisplay.DisplayLogo();

            InspectorCommon.DisplayOutOfAvatarWarning(targets);

#if UNITY_ANDROID
            EditorGUILayout.HelpBox(Localization.S("fpvisible.quest"), MessageType.Warning);

#else

            if (_processor != null)
            {
                var status = _processor.Validate(target);

                switch (status)
                {
                    case VisibleHeadAccessoryProcessor.ReadyStatus.Ready:
                    case VisibleHeadAccessoryProcessor.ReadyStatus.ParentMarked:
                        EditorGUILayout.HelpBox(Localization.S("fpvisible.normal"), MessageType.Info);
                        break;
                    default:
                    {
                        var label = "fpvisible." + status;
                        EditorGUILayout.HelpBox(Localization.S(label), MessageType.Error);
                        break;
                    }
                }
            }

#endif

            Localization.ShowLanguageUI();
        }
    }
}