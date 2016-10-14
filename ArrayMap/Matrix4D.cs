/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 18.09.2016 22:27:07
*/

// Invert Matrix4D =============================================================
//
//     -1       1      
//    M   = --------- A
//            det(M)
//
// A is adjugate (adjoint) of M, where,
//
//      T
// A = C
//
// C is Cofactor matrix of M, where,
//           i + j
// C   = (-1)      * det(M  )
//  ij                    ij
//
//     [ a b c d ]
// M = [ e f g h ]
//     [ i j k l ]
//     [ m n o p ]
//
// First Row
//           2 | f g h |
// C   = (-1)  | j k l | = + ( f ( kp - lo ) - g ( jp - ln ) + h ( jo - kn ) )
//  11         | n o p |
//
//           3 | e g h |
// C   = (-1)  | i k l | = - ( e ( kp - lo ) - g ( ip - lm ) + h ( io - km ) )
//  12         | m o p |
//
//           4 | e f h |
// C   = (-1)  | i j l | = + ( e ( jp - ln ) - f ( ip - lm ) + h ( in - jm ) )
//  13         | m n p |
//
//           5 | e f g |
// C   = (-1)  | i j k | = - ( e ( jo - kn ) - f ( io - km ) + g ( in - jm ) )
//  14         | m n o |
//
// Second Row
//           3 | b c d |
// C   = (-1)  | j k l | = - ( b ( kp - lo ) - c ( jp - ln ) + d ( jo - kn ) )
//  21         | n o p |
//
//           4 | a c d |
// C   = (-1)  | i k l | = + ( a ( kp - lo ) - c ( ip - lm ) + d ( io - km ) )
//  22         | m o p |
//
//           5 | a b d |
// C   = (-1)  | i j l | = - ( a ( jp - ln ) - b ( ip - lm ) + d ( in - jm ) )
//  23         | m n p |
//
//           6 | a b c |
// C   = (-1)  | i j k | = + ( a ( jo - kn ) - b ( io - km ) + c ( in - jm ) )
//  24         | m n o |
//
// Third Row
//           4 | b c d |
// C   = (-1)  | f g h | = + ( b ( gp - ho ) - c ( fp - hn ) + d ( fo - gn ) )
//  31         | n o p |
//
//           5 | a c d |
// C   = (-1)  | e g h | = - ( a ( gp - ho ) - c ( ep - hm ) + d ( eo - gm ) )
//  32         | m o p |
//
//           6 | a b d |
// C   = (-1)  | e f h | = + ( a ( fp - hn ) - b ( ep - hm ) + d ( en - fm ) )
//  33         | m n p |
//
//           7 | a b c |
// C   = (-1)  | e f g | = - ( a ( fo - gn ) - b ( eo - gm ) + c ( en - fm ) )
//  34         | m n o |
//
// Fourth Row
//           5 | b c d |
// C   = (-1)  | f g h | = - ( b ( gl - hk ) - c ( fl - hj ) + d ( fk - gj ) )
//  41         | j k l |
//
//           6 | a c d |
// C   = (-1)  | e g h | = + ( a ( gl - hk ) - c ( el - hi ) + d ( ek - gi ) )
//  42         | i k l |
//
//           7 | a b d |
// C   = (-1)  | e f h | = - ( a ( fl - hj ) - b ( el - hi ) + d ( ej - fi ) )
//  43         | i j l |
//
//           8 | a b c |
// C   = (-1)  | e f g | = + ( a ( fk - gj ) - b ( ek - gi ) + c ( ej - fi ) )
//  44         | i j k |
//=============================================================================


//Transpose Matrix4D ==========================================================
//
// | a b c d |     | f g h |     | e g h |     | e f h |     | e f g |
// | e f g h | = a | j k l | - b | i k l | + c | i j l | - d | i j k |
// | i j k l |     | n o p |     | m o p |     | m n p |     | m n o |
// | m n o p |
//
//   | f g h |
// a | j k l | = a ( f ( kp - lo ) - g ( jp - ln ) + h ( jo - kn ) )
//   | n o p |
//
//   | e g h |     
// b | i k l | = b ( e ( kp - lo ) - g ( ip - lm ) + h ( io - km ) )
//   | m o p |     
//
//   | e f h |
// c | i j l | = c ( e ( jp - ln ) - f ( ip - lm ) + h ( in - jm ) )
//   | m n p |
//
//   | e f g |
// d | i j k | = d ( e ( jo - kn ) - f ( io - km ) + g ( in - jm ) )
//   | m n o |
//
//===============================================================================

