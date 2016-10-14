/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 23.01.2016 8:15:33
*/

using System;

namespace Nano3
{
    [Serializable]
    public struct Vector3I : IEquatable<Vector3I>
    {
        //private static readonly string stringUID = "BB8EF4BBF1FF8E02";
        
        public int X; 
        public int Y; 
        public int Z;

        public Vector3I(int x, int y, int z)
        {
            X = x; Y = y; Z = z;
        }
        public Vector3I(float x, float y, float z)
        {
            X = (int)Math.Floor(x); Y = (int)Math.Floor(y); Z = (int)Math.Floor(z);
        }
        public Vector3I(double x, double y, double z)
        {
            X = (int)Math.Floor(x); Y = (int)Math.Floor(y); Z = (int)Math.Floor(z);
        }

        #region Operators

        public static Vector3I operator *(Vector3I v, float f)
        {
            v.X = (int)Math.Floor(v.X * f);
            v.Y = (int)Math.Floor(v.Y * f);
            v.Z = (int)Math.Floor(v.Z * f);
            return v;
        }
        public static Vector3I operator *(Vector3I v, int f)
        {
            v.X *= f;
            v.Y *= f;
            v.Z *= f;
            return v;
        }

        public static Vector3I operator /(Vector3I v, float f)
        {
            float fmod = 1f / f;
            v.X = (int)Math.Floor(v.X * fmod);
            v.Y = (int)Math.Floor(v.X * fmod);
            v.Z = (int)Math.Floor(v.Z * fmod);
            return v;
        }
        public static Vector3I operator /(Vector3I v, int f)
        {
            v.X /= f;
            v.Y /= f;
            v.Z /= f;
            return v;
        }

        public static Vector3I operator +(Vector3I v1, Vector3I v2)
        {
            v1.X += v2.X;
            v1.Y += v2.Y;
            v1.Z += v2.Z;
            return v1;
        }
        public static Vector3I operator -(Vector3I v1, Vector3I v2)
        {
            v1.X -= v2.X;
            v1.Y -= v2.Y;
            v1.Z -= v2.Z;
            return v1;
        }

        public static bool operator ==(Vector3I v1, Vector3I v2)
        {
            if (v1.X == v2.X && v1.Y == v2.Y) return v1.Z == v2.Z;
            return false;
        }
        public static bool operator !=(Vector3I v1, Vector3I v2)
        {
            if (v1.X != v2.X || v1.Y != v2.Y || v1.Z != v2.Z) return true;
            return false;
        }
        #endregion Operators

        #region CalcFunc

        public static float Distance(Vector3I v1, Vector3I v2)
        {
            float f1 = v1.X - v2.X;
            float f2 = v1.Y - v2.Y;
            float f3 = v1.Z - v2.Z;
            return (float)Math.Sqrt((f1 * f1) + (f2 * f2) + (f3 * f3));
        }
        public static void Distance(ref Vector3I v1, ref Vector3I v2, out float f)
        {
            float f1 = v1.X - v2.X;
            float f2 = v1.Y - v2.Y;
            float f3 = v1.Z - v2.Z;
            f = (float)Math.Sqrt((f1 * f1) + (f2 * f2) + (f3 * f3));
        }

        public static float DistanceSQR(Vector3I v1, Vector3I v2)
        {
            float f1 = v1.X - v2.X;
            float f2 = v1.Y - v2.Y;
            float f3 = v1.Z - v2.Z;
            return ((f1 * f1) + (f2 * f2) + (f3 * f3));
        }
        public static void DistanceSQR(ref Vector3I v1, ref Vector3I v2, out float f)
        {
            float f1 = v1.X - v2.X;
            float f2 = v1.Y - v2.Y;
            float f3 = v1.Z - v2.Z;
            f = ((f1 * f1) + (f2 * f2) + (f3 * f3));
        }

        public static int ManhattanDistance(Vector3I v1, Vector3I v2)
        {
            return Math.Abs(v1.X - v2.X) + Math.Abs(v1.Y - v2.Y) + Math.Abs(v1.Z - v2.Z);
        }
        public static int ManhattanDistance(ref Vector3I v1, ref Vector3I v2)
        {
            return Math.Abs(v1.X - v2.X) + Math.Abs(v1.Y - v2.Y) + Math.Abs(v1.Z - v2.Z);
        }
        #endregion CalcFunc

        public bool Equals(Vector3I other)
        {
            return (X == other.X) && (Y == other.Y) && (Z == other.Z);
        }
        public override int GetHashCode()
        {
            return 1024 * (1024 * Z + X) + Y;
        }
        public override bool Equals(object obj)
        {
            if (obj is Vector3I) { return Equals((Vector3I)obj); }
            return false;
        }
        public override string ToString()
        {
            return string.Format("X:{0}, Y:{1}, Z:{2}", X, Y, Z);
        }
    }
}
