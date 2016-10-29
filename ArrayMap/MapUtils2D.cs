/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 26.09.2016 20:43:38
*/

using System;
using System.Collections.Generic;

namespace Nano3.Map
{
    public static partial class MapUtils
    {
        public static TMap GetMirror<TMap, TValue>(TMap map, bool mirX, bool mirY)
            where TMap : IMap2<TValue>, ITypeCreator<TMap, Vector2I>
        {
            if (!mirX && !mirY) { return map; }
            if (map == null) { return map; }

            TMap result = map.Create(new Vector2I(map.XSize, map.YSize));
            for (int x = 0; x < result.XSize; x++) {
                int dx = mirX ? result.XSize - 1 - x : x;
                for (int y = 0; y < result.YSize; y++) {
                    int dy = mirY ? result.YSize - 1 - y : y;
                    result[dx, dy] = map[x, y];
                }
            }
            return result;
        }

        public static TMap Resize<TMap, TValue> (TMap map, Vector2I newSize)
            where TMap : IMap2<TValue>, ITypeCreator<TMap, Vector2I>
        {
            if (map == null) { return map; }
            Vector2I mapSize = new Vector2I(map.XSize, map.YSize);
            if (mapSize == newSize) { return map; }

            TMap result = map.Create(newSize);
            int maxX = Math.Min(result.XSize, map.XSize);
            int maxY = Math.Min(result.YSize, map.YSize);

            for (int x = 0; x < maxX; x++) {
                for (int y = 0; y < maxY; y++) {
                    result[x, y] = map[x, y];
                }
            }

            return result;
        }

        public static TMap Scale<TMap, TValue>(TMap map, Vector2D scale)
            where TMap : IMap2<TValue>, ITypeCreator<TMap, Vector2I>
        {
            if (map == null) { return map; }
            if (scale.X == 1 && scale.Y == 1) { return map; }

            Vector2I newsize = new Vector2I(MathEx.FloorI(map.XSize * scale.X), MathEx.FloorI(map.YSize * scale.Y));
            TMap result = map.Create(newsize);
            for (int x = 0; x < newsize.X; x++) {
                int ox = scale.X < 1 ? MathEx.RoundI(x / scale.X) : MathEx.FloorI(x / scale.X);
                if (x == newsize.X - 1 || ox >= map.XSize) { ox = map.XSize - 1; }

                for (int y = 0; y < newsize.Y; y++) {
                    int oy = scale.Y < 1 ? MathEx.RoundI(y / scale.Y) : MathEx.FloorI(y / scale.Y);
                    if (y == newsize.Y - 1 || oy >= map.YSize) { oy = map.YSize - 1; }

                    result[x, y] = map[ox, oy];
                }
            }
            return result;
        }

        public static TMap ResizeWithKeepCenter<TMap, TValue>(TMap map, Vector2I newSize)
            where TMap : IMap2<TValue>, ITypeCreator<TMap, Vector2I>
        {
            if (map == null) { return map; }
            if (newSize == new Vector2I(map.XSize, map.YSize)) { return map; }

            TMap result = map.Create(newSize);
            int minX = Math.Min(result.XSize, map.XSize);
            int minY = Math.Min(result.YSize, map.YSize);

            int startX = result.XSize > map.XSize ? (result.XSize - map.XSize) / 2 : 0;
            int startY = result.YSize > map.YSize ? (result.YSize - map.YSize) / 2 : 0;

            int offsetX = map.XSize > result.XSize ? (map.XSize - result.XSize) / 2 : 0;
            int offsetZ = map.YSize > result.YSize ? (map.YSize - result.YSize) / 2 : 0;

            for (int x = 0; x < minX; x++) {
                for (int y = 0; y < minY; y++) {
                    result[x + startX, y + startY] = map[x + offsetX, y + offsetZ];
                }
            }
            return result;
        }