//Matrix4D RotX = new Matrix4D(
//                1, 0, 0, 0,
//                0, Math.Cos(MathEx.Deg2Rad *  A), Math.Sin(MathEx.Deg2Rad * A), 0,
//                0, -Math.Sin(MathEx.Deg2Rad * A), Math.Cos(MathEx.Deg2Rad * A), 0,
//                0, 0, 0, 1
//            );

//Matrix4D RotY = new Matrix4D(
//                Math.Cos(MathEx.Deg2Rad * A), 0, -Math.Sin(MathEx.Deg2Rad * A), 0,
//                0,1,0,0,
//                Math.Sin(MathEx.Deg2Rad * A), 0, Math.Cos(MathEx.Deg2Rad * A), 0,
//                0,0,0,1
//           );

//Matrix4D RotZ = new Matrix4D(
//                Math.Cos(MathEx.Deg2Rad * A), Math.Sin(MathEx.Deg2Rad * A), 0, 0,
//                -Math.Sin(MathEx.Deg2Rad * A), Math.Cos(MathEx.Deg2Rad * A), 0, 0,
//                0, 0, 1, 0,
//                0, 0, 0, 1
//            );

using System;
using System.Text;
using System.Runtime.Serialization;

namespace Nano3
{
    [Serializable]
    public struct Matrix4D : ISerializable, ICloneable, IEquatable<Matrix4D>
    {
        public const float Deg2Rad = 0.0174532924f;
        public const float Rad2Deg = 57.29578f;

        public static readonly Matrix4D Zero = new Matrix4D(
            0, 0, 0, 0,
            0, 0, 0, 0, 
            0, 0, 0, 0, 
            0, 0, 0, 0);

        public static readonly Matrix4D Identity = new Matrix4D(
            1, 0, 0, 0,
            0, 1, 0, 0,
            0, 0, 1, 0,
            0, 0, 0, 1);
        public static readonly Matrix4D NaN = new Matrix4D(
            double.NaN, double.NaN, double.NaN, double.NaN,
            double.NaN, double.NaN, double.NaN, double.NaN,
            double.NaN, double.NaN, double.NaN, double.NaN,
            double.NaN, double.NaN, double.NaN, double.NaN
            );

        public double M11, M12, M13, M14;
        public double M21, M22, M23, M24;
        public double M31, M32, M33, M34;
        public double M41, M42, M43, M44;

        #region Constructors

        public Matrix4D(
            double m11, double m12, double m13, double m14,
            double m21, double m22, double m23, double m24,
            double m31, double m32, double m33, double m34,
            double m41, double m42, double m43, double m44
            )
        {
            M11 = m11; M12 = m12; M13 = m13; M14 = m14;
            M21 = m21; M22 = m22; M23 = m23; M24 = m24;
            M31 = m31; M32 = m32; M33 = m33; M34 = m34;
            M41 = m41; M42 = m42; M43 = m43; M44 = m44;
        }

        private Matrix4D(Matrix4D m)
        {
            M11 = m.M11; M12 = m.M12; M13 = m.M13; M14 = m.M14;
            M21 = m.M21; M22 = m.M22; M23 = m.M23; M24 = m.M24;
            M31 = m.M31; M32 = m.M32; M33 = m.M33; M34 = m.M34;
            M41 = m.M41; M42 = m.M42; M43 = m.M43; M44 = m.M44;
        }

