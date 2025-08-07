using UnityEngine;

namespace HughCommon.HGui
{
    public class EditorVerticalBlock
        : NBase.BaseBlock<EditorVerticalBlock>
    {
        bool _isValid;

        public EditorVerticalBlock Open(params GUILayoutOption[] options)
        {
#if UNITY_EDITOR
            UnityEditor.EditorGUILayout.BeginVertical(options);
#else
            GUILayout.BeginVertical(options);
#endif
            _isValid = true;
            return this;
        }
        public EditorVerticalBlock Open(GUIStyle style, params GUILayoutOption[] options)
        {
#if UNITY_EDITOR
            UnityEditor.EditorGUILayout.BeginVertical(style, options);
#else
            GUILayout.BeginVertical(style, options);
#endif
            _isValid = true;
            return this;
        }

        protected override void Dispose()
        {
            if (_isValid)
            {
#if UNITY_EDITOR
                UnityEditor.EditorGUILayout.EndVertical();
#else
                GUILayout.EndVertical();
#endif
            }

            base.Dispose();
        }
    }
}
