using UnityEngine;
using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;

// enum과 1:1로 대응되는 배열을 생성하고 다루기위한 클래스
public class EnumArray<TEnum, T> : IEnumerable<T>
    where TEnum : struct, IComparable, IFormattable, IConvertible // it means enum
{
    public T[] Array
    {
        get;
        private set;
    }

    public int Length
    {
        get
        {
            return Array.Length;
        }
    }

    public TEnum[] Keys
    {
        get;
        private set;
    }

    public EnumArray()
    {
        Keys = Enum.GetValues(typeof(TEnum)) as TEnum[];
        Array = new T[Keys.Length];
    }

    public bool IsDefined(string type)
    {
        return Enum.IsDefined(typeof(TEnum), type);
    }

    public T Get(string type)
    {
        if (!IsDefined(type))
            throw new ArgumentException("Try to get invalid data type", "type");

        return Array[(int)Enum.Parse(typeof(TEnum), type, true)];
    }

    public T Get(TEnum type)
    {
        //return Array[type.ToInt32(NumberFormatInfo.InvariantInfo)];
        return Array[Convert.ToInt32(type)];
    }

    public void Set(string type, T value)
    {
        if (!IsDefined(type))
            throw new ArgumentException("Try to set invalid data type", "type");

        Array[(int)Enum.Parse(typeof(TEnum), type, true)] = value;
    }

    public void Set(TEnum type, T value)
    {
        //Array[type.ToInt32(NumberFormatInfo.InvariantInfo)] = value;
        Array[Convert.ToInt32(type)] = value;
    }

    public void Set(EnumArray<TEnum, T> other)
    {
        System.Array.Copy(other.Array, Array, Array.Length);
    }

    public void SetAll(T value)
    {
        for (int i = 0; i < Array.Length; i++)
            Array[i] = value;

    }

    public IEnumerator<T> GetEnumerator()
    {
        return ((IEnumerable<T>)Array).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return Array.GetEnumerator();
    }

    public T this[TEnum key]
    {
        get
        {
            return Array[Convert.ToInt32(key)];
        }
        set
        {
            Array[Convert.ToInt32(key)] = value;
        }
    }
}