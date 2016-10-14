/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 28.07.2016 21:43:07
*/

using System;

namespace Nano3
{
    [Serializable]
    public struct XYZ64 : IEquatable<XYZ64>
    {
        public const long FailIndex = -1;
        public const long MAX_VALUE = 1048575;
        public const long MIN_VALUE = -1048576;

        private const long ADDVAL = 1048576;
        private const long MASK_VALUE = ADDVAL + MAX_VALUE;
        private const int SHIFT_X = 0;
        private const int SHIFT_Y = 21;
        private const int SHIFT_Z = 42;

        private const long CL_MASK_X = long.MaxValue & (~(MASK_VALUE << SHIFT_X));
        private const long CL_MASK_Y = long.MaxValue & (~(MASK_VALUE << SHIFT_Y));
        private const long CL_MASK_Z = long.MaxValue & (~(MASK_VALUE << SHIFT_Z));

        public long Index;

        public int X
        {
            get { return (int)((MASK_VALUE & Index) - ADDVAL); }
            set { Index = ((Index & CL_MASK_X) | (MASK_VALUE & (value + ADDVAL))); }
        }
        public int Y
        {
            get { return (int)((MASK_VALUE & Index >> SHIFT_Y) - ADDVAL); }
            set { Index = ((Index & CL_MASK_Y) | (MASK_VALUE & (value + ADDVAL)) << SHIFT_Y); }
        }
        public int Z
        {
            get { return (int)((MASK_VALUE & Index >> SHIFT_Z) - ADDVAL); }
            set { Index = ((Index & CL_MASK_Z) | (MASK_VALUE & (value + ADDVAL)) << SHIFT_Z); }
        }

        public Vector3I Position
        {
            get { return new Vector3I(X, Y, Z); }
        }

        private XYZ64(long index)
        {
            Index = index;
        }

        public XYZ64(int x, int y, int z)
        {
            Index = (MASK_VALUE & (x + ADDVAL))
                | (MASK_VALUE & (y + ADDVAL)) << SHIFT_Y
                | (MASK_VALUE & (z + ADDVAL)) << SHIFT_Z;
        }
        public XYZ64(Vector3I position)
        {
            Index = (MASK_VALUE & (position.X + ADDVAL))
                | (MASK_VALUE & (position.Y + ADDVAL)) << SHIFT_Y
                | (MASK_VALUE & (position.Z + ADDVAL)) << SHIFT_Z;
        }

        public static explicit operator long (XYZ64 coord)
        {
            return coord.Index;
        }
        public static explicit operator XYZ64(long index)
        {
            if (index < 0) { return new XYZ64(FailIndex); }
            return new XYZ64(index);
        }

        public bool Equals(XYZ64 other)
        {
            return other.Index == Index;
        }

        public static bool operator ==(XYZ64 v1, XYZ64 v2)
        {
            return v1.Index == v2.Index;
        }
        public static bool operator !=(XYZ64 v1, XYZ64 v2)
        {
            return v1.Index != v2.Index;
        }

        public override bool Equals(object obj)
        {
            if (obj is XYZ64)
            {
                XYZ64 val = (XYZ64)obj;
                return val.Index == Index;
            }
            else return false;
        }
        public override int GetHashCode()
        {
            return ((int)Index) ^ (int)(Index >> 32);
        }
        public override string ToString()
        {
            return "X: " + X + ", Y: " + Y + ", Z: " + Z;
        }
    }
}
