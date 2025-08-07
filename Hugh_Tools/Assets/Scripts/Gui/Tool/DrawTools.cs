using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HughCommon.HGui.HTool
{
    public class DrawTools
    {
        public static Texture2D WhiteTex
        {
            get
            {
                if (_whiteTex == null)
                {
                    _whiteTex = CreateBlankTex(Color.white);
                }
                return _whiteTex;
            }
        }
        public static Texture2D BlackTex
        {
            get
            {
                if (_blackTex == null)
                {
                    _blackTex = CreateBlankTex(Color.black);
                }
                return _blackTex;
            }
        }

        static Dictionary<Color, Texture2D> _dictCircleTex = new Dictionary<Color, Texture2D>();

        static Texture2D _whiteTex;
        static Texture2D _blackTex;

        public static Texture2D GetCircleTexture(Color c)
        {
            if (_dictCircleTex.TryGetValue(c, out var tex) == false)
            {
                tex = CreateCircleTex(64, c);
                _dictCircleTex.Add(c, tex);
            }

            return tex;
        }


        static Texture2D CreateBlankTex(Color color)
        {
            var tex = new Texture2D(4, 4);
            for (int yIndex = 0; yIndex < 4; yIndex++)
            {
                for (int xIndex = 0; xIndex < 4; xIndex++)
                {
                    tex.SetPixel(xIndex, yIndex, color);
                }
            }
            tex.Apply(true);
            return tex;
        }

        static Texture2D CreateCircleTex(int diameter, Color color)
        {
            var texture = new Texture2D(diameter, diameter);
#if UNITY_EDITOR
            texture.alphaIsTransparency = true;
#endif
            var transparent = new Color(0, 0, 0, 0);
            var center = new Vector2(diameter / 2f, diameter / 2f);
            var radius = diameter / 2f;

            for (int y = 0; y < diameter; y++)
            {
                for (int x = 0; x < diameter; x++)
                {
                    var pos = new Vector2(x, y);
                    var dis = Vector2.Distance(pos, center);
                    if (dis <= radius)
                    {
                        var alpha = Mathf.Lerp(1f, 0.5f, dis / radius);
                        var c = color;
                        c.a = alpha;
                        texture.SetPixel(x, y, c);
                    }
                    else
                        texture.SetPixel(x, y, transparent);
                }
            }

            texture.Apply();
            return texture;
        }

        public static void DrawSeparatorVertical(Color i_color)
        {
            DrawSeparator(i_color, 3f, true);
        }

        public static void DrawSeparatorHorizontal(Color i_color)
        {
            DrawSeparator(i_color, 3f, false);
        }


        public static void DrawSeparatorHorizontal(Color i_color, float i_thickness)
        {
            DrawSeparator(i_color, i_thickness, false);
        }

        public static void DrawSeparatorNoSpace(Color i_color, float i_thickness, bool isVertical)
        {
            if (Event.current.type == EventType.Repaint)
            {
                Rect rect = GUILayoutUtility.GetLastRect();
                if (isVertical)
                {
                    rect = rect.EnlargeRect(0f, -2f);
                }
                else
                {
                    rect = rect.EnlargeRect(-2f, 0f);
                }
                RepaintDrawSeparator(rect, i_color, i_thickness, 0f, isVertical);
            }

            GUILayout.Space(i_thickness);
        }

        public static void DrawSeparator(Color i_color, float i_thickness, bool isVertical)
        {
            if (Event.current.type == EventType.Repaint)
            {
                Rect rect = GUILayoutUtility.GetLastRect();
                RepaintDrawSeparator(rect, i_color, i_thickness, 4f, isVertical);
            }

            GUILayout.Space(5f + i_thickness);
        }

        public static Rect RepaintDrawSeparator(Rect i_rect, Color i_color, float i_thickness, float i_space, bool isVertical)
        {
            if (Event.current.type != EventType.Repaint)
            {
                return default(Rect);
            }

            Texture2D tex = WhiteTex;

            var colorBackup = GUI.color;

            GUI.color = i_color;
            if (isVertical)
            {
                GUI.DrawTexture(new Rect(i_rect.xMax + i_space, i_rect.yMin, i_thickness, i_rect.height), tex);
                GUI.DrawTexture(new Rect(i_rect.xMax + i_space, i_rect.yMin, 1f, i_rect.height), tex);
                GUI.DrawTexture(new Rect(i_rect.xMax + i_space - 1 + i_thickness, i_rect.yMin, 1f, i_rect.height), tex);
                GUI.color = colorBackup;

                return new Rect(i_rect.xMax + i_space, i_rect.yMin, i_thickness, i_rect.height);
            }
            else
            {
                GUI.DrawTexture(new Rect(i_rect.xMin, i_rect.yMax + i_space, i_rect.width, i_thickness), tex);
                GUI.DrawTexture(new Rect(i_rect.xMin, i_rect.yMax + i_space, i_rect.width, 1f), tex);
                GUI.DrawTexture(new Rect(i_rect.xMin, i_rect.yMax + i_space - 1 + i_thickness, i_rect.width, 1f), tex);
                GUI.color = colorBackup;

                return new Rect(i_rect.xMin, i_rect.yMax + i_space, i_rect.width, i_thickness);
            }
        }

        public static void DrawGauge(Color i_bgColor, Color i_gaugeColor, float i_value)
        {
            if (Event.current.type == EventType.Repaint)
            {
                Texture2D tex = WhiteTex;
                Rect rect = GUILayoutUtility.GetLastRect();

                var colorBackup = GUI.color;

                GUI.color = i_bgColor;
                GUI.DrawTexture(new Rect(rect.xMin, rect.yMax + 4f, rect.width, 5f), tex);
                GUI.color = i_gaugeColor;
                GUI.DrawTexture(new Rect(rect.xMin, rect.yMax + 5f, rect.width * Mathf.Clamp01(i_value), 3f), tex);

                GUI.color = colorBackup;
            }

            GUILayout.Space(10f);
        }

        static public void DrawOutline(Rect rect, Color color)
        {
            DrawOutline(rect, color, 1f);
        }

        static public void LastRectOutline(Color color)
        {
            var lastRect = GUILayoutUtility.GetLastRect();
            DrawOutline(lastRect, color);
        }

        static public void LastRectOutline(Color color, float i_thickness)
        {
            var lastRect = GUILayoutUtility.GetLastRect();
            DrawOutline(lastRect, color, i_thickness);
        }

        static public void DrawOutline(Rect rect, Color color, float i_thickness)
        {
            if (Event.current.type == EventType.Repaint)
            {
                float halfThick = i_thickness * 0.5f;

                var colorBackup = GUI.color;

                Texture2D tex = WhiteTex;
                GUI.color = color;

                GUI.DrawTexture(Rect.MinMaxRect(rect.xMin - halfThick, rect.yMin - halfThick, rect.xMin + halfThick, rect.yMax + halfThick), tex);
                GUI.DrawTexture(Rect.MinMaxRect(rect.xMax - halfThick, rect.yMin - halfThick, rect.xMax + halfThick, rect.yMax + halfThick), tex);

                GUI.DrawTexture(Rect.MinMaxRect(rect.xMin - halfThick, rect.yMin - halfThick, rect.xMax + halfThick, rect.yMin + halfThick), tex);
                GUI.DrawTexture(Rect.MinMaxRect(rect.xMin - halfThick, rect.yMax - halfThick, rect.xMax + halfThick, rect.yMax + halfThick), tex);

                GUI.color = colorBackup;
            }
        }

        static public void DrawBox(Rect rect, Color color)
        {
            if (Event.current.type == EventType.Repaint)
            {
                var colorBackup = GUI.color;
                Texture2D tex = WhiteTex;
                GUI.color = color;
                GUI.DrawTexture(rect, tex);
                GUI.color = colorBackup;
            }
        }

        static public Rect DrawOutlineLayout(Color i_color, int i_width, int i_height)
        {
            GUILayout.Label("", GUILayout.Width(i_width), GUILayout.Height(i_height));
            var rect = GUILayoutUtility.GetLastRect();

            DrawOutline(rect, i_color);
            return rect;
        }

        static public Rect DrawTextureLayout(Texture2D i_tex)
        {
            return DrawTextureLayout(i_tex, i_tex.width, i_tex.height);
        }

        static public Rect DrawTextureLayout(Texture i_tex, int i_width, int i_height)
        {
            GUILayout.Label("", GUILayout.Width(i_width), GUILayout.Height(i_height));
            var rect = GUILayoutUtility.GetLastRect();

            if (Event.current.type == EventType.Repaint)
            {
                GUI.DrawTexture(rect, i_tex);
            }

            return rect;
        }

        static public Rect DrawTextureLayout(Texture i_tex, Color i_bgColor, int i_width, int i_height)
        {
            GUILayout.Label("", GUILayout.Width(i_width), GUILayout.Height(i_height));
            var rect = GUILayoutUtility.GetLastRect();

            if (Event.current.type == EventType.Repaint)
            {
                DrawBox(rect, i_bgColor);
                GUI.DrawTexture(rect, i_tex);
            }

            return rect;
        }

        public static void DrawCircle(float x, float y, float width, float height, Color c)
        {
            GUI.DrawTexture(new Rect(x, y, width, height), GetCircleTexture(c));
        }
    }

    public static class Unity_Rect_Extension_Method
    {
        public static Rect Merge(this Rect i_self, Rect i_rect)
        {
            if (i_self.center.x == 0f && i_self.center.y == 0f && i_self.width == 0f && i_self.height == 0f)
            {
                return i_rect;
            }

            return Rect.MinMaxRect(Mathf.Min(i_self.xMin, i_rect.xMin),
                                    Mathf.Min(i_self.yMin, i_rect.yMin),
                                    Mathf.Max(i_self.xMax, i_rect.xMax),
                                    Mathf.Max(i_self.yMax, i_rect.yMax));
        }

        public static Rect And(this Rect i_self, Rect i_rect)
        {
            if (i_self.center.x == 0f && i_self.center.y == 0f && i_self.width == 0f && i_self.height == 0f)
            {
                return i_rect;
            }

            return Rect.MinMaxRect(Mathf.Max(i_self.xMin, i_rect.xMin),
                                    Mathf.Max(i_self.yMin, i_rect.yMin),
                                    Mathf.Min(i_self.xMax, i_rect.xMax),
                                    Mathf.Min(i_self.yMax, i_rect.yMax));
        }

        public static Rect EnlargeRect(this Rect i_rect, float i_x, float i_y)
        {
            return Rect.MinMaxRect(i_rect.xMin - i_x, i_rect.yMin - i_y, i_rect.xMax + i_x, i_rect.yMax + i_y);
        }

        public static string MinMaxString(this Rect This)
        {
            return string.Format("(xMin:{0}, yMin:{1}, xMax:{2}, yMax:{3})", This.xMin, This.yMin, This.xMax, This.yMax);
        }

        public static string MinMaxString(this Rect This, string format)
        {
            return string.Format("(xMin:{0}, yMin:{1}, xMax:{2}, yMax:{3})", This.xMin.ToString(format), This.yMin.ToString(format), This.xMax.ToString(format), This.yMax.ToString(format));
        }
    }
}