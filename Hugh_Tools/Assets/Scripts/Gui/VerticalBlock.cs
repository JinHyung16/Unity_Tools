using UnityEngine;

namespace HughCommon.HGui
{
    public class VerticalBlock
        : NBase.BaseBlock<VerticalBlock>
    {
        bool _isValid;

        public VerticalBlock Open(params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical(GUI.skin.box, options);
            _isValid = true;
            return this;
        }

        protected override void Dispose()
        {
            if (_isValid)
            {
                GUILayout.EndVertical();
            }

            base.Dispose();
        }
    }
}
