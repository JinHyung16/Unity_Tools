using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static bool IsNullOrEmpty<T>(this T obj) where T : ICollection
    {
        return obj == null || obj.Count == 0;
    }

    public static bool IsNullOrEmpty<T>(this T[] objs)
    {
        return objs == null || objs.Length == 0;
    }

    public static bool IsValidIndex<T>(this T[] objs, int index)
    {
        if (index < 0 ||
            index >= objs.COUNT())
            return false;

        return true;
    }

    public static int COUNT<T>(this T obj) where T : ICollection
    {
        if (obj == null)
            return 0;

        return obj.Count;
    }

    public static int COUNT<T>(this T[] objs)
    {
        if (objs == null)
            return 0;

        return objs.Length;
    }

    public static void Fire(this Action This)
    {
        if (This == null)
        {
            return;
        }

        This.Invoke();
    }

    public static void Fire<T1>(this Action<T1> This, T1 t1)
    {
        if (This == null)
        {
            return;
        }

        This.Invoke(t1);
    }
    public static void Fire<T1, T2>(this Action<T1, T2> This, T1 t1, T2 t2)
    {
        if (This == null)
        {
            return;
        }

        This.Invoke(t1, t2);
    }
    public static void Fire<T1, T2, T3>(this Action<T1, T2, T3> This, T1 t1, T2 t2, T3 t3)
    {
        if (This == null)
        {
            return;
        }

        This.Invoke(t1, t2, t3);
    }
    public static void Fire<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> This, T1 t1, T2 t2, T3 t3, T4 t4)
    {
        if (This == null)
        {
            return;
        }

        This.Invoke(t1, t2, t3, t4);
    }
    public static void Fire<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> This, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
    {
        if (This == null)
        {
            return;
        }

        This.Invoke(t1, t2, t3, t4, t5);
    }

    public static TReturn Fire<TReturn>(this Func<TReturn> This)
    {
        if (This == null)
            return default;

        return This.Invoke();
    }

    public static TReturn Fire<T1, TReturn>(this Func<T1, TReturn> This, T1 t1)
    {
        if (This == null)
            return default;

        return This.Invoke(t1);
    }

    public static TReturn Fire<T1, T2, TReturn>(this Func<T1, T2, TReturn> This, T1 t1, T2 t2)
    {
        if (This == null)
            return default;

        return This.Invoke(t1, t2);
    }
    public static TReturn Fire<T1, T2, T3, TReturn>(this Func<T1, T2, T3, TReturn> This, T1 t1, T2 t2, T3 t3)
    {
        if (This == null)
            return default;

        return This.Invoke(t1, t2, t3);
    }
    public static TReturn Fire<T1, T2, T3, T4, TReturn>(this Func<T1, T2, T3, T4, TReturn> This, T1 t1, T2 t2, T3 t3, T4 t4)
    {
        if (This == null)
            return default;

        return This.Invoke(t1, t2, t3, t4);
    }

    public static TReturn Fire<T1, T2, T3, T4, T5, TReturn>(this Func<T1, T2, T3, T4, T5, TReturn> This, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
    {
        if (This == null)
            return default;

        return This.Invoke(t1, t2, t3, t4, t5);
    }
}
public static class ICollectionExtension
{
    public static bool IsNullOrEmpty<T>(this ICollection<T> i_this)
    {
        if (i_this == null)
        {
            return true;
        }

        return (i_this.Count == 0);
    }

    public static int COUNT<T>(this ICollection<T> i_this)
    {
        if (i_this == null)
        {
            return 0;
        }

        return i_this.Count;
    }

    public static void CLEAR<T>(this ICollection<T> i_this)
    {
        if (i_this == null)
        {
            return;
        }

        i_this.Clear();
    }
}

public static class StringExtension
{
    public static bool IsNullOrEmpty(this string str)
    {
        return string.IsNullOrEmpty(str);
    }
}