        /// <summary>
		/// Initializes a new instance of the <see cref="Matrix4D"/> class with serialized data.
		/// </summary>
		/// <param name="info">The object that holds the serialized object data.</param>
		/// <param name="context">The contextual information about the source or destination.</param>
		private Matrix4D(SerializationInfo info, StreamingContext context)
        {
            // Get the first row
            M11 = info.GetSingle("M11");
            M12 = info.GetSingle("M12");
            M13 = info.GetSingle("M13");
            M14 = info.GetSingle("M14");

            // Get the second row
            M21 = info.GetSingle("M21");
            M22 = info.GetSingle("M22");
            M23 = info.GetSingle("M23");
            M24 = info.GetSingle("M24");

            // Get the third row
            M31 = info.GetSingle("M31");
            M32 = info.GetSingle("M32");
            M33 = info.GetSingle("M33");
            M34 = info.GetSingle("M34");

            // Get the fourth row
            M41 = info.GetSingle("M41");
            M42 = info.GetSingle("M42");
            M43 = info.GetSingle("M43");
            M44 = info.GetSingle("M44");
        }

        #endregion Constructors

        public Matrix4D Transpose
        {
            get {
                return new Matrix4D(
                    M11, M21, M31, M41,
                    M12, M22, M32, M42,
                    M13, M23, M33, M43,
                    M14, M24, M34, M44
                    );
            }
        }
        public double Determinant()
        {
                double kp_lo = M33 * M44 - M34 * M43;
                double jp_ln = M32 * M44 - M34 * M42;
                double jo_kn = M32 * M43 - M33 * M42;
                double ip_lm = M31 * M44 - M34 * M41;
                double io_km = M31 * M43 - M33 * M41;
                double in_jm = M31 * M42 - M32 * M41;

                return M11 * (M22 * kp_lo - M23 * jp_ln + M24 * jo_kn) -
                       M12 * (M21 * kp_lo - M23 * ip_lm + M24 * io_km) +
                       M13 * (M21 * jp_ln - M22 * ip_lm + M24 * in_jm) -
                       M14 * (M21 * jo_kn - M22 * io_km + M23 * in_jm);
        }
        public Matrix4D Invert()
        {
            double a = M11, b = M12, c = M13, d = M14;
            double e = M21, f = M22, g = M23, h = M24;
            double i = M31, j = M32, k = M33, l = M34;
            double m = M41, n = M42, o = M43, p = M44;

            double kp_lo = k * p - l * o;
            double jp_ln = j * p - l * n;
            double jo_kn = j * o - k * n;
            double ip_lm = i * p - l * m;
            double io_km = i * o - k * m;
            double in_jm = i * n - j * m;

            double a11 = (f * kp_lo - g * jp_ln + h * jo_kn);
            double a12 = -(e * kp_lo - g * ip_lm + h * io_km);
            double a13 = (e * jp_ln - f * ip_lm + h * in_jm);
            double a14 = -(e * jo_kn - f * io_km + g * in_jm);

            double D = a * a11 + b * a12 + c * a13 + d * a14;
            if (Math.Abs(D) < double.Epsilon) { return NaN; }
            double md = 1 / D;

            double gp_ho = g * p - h * o;
            double fp_hn = f * p - h * n;
            double fo_gn = f * o - g * n;
            double ep_hm = e * p - h * m;
            double eo_gm = e * o - g * m;
            double en_fm = e * n - f * m;

            double gl_hk = g * l - h * k;
            double fl_hj = f * l - h * j;
            double fk_gj = f * k - g * j;
            double el_hi = e * l - h * i;
            double ek_gi = e * k - g * i;
            double ej_fi = e * j - f * i;

            return new Matrix4D(
                a11 * md,                                   // m11
                -(b * kp_lo - c * jp_ln + d * jo_kn) * md,  // m12
                (b * gp_ho - c * fp_hn + d * fo_gn) * md,   // m13
                -(b * gl_hk - c * fl_hj + d * fk_gj) * md,  // m14
                a12 * md,                                   // m21
                (a * kp_lo - c * ip_lm + d * io_km) * md,   // m22
                -(a * gp_ho - c * ep_hm + d * eo_gm) * md,  // m23
                (a * gl_hk - c * el_hi + d * ek_gi) * md,   // m24
                a13 * md,                                   // m31
                -(a * jp_ln - b * ip_lm + d * in_jm) * md,  // m32
                (a * fp_hn - b * ep_hm + d * en_fm) * md,   // m33
                -(a * fl_hj - b * el_hi + d * ej_fi) * md,  // m34
                a14 * md,                                   // m41
                (a * jo_kn - b * io_km + c * in_jm) * md,   // m42
                -(a * fo_gn - b * eo_gm + c * en_fm) * md,  // m43
                (a * fk_gj - b * ek_gi + c * ej_fi) * md    // m44
                );
        }

