/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 26.09.2016 21:20:25
*/

using System;
using System.Collections.Generic;

namespace Nano3.Map
{
    public static partial class MapUtils
    {
        public static TMap GetMirror<TMap, TValue>(TMap map, bool mirX, bool mirY, bool mirZ)
           where TMap : IMap3<TValue>, ITypeCreator<TMap, Vector3I>
        {
            if (!mirX && !mirY && !mirZ) { return map; }
            if (map == null) { return map; }

            TMap result = map;// map.Create(new Vector3I(map.XSize, map.YSize, map.ZSize));
            for (int x = 0; x < result.XSize; x++) {
                int dx = mirX ? map.XSize - 1 - x : x;
                for (int y = 0; y < result.YSize; y++) {
                    int dy = mirY ? map.YSize - 1 - x : x;
                    for (int z = 0; z < result.ZSize; z++) {
                        int dz = mirZ ? map.ZSize - 1 - z : z;
                        result[x, y, z] = map[dx, dy, dz];
                    }
                }
            }
            return result;
        }

        public static TMap Resize<TMap, TValue>(TMap map, Vector3I newSize)
            where TMap : IMap3<TValue>, ITypeCreator<TMap, Vector3I>
        {
            if (map == null) { return map; }
            Vector3I mapSize = new Vector3I(map.XSize, map.YSize, map.ZSize);
            if (mapSize == newSize) { return map; }

            TMap result = map.Create(newSize);
            int maxX = Math.Min(result.XSize, map.XSize);
            int maxY = Math.Min(result.YSize, map.YSize);
            int maxZ = Math.Min(result.ZSize, map.ZSize);
            for (int x = 0; x < maxX; x++) {
                for (int y = 0; y < maxY; y++) {
                    for (int z = 0; z < maxZ; z++) {
                        result[x, y, z] = map[x, y, z];
                    }
                }
            }

            return result;
        }

        public static TMap ResizeWithKeepCenterXZ<TMap, TValue>(TMap map, Vector3I newSize)
            where TMap : IMap3<TValue>, ITypeCreator<TMap, Vector3I>
        {
            if (map == null) { return map; }
            if (newSize == new Vector3I(map.XSize, map.YSize, map.ZSize)) { return map; }

            TMap result = map.Create(newSize);
            int minX = Math.Min(result.XSize, map.XSize);
            int minY = Math.Min(result.YSize, map.YSize);
            int minZ = Math.Min(result.ZSize, map.ZSize);

            int startX = result.XSize > map.XSize ? (result.XSize - map.XSize) / 2 : 0;
            int startZ = result.ZSize > map.ZSize ? (result.ZSize - map.ZSize) / 2 : 0;

            int offsetX = map.XSize > result.XSize ? (map.XSize - result.XSize) / 2 : 0;
            int offsetZ = map.ZSize > result.ZSize ? (map.ZSize - result.ZSize) / 2 : 0;

            for (int x = 0; x < minX; x++) {
                for (int y = 0; y < minY; y++) {
                    for (int z = 0; z < minZ; z++) {
                        result[x + startX, y, z + startZ] = map[x + offsetX, y, z + offsetZ];
                    }
                }
            }
            return result;
        }

