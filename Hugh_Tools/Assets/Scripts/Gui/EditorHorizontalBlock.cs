using UnityEngine;

namespace HughCommon.HGui
{
    public class EditorHorizontalBlock
        : NBase.BaseBlock<EditorHorizontalBlock>
    {
        bool _isValid;

        public EditorHorizontalBlock Open(params GUILayoutOption[] options)
        {
#if UNITY_EDITOR
            UnityEditor.EditorGUILayout.BeginHorizontal(options);
#else
            GUILayout.BeginHorizontal(options);
#endif
            _isValid = true;
            return this;
        }
        public EditorHorizontalBlock Open(GUIStyle style, params GUILayoutOption[] options)
        {
#if UNITY_EDITOR
            UnityEditor.EditorGUILayout.BeginHorizontal(style, options);
#else
            GUILayout.BeginHorizontal(style, options);
#endif
            _isValid = true;
            return this;
        }

        protected override void Dispose()
        {
            if (_isValid)
            {
#if UNITY_EDITOR
                UnityEditor.EditorGUILayout.EndHorizontal();
#else
                GUILayout.EndHorizontal();
#endif
            }

            base.Dispose();
        }
    }
}
