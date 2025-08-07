public static class Tuple
{
    static class GenericHelper<T>
    {
        public static System.Func<T, int> CalcHashCode;
        public static System.Func<T, T, bool> EqualsT;

        static GenericHelper()
        {
            var typeInfo = typeof(T);
            if (typeInfo.IsClass ||
                typeInfo.IsInterface)
            {
                CalcHashCode = CalcClassHashCode;
                EqualsT = EqualsClass;
            }
            else
            {
                CalcHashCode = CalcStructHashCode;
                EqualsT = EqualsStruct;
            }
        }

        static bool IsNull(object value)
        {
            if (value == null)
            {
                return true;
            }

            return false;
        }

        static int CalcClassHashCode(T value)
        {
            if (value == null)
                return 0;

            return value.GetHashCode();
        }

        static bool EqualsClass(T lhs, T rhs)
        {
            var objL = lhs as object;
            var objR = rhs as object;

            var leftIsNull = IsNull(objL);
            var rightIsNull = IsNull(objR);

            if (leftIsNull != rightIsNull)
            {
                return false;
            }

            if (leftIsNull)
            {
                return true;
            }

            return Equals(lhs, rhs);
        }

        static int CalcStructHashCode(T value)
        {
            return value.GetHashCode();
        }

        static bool EqualsStruct(T lhs, T rhs)
        {
            return lhs.Equals(rhs);
        }
    }
    public static bool EqualsT<T>(T lhs, T rhs)
    {
        return GenericHelper<T>.EqualsT(lhs, rhs);
    }
    public static int CalcHashCode<T>(T value)
    {
        return GenericHelper<T>.CalcHashCode(value);
    }

    public static Tuple<T1> Create<T1>(T1 i_item1)
    {
        return new Tuple<T1>(i_item1);
    }

    public static Tuple<T1, T2> Create<T1, T2>(T1 i_item1, T2 i_item2)
    {
        return new Tuple<T1, T2>(i_item1, i_item2);
    }

    public static Tuple<T1, T2, T3> Create<T1, T2, T3>(T1 i_item1, T2 i_item2, T3 i_item3)
    {
        return new Tuple<T1, T2, T3>(i_item1, i_item2, i_item3);
    }

    public static Tuple<T1, T2, T3, T4> Create<T1, T2, T3, T4>(T1 i_item1, T2 i_item2, T3 i_item3, T4 i_item4)
    {
        return new Tuple<T1, T2, T3, T4>(i_item1, i_item2, i_item3, i_item4);
    }

    public static Tuple<T1, T2, T3, T4, T5> Create<T1, T2, T3, T4, T5>(T1 i_item1, T2 i_item2, T3 i_item3, T4 i_item4, T5 i_item5)
    {
        return new Tuple<T1, T2, T3, T4, T5>(i_item1, i_item2, i_item3, i_item4, i_item5);
    }
}

public class Tuple<T1>
{
    public T1 Item1;
    public Tuple(T1 t1)
    {
        Item1 = t1;
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;

        var other = obj as Tuple<T1>;
        if (other == null)
            return false;

        return Tuple.EqualsT(Item1, other.Item1);
    }

    public override int GetHashCode()
    {
        return Tuple.CalcHashCode(Item1);
    }
}

public class Tuple<T1, T2>
{
    public T1 Item1;
    public T2 Item2;

    public Tuple()
    {
    }

    public Tuple(T1 t1, T2 t2)
    {
        Item1 = t1;
        Item2 = t2;
    }

    public Tuple(Tuple<T1, T2> other)
    {
        Item1 = other.Item1;
        Item2 = other.Item2;
    }

    public void Set(T1 t1, T2 t2)
    {
        Item1 = t1;
        Item2 = t2;
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;

        var other = obj as Tuple<T1, T2>;
        if (other == null)
            return false;

        return Tuple.EqualsT(Item1, other.Item1) &&
               Tuple.EqualsT(Item2, other.Item2);
    }

    public override int GetHashCode()
    {
        return Tuple.CalcHashCode(Item1) ^ Tuple.CalcHashCode(Item2);
    }
}

public class Tuple<T1, T2, T3>
{
    public T1 Item1;
    public T2 Item2;
    public T3 Item3;

    public Tuple()
    { }

