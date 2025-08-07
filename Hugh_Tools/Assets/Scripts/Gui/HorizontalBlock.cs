using UnityEngine;

namespace HughCommon.HGui
{
    public class HorizontalBlock
        : NBase.BaseBlock<HorizontalBlock>
    {
        bool _isValid;

        public HorizontalBlock Open(params GUILayoutOption[] options)
        {

            GUILayout.BeginHorizontal(GUI.skin.box, options);
            _isValid = true;
            return this;
        }
        public HorizontalBlock Open(GUIStyle style, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal(style, options);
            _isValid = true;
            return this;
        }

        protected override void Dispose()
        {
            if (_isValid)
            {
                GUILayout.EndHorizontal();
            }

            base.Dispose();
        }
    }
}
