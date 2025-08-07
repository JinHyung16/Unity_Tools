using System;
using System.Collections.Generic;
using UnityEngine;

namespace HughCommon.HGui
{
    public class ScrollBlock
        : NBase.BaseBlock<ScrollBlock>
    {
        static Dictionary<Tuple<object, string>, Vector2> s_scrollDic = new Dictionary<Tuple<object, string>, Vector2>();
        static Dictionary<Tuple<object, string>, Rect> s_scrollRectDic = new Dictionary<Tuple<object, string>, Rect>();
        static Dictionary<Tuple<object, string>, Dictionary<string, Rect>> s_scrollItemDic = new Dictionary<Tuple<object, string>, Dictionary<string, Rect>>();

        static Tuple<object, string> _key = new Tuple<object, string>();

        static Vector2 GetScroll(Tuple<object, string> key)
        {
            Vector2 result;
            lock (s_scrollDic)
            {
                if (s_scrollDic.TryGetValue(key, out result) == false)
                {
                    result = Vector2.zero;
                    s_scrollDic.Add(key, Vector2.zero);
                }
            }
            return result;
        }

        public static Vector2 GetScroll(object i_keyObj, string i_keyName)
        {
            _key.Item1 = i_keyObj;
            _key.Item2 = i_keyName;

            return GetScroll(_key);
        }

        public static void SetScroll(object i_keyObj, string i_keyName, Vector2 i_scrollPos)
        {
            lock (s_scrollDic)
            {
                s_scrollDic[Tuple.Create(i_keyObj, i_keyName)] = i_scrollPos;
            }
        }

        public static Rect GetScrollRect(object i_keyObj, string i_keyName)
        {
            _key.Item1 = i_keyObj;
            _key.Item2 = i_keyName;

            return GetScrollRect(_key);
        }

        static Rect GetScrollRect(Tuple<object, string> key)
        {
            Rect result;
            lock (s_scrollRectDic)
            {
                if (s_scrollRectDic.TryGetValue(key, out result) == false)
                {
                    result = Rect.zero;
                    s_scrollRectDic.Add(key, result);
                }
            }
            return result;
        }

        static void SetScrollRect(object i_keyObj, string i_keyName, Rect rect)
        {
            lock (s_scrollRectDic)
            {
                s_scrollRectDic[Tuple.Create(i_keyObj, i_keyName)] = rect;
            }
        } 
        static Dictionary<string, Rect> GetScrollItemDic(object owner, string name) 
        {
            Dictionary<string, Rect> result;
            lock (s_scrollItemDic)
            {
                _key.Item1 = owner;
                _key.Item2 = name;

                if (s_scrollItemDic.TryGetValue(_key, out result) == false)
                {
                    result = new Dictionary<string, Rect>();
                    s_scrollItemDic.Add(_key, result);
                }
            }
            return result;
        }


        Dictionary<string, Rect> _scrollItemDic;

        bool _enable = false;
        bool _isValid = false;
        Vector2 _scrollPosition;
        Rect _scrollRect;

        object _owner;
        string _name;
        public ScrollBlock Open(object owner, string name, bool alwaysShowHorizontal, bool alwaysShowVertical, params GUILayoutOption[] options)
        {

            if (Event.current != null && Event.current.type == EventType.Used)
            {
                _enable = false;
                return this;
            }

            _enable = true;
            _isValid = true;

            _owner = owner;
            _name = name;            

            var scroll = GetScroll(owner, name);
            scroll = GUILayout.BeginScrollView(scroll, alwaysShowHorizontal, alwaysShowVertical, options);
            SetScroll(owner, name, scroll);

            _scrollPosition = scroll;
            _scrollRect = GetScrollRect(owner, name);

            return this;
        }

        public ScrollBlock Open(object owner, string name, params GUILayoutOption[] options)
        {
            if (Event.current != null && Event.current.type == EventType.Used)
            {
                _enable = false;
                return this;
            }

            _enable = true;
            _isValid = true;

            _owner = owner;
            _name = name;

            var scroll = GetScroll(owner, name);
            scroll = GUILayout.BeginScrollView(scroll, options);
            SetScroll(owner, name, scroll);

            _scrollPosition = scroll;
            _scrollRect = GetScrollRect(owner, name);

            return this;
        }


        Rect? GetItemRect(string key)
        {
            if (_scrollItemDic == null)
            {
                _scrollItemDic = GetScrollItemDic(_owner, _name);
                return null;
            }

            Rect result;
            if (_scrollItemDic.TryGetValue(key, out result) == false)
            {
                return null;
            }
            return result;
        }



        public void Draw(string key, Action draw)
        {
            var minHight = _scrollPosition.y;
            var maxHieght = minHight + _scrollRect.y + _scrollRect.height;
            var itemRect = GetItemRect(key);
            if (IsDrawItem(minHight,maxHieght, itemRect))
            {
                using (new VerticalBlock().Open())
                {
                    draw.Fire();
                }

                if (Event.current.type == EventType.Repaint)
                {
                    var rect = GUILayoutUtility.GetLastRect();
                    _scrollItemDic[key] = rect;
                }
            }
            else 
            {

                GUILayout.Box(string.Empty, GUILayout.Height(itemRect.Value.height));

                if (Event.current.type == EventType.Repaint)
                {
                    var rect = GUILayoutUtility.GetLastRect();
                    _scrollItemDic[key] = rect;
                }
            }
        }

        bool IsDrawItem(float minHeight, float maxHeight , Rect? item)
        {
            if (item.HasValue == false)
                return true;

            var itemTopY = item.Value.y;
            var itemBottomY = itemTopY + item.Value.height;

            if (itemBottomY < minHeight)
                return false;

            if (itemTopY > maxHeight)
                return false;

            return true;
        }

        public Vector2 GetScrollPosition()
        {
            return _scrollPosition;
        }

        public Rect GetScrollRect() 
        {
            return _scrollRect;
        }

        protected override void Dispose()
        {
            if (_enable && _isValid)
            {
                GUILayout.EndScrollView();

                if (Event.current.type == EventType.Repaint)
                {
                    var rect = GUILayoutUtility.GetLastRect();
                    SetScrollRect(_owner, _name, rect);
                }
            }

            base.Dispose();
        }
    }
}
