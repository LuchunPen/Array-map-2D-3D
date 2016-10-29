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

            TMap result = map.Create(new Vector3I(map.XSize, map.YSize, map.ZSize));
            for (int x = 0; x < result.XSize; x++) {
                int dx = mirX ? result.XSize - 1 - x : x;
                for (int y = 0; y < result.YSize; y++) {
                    int dy = mirY ? result.YSize - 1 - y : y;
                    for (int z = 0; z < result.ZSize; z++) {
                        int dz = mirZ ? result.ZSize - 1 - z : z;
                        result[dx, dy, dz] = map[x, y, z];
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

        public static TMap Scale<TMap, TValue>(TMap map, Vector3D scale)
            where TMap : IMap3<TValue>, ITypeCreator<TMap, Vector3I>
        {
            if (map == null) { return map; }
            if (scale.X == 1 && scale.Y == 1) { return map; }

            Vector3I newSize = new Vector3I(MathEx.FloorI(map.XSize * scale.X), MathEx.FloorI(map.YSize * scale.Y), MathEx.FloorI(map.ZSize * scale.Z));
            TMap result = map.Create(newSize);
            for (int x = 0; x < newSize.X; x++) {
                int ox = scale.X < 1 ? MathEx.RoundI(x / scale.X) : MathEx.FloorI(x / scale.X);
                if (x == newSize.X - 1 || ox >= map.XSize) { ox = map.XSize - 1; }

                for (int y = 0; y < newSize.Y; y++) {
                    int oy = scale.Y < 1 ? MathEx.RoundI(y / scale.Y) : MathEx.FloorI(y / scale.Y);
                    if (y == newSize.Y - 1 || oy >= map.YSize) { oy = map.YSize - 1; }

                    for (int z = 0; z < newSize.Z; z++) {
                        int oz = scale.Z < 1 ? MathEx.RoundI(z / scale.Z) : MathEx.FloorI(z / scale.Z);
                        if (z == newSize.Z - 1 || oz >= map.ZSize) { oz = map.ZSize - 1; }
                        result[x, y, z] = map[ox, oy, oz];
                    }
                }
            }
            return result;
        }

        public static TMap CropEmpty3D<TMap, TValue>(TMap map, TValue emptyValue)
            where TMap : IMap3<TValue>, ITypeCreator<TMap, Vector3I>
            where TValue : IEquatable<TValue>
        {
            if (map == null) { return map; }
            Vector3I voxelMapSize = new Vector3I(map.XSize, map.YSize, map.ZSize);
            Vector3I start = new Vector3I(map.XSize, map.YSize, map.ZSize);
            Vector3I end = new Vector3I(0, 0, 0);

            for (int x = 0; x < map.XSize; x++) {
                for (int y = 0; y < map.YSize; y++) {
                    for (int z = 0; z < map.ZSize; z++) {
                        if (map[x, y, z].Equals(emptyValue)) { continue; }
                        if (start.X > x) { start.X = x; }
                        if (start.Y > y) { start.Y = y; }
                        if (start.Z > z) { start.Z = z; }
                        if (end.X < x) { end.X = x; }
                        if (end.Y < y) { end.Y = y; }
                        if (end.Z < z) { end.Z = z; }
                    }
                }
            }
            end += new Vector3I(1, 1, 1);
            Vector3I newSize = end - start;
            if (newSize == voxelMapSize) { return map; }

            TMap result = map.Create(newSize);
            for (int x = start.X, dx = 0; x <= end.X; x++, dx++) {
                for (int y = start.Y, dy = 0; y <= end.Y; y++, dy++) {
                    for (int z = start.Z, dz = 0; z <= end.Z; z++, dz++) {
                        result[dx, dy, dz] = map[x, y, z];
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
                int ox = MathEx.ModM(mx - moveOffset.X, result.XSize);
                for (int my = 0; my < result.YSize; my++) {
                    int oy = MathEx.ModM(my - moveOffset.Y, result.YSize);
                    for (int mz = 0; mz < result.ZSize; mz++) {
                        int oz = MathEx.ModM(mz - moveOffset.Z, result.ZSize);
                        result[mx, my, mz] = map[ox, oy, oz];
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fillAxis">Floodfill axis set [1] or [0] </param>
        public static void FloodFill6<TMap, TValue>(TMap map, TValue from, TValue to, Vector3I startPoint, Vector3I fillAxis)
            where TMap : IMap3<TValue>
            where TValue : struct, IEquatable<TValue>
        {
            if (from.Equals(to)) { return; }

            List<Vector3I>_openList = new List<Vector3I>();
            Vector3I startPos = startPoint;
            if (startPos.Y < 0) { startPos.Y = 0; }
            else if (startPos.Y >= map.YSize) { startPos.Y = map.YSize - 1; }
            if (startPoint.X < 0) { startPos.X = 0; }
            else if (startPos.Y >= map.XSize) { startPos.X = map.XSize - 1; }
            if (startPos.Z < 0) { startPos.Z = 0; }
            else if (startPos.Z >= map.ZSize) { startPos.Z = map.ZSize - 1; }

            List<Vector3I> o = new List<Vector3I>();
            if (fillAxis.X != 0) {
                o.Add(new Vector3I(-1, 0, 0));
                o.Add(new Vector3I(1, 0, 0));
            }
            if (fillAxis.Y != 0) {
                o.Add(new Vector3I(0, -1, 0));
                o.Add(new Vector3I(0, 1, 0));
            }
            if (fillAxis.Z != 0) {
                o.Add(new Vector3I(0, 0, -1));
                o.Add(new Vector3I(0, 0, 1));
            }

            Vector3I[] offsets = o.ToArray();
            map[startPos.X, startPos.Y, startPos.Z] = to;

        NEXT:
            for (int i = 0; i < offsets.Length; i++) {
                Vector3I off = offsets[i];
                Vector3I nPos = new Vector3I(startPos.X + off.X, startPos.Y + off.Y, startPos.Z + off.Z);
                if (!map.IsBounded(nPos)) { continue; }
                if (map[nPos.X, nPos.Y, nPos.Z].Equals(from)) {
                    _openList.Add(nPos);
                    map[nPos.X, nPos.Y, nPos.Z] = to;
                }
            }
            int count = _openList.Count;
            if (count > 0) {
                startPos = _openList[count - 1];
                _openList.RemoveAt(count - 1);
                goto NEXT;
            }
        }
    }
}
