using System;
using UnityEngine;

namespace HughCommon.HGui.NBase
{
    public class BaseBlock<T>
        : AbstractBlock
        where T : BaseBlock<T>
    {
        Color _guiColor;
        Color _contentColor;
        Color _backgroundColor;
        Color _outlineColor;

        bool _guiEnable;

        bool _outline;
        bool _useRightClickEvent;
        Action _rightClickEvent;
        float _thickness;

        T _this;

        public BaseBlock()
        {
            _guiColor = GUI.color;
            _contentColor = GUI.contentColor;
            _backgroundColor = GUI.backgroundColor;
            _guiEnable = GUI.enabled;
            _this = this as T;
        }

        public T SetEnable(bool enable)
        {
            GUI.enabled = enable;
            return _this;
        }

        public T SetColor(Color color)
        {
            GUI.color = color;
            return _this;
        }

        public T SetBackgroundColor(Color color)
        {
            GUI.backgroundColor = color;
            return _this;
        }

        public T SetContentColor(Color color)
        {
            GUI.contentColor = color;
            return _this;
        }

        public T SetOutline()
        {
            _outlineColor = Color.white;
            _thickness = 1;
            _outline = true;

            return _this;
        }

        public T SetOutline(Color color, float thickness)
        {
            if (thickness <= 0)
                thickness = 1;

            _outlineColor = color;
            _thickness = thickness;
            _outline = true;

            return _this;
        }

        public T SetRightClickEvent(Action action)
        {
            _useRightClickEvent = true;
            _rightClickEvent = action;
            return _this;
        }

        public T CheckEnable(Func<bool> predicator)
        {
            SetEnable(predicator.Fire());
            return _this;
        }

        protected override void Dispose()
        {
            GUI.color = _guiColor;
            GUI.contentColor = _contentColor;
            GUI.backgroundColor = _backgroundColor;
            GUI.enabled = _guiEnable;

            if (_useRightClickEvent &&
               Event.current.type == EventType.MouseUp &&
               Event.current.button == 1)
            {
                var rect = GUILayoutUtility.GetLastRect();
                var contains = rect.Contains(Event.current.mousePosition);
                if (contains)
                {
#if ASSET_PROJECT
                    _rightClickEvent.Invoke();
#else
                    _rightClickEvent.Fire();
#endif
                }
            }

            if (_outline)
            {
                var rect = GUILayoutUtility.GetLastRect();
                HTool.DrawTools.DrawOutline(rect, _outlineColor, _thickness);
            }
        }
    }

}
