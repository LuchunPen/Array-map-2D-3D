/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 23.01.2016 10:09:41
*/

using System;
namespace Nano3
{
    [Serializable]
    public struct Vector2I : IEquatable<Vector2I>
    {
        //private static readonly string stringUID = "FB8EF499DB217F01";

        public int X;
        public int Y;

        public Vector2I(int x, int y)
        {
            X = x; Y = y;
        }

        public Vector2I(float x, float y)
        {
            X = (int)Math.Floor(x); Y = (int)Math.Floor(y);
        }

        public Vector2I(double x, double y)
        {
            X = (int)Math.Floor(x); Y = (int)Math.Floor(y);
        }

        #region Operators
        public static Vector2I operator *(Vector2I v, float f)
        {

            v.X = (int)Math.Floor(v.X * f);
            v.Y = (int)Math.Floor(v.Y * f);
            return v;
        }
        public static Vector2I operator *(Vector2I v, int f)
        {
            v.X *= f;
            v.Y *= f;
            return v;
        }
        public static Vector2I operator *(Vector2I v1, Vector2I v2)
        {
            v1.X *= v2.X;
            v1.Y *= v2.Y;
            return v1;
        }

        public static Vector2I operator /(Vector2I v, float f)
        {
            v.X = (int)Math.Floor(v.X / f);
            v.Y = (int)Math.Floor(v.X / f);
            return v;
        }
        public static Vector2I operator /(Vector2I v, int f)
        {
            v.X /= f;
            v.Y /= f;
            return v;
        }

        public static Vector2I operator +(Vector2I v1, Vector2I v2)
        {
            v1.X += v2.X;
            v1.Y += v2.Y;
            return v1;
        }
        public static Vector2I operator -(Vector2I v1, Vector2I v2)
        {
            v1.X -= v2.X;
            v1.Y -= v2.Y;
            return v1;
        }

        public static bool operator ==(Vector2I v1, Vector2I v2)
        {
            if (v1.X == v2.X) return v1.Y == v2.Y;
            else return false;
        }
        public static bool operator !=(Vector2I v1, Vector2I v2)
        {
            if (v1.X != v2.X || v1.Y != v2.Y) return true;
            return false;
        }

        public static implicit operator Vector2I(Vector3I v)
        {
            return new Vector2I(v.X, v.Y);
        }
        #endregion Operators

        #region Dist
        public static float Distance(Vector2I v1, Vector2I v2)
        {
            float f1 = v1.X - v2.X; float f2 = v1.Y - v2.Y;
            return (float)Math.Sqrt((f1 * f1) + (f2 * f2));
        }
        public static void Distance(ref Vector2I v1, ref Vector2I v2, out float f)
        {
            float f1 = v1.X - v2.X; float f2 = v1.Y - v2.Y;
            f = (float)Math.Sqrt((f1 * f1) + (f2 * f2));
        }

        public static float DistanceSQR(Vector2I v1, Vector2I v2)
        {
            float f1 = v1.X - v2.X; float f2 = v1.Y - v2.Y;
            return (f1 * f1) + (f2 * f2);
        }
        public static void DistanceSQR(ref Vector2I v1, ref Vector2I v2, out float f)
        {
            float f1 = v1.X - v2.X; float f2 = v1.Y - v2.Y;
            f = (f1 * f1) + (f2 * f2);
        }

        public static int ManhattanDistance(Vector2I v1, Vector2I v2)
        {
            return Math.Abs(v1.X - v2.X) + Math.Abs(v1.Y - v2.Y);
        }
        public static int ManhattanDistance(ref Vector2I v1, ref Vector2I v2)
        {
            return Math.Abs(v1.X - v2.X) + Math.Abs(v1.Y - v2.Y);
        }
        #endregion Dist

        public bool Equals(Vector2I other)
        {
            return (X == other.X) && (Y == other.Y);
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector2I) { return Equals((Vector2I)obj); }
            return false;
        }

        public override int GetHashCode()
        {
            return X * 32768 + Y;
        }

        public override string ToString()
        {
            return string.Format("X:{0}, Y:{1}", X, Y);
        }
    }
}
