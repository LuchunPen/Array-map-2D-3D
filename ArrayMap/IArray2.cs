/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 28.07.2016 19:26:45
*/

using System;

namespace Nano3.Map
{
    public interface IMap2<TValue>
    {
        int XSize { get; }
        int YSize { get; }
        int Size { get; }

        TValue this[int index] { get; set; }
        TValue this[int x, int y] { get;  set; }

        int ToIndex(int x, int y);
        Vector2I ToMapPosition(int index);

        bool IsBounded(Vector2I position);
        void Clear();
    }

    public interface IExchangable2<TValue>
    {
        TValue Exchange(int x, int y, TValue value);
    }
}