        public override int GetHashCode()
        {
            return
                M11.GetHashCode() ^ M12.GetHashCode() ^ M13.GetHashCode() ^ M14.GetHashCode() ^
                M21.GetHashCode() ^ M22.GetHashCode() ^ M23.GetHashCode() ^ M24.GetHashCode() ^
                M31.GetHashCode() ^ M32.GetHashCode() ^ M33.GetHashCode() ^ M34.GetHashCode() ^
                M41.GetHashCode() ^ M42.GetHashCode() ^ M43.GetHashCode() ^ M44.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj is Matrix4D) {
                Matrix4D m = (Matrix4D)obj;
                return
                    (M11 == m.M11) && (M12 == m.M12) && (M13 == m.M13) && (M14 == m.M14) &&
                    (M21 == m.M21) && (M22 == m.M22) && (M23 == m.M23) && (M24 == m.M24) &&
                    (M31 == m.M31) && (M32 == m.M32) && (M33 == m.M33) && (M34 == m.M34) &&
                    (M41 == m.M41) && (M42 == m.M42) && (M43 == m.M43) && (M44 == m.M44);
            }
            return false;
        }
        public bool Equals(Matrix4D other)
        {
            return
                (M11 == other.M11) && (M12 == other.M12) && (M13 == other.M13) && (M14 == other.M14) &&
                (M21 == other.M21) && (M22 == other.M22) && (M23 == other.M23) && (M24 == other.M24) &&
                (M31 == other.M31) && (M32 == other.M32) && (M33 == other.M33) && (M34 == other.M34) &&
                (M41 == other.M41) && (M42 == other.M42) && (M43 == other.M43) && (M44 == other.M44);
        }
        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            s.Append(String.Format("|{0}, {1}, {2}, {3}|\n", M11, M12, M13, M14));
            s.Append(String.Format("|{0}, {1}, {2}, {3}|\n", M21, M22, M23, M24));
            s.Append(String.Format("|{0}, {1}, {2}, {3}|\n", M31, M32, M33, M34));
            s.Append(String.Format("|{0}, {1}, {2}, {3}|\n", M41, M42, M43, M44));