    public Tuple(T1 t1, T2 t2, T3 t3)
    {
        Item1 = t1;
        Item2 = t2;
        Item3 = t3;
    }

    public Tuple(Tuple<T1, T2, T3> other)
    {
        Item1 = other.Item1;
        Item2 = other.Item2;
        Item3 = other.Item3;
    }

    public void Set(T1 t1, T2 t2, T3 t3)
    {
        Item1 = t1;
        Item2 = t2;
        Item3 = t3;
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;

        var other = obj as Tuple<T1, T2, T3>;
        if (other == null)
            return false;

        return Tuple.EqualsT(Item1, other.Item1) &&
               Tuple.EqualsT(Item2, other.Item2) &&
               Tuple.EqualsT(Item3, other.Item3);
    }

    public override int GetHashCode()
    {
        return Tuple.CalcHashCode(Item1) ^
               Tuple.CalcHashCode(Item2) ^
               Tuple.CalcHashCode(Item3);
    }
}

public class Tuple<T1, T2, T3, T4>
{
    public T1 Item1;
    public T2 Item2;
    public T3 Item3;
    public T4 Item4;
    public Tuple(T1 t1, T2 t2, T3 t3, T4 t4)
    {
        Item1 = t1;
        Item2 = t2;
        Item3 = t3;
        Item4 = t4;
    }
    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;

        var other = obj as Tuple<T1, T2, T3, T4>;
        if (other == null)
            return false;

        return Tuple.EqualsT(Item1, other.Item1) &&
               Tuple.EqualsT(Item2, other.Item2) &&
               Tuple.EqualsT(Item3, other.Item3) &&
               Tuple.EqualsT(Item4, other.Item4);
    }

    public override int GetHashCode()
    {
        return Tuple.CalcHashCode(Item1) ^
               Tuple.CalcHashCode(Item2) ^
               Tuple.CalcHashCode(Item3) ^
               Tuple.CalcHashCode(Item4);
    }
}

public class Tuple<T1, T2, T3, T4, T5>
{
    public T1 Item1;
    public T2 Item2;
    public T3 Item3;
    public T4 Item4;
    public T5 Item5;
    public Tuple(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
    {
        Item1 = t1;
        Item2 = t2;
        Item3 = t3;
        Item4 = t4;
        Item5 = t5;
    }
    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;

        var other = obj as Tuple<T1, T2, T3, T4, T5>;
        if (other == null)
            return false;

        return Tuple.EqualsT(Item1, other.Item1) &&
               Tuple.EqualsT(Item2, other.Item2) &&
               Tuple.EqualsT(Item3, other.Item3) &&
               Tuple.EqualsT(Item4, other.Item4) &&
               Tuple.EqualsT(Item5, other.Item5);
    }

    public override int GetHashCode()
    {
        return Tuple.CalcHashCode(Item1) ^
               Tuple.CalcHashCode(Item2) ^
               Tuple.CalcHashCode(Item3) ^
               Tuple.CalcHashCode(Item4) ^
               Tuple.CalcHashCode(Item5);
    }


}
public class Tuple<T1, T2, T3, T4, T5, T6>
{
    public T1 Item1;
    public T2 Item2;
    public T3 Item3;
    public T4 Item4;
    public T5 Item5;
    public T6 Item6;
    public Tuple(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
    {
        Item1 = t1;
        Item2 = t2;
        Item3 = t3;
        Item4 = t4;
        Item5 = t5;
        Item6 = t6;
    }
    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;

        var other = obj as Tuple<T1, T2, T3, T4, T5, T6>;
        if (other == null)
            return false;

        return Tuple.EqualsT(Item1, other.Item1) &&
               Tuple.EqualsT(Item2, other.Item2) &&
               Tuple.EqualsT(Item3, other.Item3) &&
               Tuple.EqualsT(Item4, other.Item4) &&
               Tuple.EqualsT(Item5, other.Item5) &&
               Tuple.EqualsT(Item6, other.Item6);
    }

    public override int GetHashCode()
    {
        return Tuple.CalcHashCode(Item1) ^
               Tuple.CalcHashCode(Item2) ^
               Tuple.CalcHashCode(Item3) ^
               Tuple.CalcHashCode(Item4) ^
               Tuple.CalcHashCode(Item5) ^
               Tuple.CalcHashCode(Item6);
    }
}