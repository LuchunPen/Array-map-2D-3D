/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 28.07.2016 20:24:45
*/

using System;

namespace Nano3.Map
{
    public interface IMap3<TValue>
    {
        int XSize { get; }
        int YSize { get; }
        int ZSize { get; }

        TValue this[XYZ64 coord] { get; set; }
        TValue this[int px, int py, int pz] { get; set; }

        bool IsBounded(Vector3I position);
        void Clear();
    }

    public interface IExchangable3<TValue>
    {
        TValue Exchange(XYZ64 coord, TValue value);
    }
}