            return s.ToString();
        }

        object ICloneable.Clone()
        {
            return new Matrix4D(this);
        }
        public Matrix4D Clone()
        {
            return new Matrix4D(this);
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // First row
            info.AddValue("M11", M11);
            info.AddValue("M12", M12);
            info.AddValue("M13", M13);
            info.AddValue("M14", M14);

            // Second row
            info.AddValue("M21", M21);
            info.AddValue("M22", M22);
            info.AddValue("M23", M23);
            info.AddValue("M24", M24);

            // Third row
            info.AddValue("M31", M31);
            info.AddValue("M32", M32);
            info.AddValue("M33", M33);
            info.AddValue("M34", M34);

            // Fourth row
            info.AddValue("M41", M41);
            info.AddValue("M42", M42);
            info.AddValue("M43", M43);
            info.AddValue("M44", M44);
        }

        public static Matrix4D Add(Matrix4D a, Matrix4D b)
        {
            return Add(ref a, ref b);
        }
        public static Matrix4D Add(ref Matrix4D a, ref Matrix4D b)
        {
            return new Matrix4D(
                a.M11 + b.M11, a.M12 + b.M12, a.M13 + b.M13, a.M14 + b.M14,
                a.M21 + b.M21, a.M22 + b.M22, a.M23 + b.M23, a.M24 + b.M24,
                a.M31 + b.M31, a.M32 + b.M32, a.M33 + b.M33, a.M34 + b.M34,
                a.M41 + b.M41, a.M42 + b.M42, a.M43 + b.M43, a.M44 + b.M44
                );
        }

        public static Matrix4D Subtract(Matrix4D a, Matrix4D b)
        {
            return Subtract(ref a, ref b);
        }
        public static Matrix4D Subtract(ref Matrix4D a, ref Matrix4D b)
        {
            return new Matrix4D(
                a.M11 - b.M11, a.M12 - b.M12, a.M13 - b.M13, a.M14 - b.M14,
                a.M21 - b.M21, a.M22 - b.M22, a.M23 - b.M23, a.M24 - b.M24,
                a.M31 - b.M31, a.M32 - b.M32, a.M33 - b.M33, a.M34 - b.M34,
                a.M41 - b.M41, a.M42 - b.M42, a.M43 - b.M43, a.M44 - b.M44
                );
        }

        public static Matrix4D Multiply(Matrix4D a, Matrix4D b)
        {
            return Multiply(ref a, ref b);
        }
        public static Matrix4D Multiply(ref Matrix4D a, ref Matrix4D b)
        {
            return new Matrix4D(
                a.M11 * b.M11 + a.M12 * b.M21 + a.M13 * b.M31 + a.M14 * b.M41,
                a.M11 * b.M12 + a.M12 * b.M22 + a.M13 * b.M32 + a.M14 * b.M42,
                a.M11 * b.M13 + a.M12 * b.M23 + a.M13 * b.M33 + a.M14 * b.M43,
                a.M11 * b.M14 + a.M12 * b.M24 + a.M13 * b.M34 + a.M14 * b.M44,

                a.M21 * b.M11 + a.M22 * b.M21 + a.M23 * b.M31 + a.M24 * b.M41,
                a.M21 * b.M12 + a.M22 * b.M22 + a.M23 * b.M32 + a.M24 * b.M42,
                a.M21 * b.M13 + a.M22 * b.M23 + a.M23 * b.M33 + a.M24 * b.M43,
                a.M21 * b.M14 + a.M22 * b.M24 + a.M23 * b.M34 + a.M24 * b.M44,

                a.M31 * b.M11 + a.M32 * b.M21 + a.M33 * b.M31 + a.M34 * b.M41,
                a.M31 * b.M12 + a.M32 * b.M22 + a.M33 * b.M32 + a.M34 * b.M42,
                a.M31 * b.M13 + a.M32 * b.M23 + a.M33 * b.M33 + a.M34 * b.M43,
                a.M31 * b.M14 + a.M32 * b.M24 + a.M33 * b.M34 + a.M34 * b.M44,

                a.M41 * b.M11 + a.M42 * b.M21 + a.M43 * b.M31 + a.M44 * b.M41,
                a.M41 * b.M12 + a.M42 * b.M22 + a.M43 * b.M32 + a.M44 * b.M42,
                a.M41 * b.M13 + a.M42 * b.M23 + a.M43 * b.M33 + a.M44 * b.M43,
                a.M41 * b.M14 + a.M42 * b.M24 + a.M43 * b.M34 + a.M44 * b.M44
                );
        }

        public static Matrix4D Multiply(Matrix4D a, double s)
        {
            return Multiply(ref a, s);
        }
        public static Matrix4D Multiply(ref Matrix4D a, double s)
        {
            return new Matrix4D(
                a.M11 * s, a.M12 * s, a.M13 * s, a.M14 * s,
                a.M21 * s, a.M22 * s, a.M23 * s, a.M24 * s,
                a.M31 * s, a.M32 * s, a.M33 * s, a.M34 * s,
                a.M41 * s, a.M42 * s, a.M43 * s, a.M44 * s
                );
        }

        public static Matrix4D CreateRotX(float angle)
        {
            double aCos = Math.Cos(Deg2Rad * angle);
            double aSin = Math.Sin(Deg2Rad * angle);
            return new Matrix4D(
                1, 0, 0, 0,
                0, aCos, aSin, 0,
                0, -aSin, aCos, 0,
                0, 0, 0, 1
            );
        }
        public static Matrix4D CreateRotY(float angle)
        {
            double aCos = Math.Cos(Deg2Rad * angle);
            double aSin = Math.Sin(Deg2Rad * angle);

            return new Matrix4D(
                aCos, 0, -aSin, 0,
                0, 1, 0, 0,
                aSin, 0, aCos, 0,
                0, 0, 0, 1
            );
        }
        public static Matrix4D CreateRotZ(float angle)
        {
            double aCos = Math.Cos(Deg2Rad * angle);
            double aSin = Math.Sin(Deg2Rad * angle);

            return new Matrix4D(
                aCos, aSin, 0, 0,
                -aSin, aCos, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1
            );
        }
    }
}