        public static TMap Scale<TMap, TValue>(TMap map, float scaleX, float scaleY, float scaleZ)
            where TMap : IMap3<TValue>, ITypeCreator<TMap, Vector3I>
        {
            if (map == null) { return map; }
            if (scaleX == 1 && scaleY == 1 && scaleZ == 1) { return map; }

            Vector3I newSize = new Vector3I(Math.Floor(map.XSize * scaleX), Math.Floor(map.YSize * scaleY), Math.Floor(map.ZSize * scaleZ));
            TMap result = map.Create(newSize);
            for (int x = 0; x < newSize.X; x++) {
                int ox = (int)(scaleX < 1 ? Math.Round(x / scaleX) : Math.Floor(x / scaleX));
                if (x == newSize.X - 1 || ox >= map.XSize) { ox = map.XSize - 1; }

                for (int y = 0; y < newSize.Y; y++) {
                    int oy = (int)(scaleY < 1 ? Math.Round(y / scaleY) : Math.Floor(y / scaleY));
                    if (y == newSize.Y - 1 || oy >= map.YSize) { oy = map.YSize - 1; }

                    for (int z = 0; z < newSize.Z; z++) {
                        int oz = (int)(scaleZ < 1 ? Math.Round(z / scaleZ) : Math.Floor(z / scaleZ));
                        if (z == newSize.Z - 1 || oz >= map.ZSize) { oz = map.ZSize - 1; }
                        result[x, y, z] = map[ox, oy, oz];
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMap"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="map">Original Map </param>
        /// <param name="rotateDirection"> if need to rotate axis must be -1 or 1</param>
        /// <returns></returns>
        public static TMap Rotate90<TMap, TValue>(TMap map, Vector3I rotateDirection)
            where TMap : IMap3<TValue>, ITypeCreator<TMap, Vector3I>
        {
            if (map == null) { return map; }
            if (rotateDirection == new Vector3I(0, 0, 0)) { return map; }

            Matrix4D m = new Matrix4D(
           1, 0, 0, 0,
           0, 1, 0, 0,
           0, 0, 1, 0,
           1, 2, 3, 1);

            if (rotateDirection.Z == 1) { m = Matrix4D.Multiply(m, Matrix4D.CreateRotZ(90)); }
            else if (rotateDirection.Z == -1) { m = Matrix4D.Multiply(m, Matrix4D.CreateRotZ(-90)); }

            if (rotateDirection.X == 1) { m = Matrix4D.Multiply(m, Matrix4D.CreateRotX(90)); }
            else if (rotateDirection.X == -1) { m = Matrix4D.Multiply(m, Matrix4D.CreateRotX(-90)); }

            if (rotateDirection.Y == 1) { m = Matrix4D.Multiply(m, Matrix4D.CreateRotY(90)); }
            else if (rotateDirection.Y == -1) { m = Matrix4D.Multiply(m, Matrix4D.CreateRotY(-90)); }

            Vector3I v = new Vector3I(Math.Round(m.M41 + 0.001), Math.Round(m.M42 + 0.001), Math.Round(m.M43 + 0.001));
            Vector3I mSize = new Vector3I(map.XSize, map.YSize, map.ZSize);

            int xAxis = v.X;
            int yAxis = v.Y;
            int zAxis = v.Z;

            int xSize = Math.Abs(xAxis) == 1 ? mSize.X : Math.Abs(xAxis) == 2 ? mSize.Y : mSize.Z;
            int ySize = Math.Abs(yAxis) == 1 ? mSize.X : Math.Abs(yAxis) == 2 ? mSize.Y : mSize.Z;
            int zSize = Math.Abs(zAxis) == 1 ? mSize.X : Math.Abs(zAxis) == 2 ? mSize.Y : mSize.Z;

            TMap result = map.Create(new Vector3I(xSize, ySize, zSize));

            for (int mx = 0; mx < result.XSize; mx++) {
                for (int my = 0; my < result.YSize; my++) {
                    for (int mz = 0; mz < result.ZSize; mz++) {
                        int dx = xAxis == 1 ? mx : xAxis == -1 ? Math.Abs(-result.XSize + 1 + mx) : xAxis == 2 ? my : xAxis == -2 ? Math.Abs(-result.YSize + 1 + my) : xAxis == 3 ? mz : Math.Abs(-result.ZSize + 1 + mz);
                        int dy = yAxis == 1 ? mx : yAxis == -1 ? Math.Abs(-result.XSize + 1 + mx) : yAxis == 2 ? my : yAxis == -2 ? Math.Abs(-result.YSize + 1 + my) : yAxis == 3 ? mz : Math.Abs(-result.ZSize + 1 + mz);
                        int dz = zAxis == 1 ? mx : zAxis == -1 ? Math.Abs(-result.XSize + 1 + mx) : zAxis == 2 ? my : zAxis == -2 ? Math.Abs(-result.YSize + 1 + my) : zAxis == 3 ? mz : Math.Abs(-result.ZSize + 1 + mz);
                        result[mx, my, mz] = map[dx, dy, dz];
                    }
                }
            }
            return result;
        }

        public static TMap Move<TMap, TValue>(TMap map, Vector3I moveOffset)
            where TMap : IMap3<TValue>, ITypeCreator<TMap, Vector3I>
        {
            if (map == null) { return map; }
            if (moveOffset.X == 0 && moveOffset.Y == 0 && moveOffset.Z == 0) { return map; }

            TMap result = map.Create(new Vector3I(map.XSize, map.YSize, map.ZSize));
            for (int mx = 0; mx < result.XSize; mx++) {
                int ox = ModM(mx - moveOffset.X, result.XSize);
                for (int my = 0; my < result.YSize; my++) {
                    int oy = ModM(my - moveOffset.Y, result.YSize);
                    for (int mz = 0; mz < result.ZSize; mz++) {
                        int oz = ModM(mz - moveOffset.Z, result.ZSize);
                        result[mx, my, mz] = map[ox, oy, oz];
                    }
                }
            }
            return result;
        }

        public static int ModM(int value, int mod)
        {
            int dv = value / mod;
            int val = value - dv * mod;
            if (val < 0) { val += mod; }
            return val;
        }

        public static long ModM(long value, long mod)
        {
            long dv = value / mod;
            long val = value - dv * mod;
            if (val < 0) { val += mod; }
            return val;
        }
    }
}