        public static TMap CropEmpty2D<TMap, TValue>(TMap map, TValue emptyValue)
            where TMap : IMap2<TValue>, ITypeCreator<TMap, Vector2I>
            where TValue : IEquatable<TValue>
        {
            if (map == null) { return map; }
            Vector2I voxelMapSize = new Vector2I(map.XSize, map.YSize);
            Vector2I start = new Vector2I(map.XSize, map.YSize);
            Vector2I end = new Vector2I(0, 0);

            for (int x = 0; x < map.XSize; x++) {
                for (int y = 0; y < map.YSize; y++) {
                    if (map[x, y].Equals(emptyValue)) { continue; }
                    if (start.X > x) { start.X = x; }
                    if (start.Y > y) { start.Y = y; }
                    if (end.X < x) { end.X = x; }
                    if (end.Y < y) { end.Y = y; }
                }
            }
            end += new Vector2I(1, 1);
            Vector2I newSize = end - start;
            if (newSize == voxelMapSize) { return map; }

            TMap result = map.Create(newSize);
            for (int x = start.X, dx = 0; x <= end.X; x++, dx++) {
                for (int y = start.Y, dy = 0; y <= end.Y; y++, dy++) {
                    result[dx, dy] = map[x, y];
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMap"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="map">Original map</param>
        /// <param name="rotateDirection"> -1 or +1</param>
        /// <returns></returns>
        public static TMap Rotate90<TMap, TValue>(TMap map, int rotateDirection)
            where TMap : IMap2<TValue>, ITypeCreator<TMap, Vector2I>
        {
            if (map == null) { return map; }
            if (Math.Abs(rotateDirection) != 1) { return map; }

            TMap result = map.Create(new Vector2I(map.YSize, map.XSize));

            for (int mx = 0; mx < result.XSize; mx++) {
                int dy = rotateDirection == -1 ? mx : map.XSize - 1 - mx;
                for (int my = 0; my < result.YSize; my++) {
                    int dx = rotateDirection == -1 ? map.YSize - 1 - my : my;
                    result[mx, my] = map[dx, dy];
                }
            }
            return result;
        }

        public static TMap Move<TMap, TValue>(TMap map, Vector2I moveOffset)
            where TMap : IMap2<TValue>, ITypeCreator<TMap, Vector2I>
        {
            if (map == null) { return map; }
            if (moveOffset.X == 0 && moveOffset.Y == 0) { return map; }

            TMap result = map.Create(new Vector2I(map.XSize, map.YSize));
            for (int mx = 0; mx < result.XSize; mx++) {
                int ox = MathEx.ModM(mx - moveOffset.X, result.XSize);
                for (int my = 0; my < result.YSize; my++) {
                    int oy = MathEx.ModM(my - moveOffset.Y, result.YSize);

                    result[mx, my] = map[ox, oy];
                }
            }
            return result;
        }

        public static void FloodFill4<TMap, TValue>(TMap map, TValue from, TValue to, Vector2I startPoint)
            where TMap : IMap2<TValue>
            where TValue : struct, IEquatable<TValue>
        {
            if (from.Equals(to)) { return; }
            List<Vector2I> _openList = new List<Vector2I>();
            Vector2I startPos = startPoint;

            if (startPos.Y < 0) { startPos.Y = 0; }
            else if (startPos.Y >= map.YSize) { startPos.Y = map.YSize - 1; }
            if (startPoint.X < 0) { startPos.X = 0; }
            else if (startPos.Y >= map.XSize) { startPos.X = map.XSize - 1; }

            Vector2I[] offsets = new Vector2I[4]{
                new Vector2I(-1, 0), new Vector2I(1, 0), new Vector2I(0, -1), new Vector2I(0, 1)
            };
            map[startPos.X, startPos.Y] = to;

        NEXT:
            for (int i = 0; i < offsets.Length; i++) {
                Vector2I off = offsets[i];
                Vector2I nPos = new Vector2I(startPos.X + off.X, startPos.Y + off.Y);
                if (!map.IsBounded(nPos)) { continue; }
                if (map[nPos.X, nPos.Y].Equals(from)) {
                    _openList.Add(nPos);
                    map[nPos.X, nPos.Y] = to;
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
