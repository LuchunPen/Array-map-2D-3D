/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 28.07.2016 23:05:21
*/

using System;

namespace Nano3.Map
{
    public class Map3Cycle<TValue> : IMap3<TValue>
    {
        protected TValue _emptyItem;
        protected TValue[] _items;
        private int _xsize; public int XSize { get { return _xsize; } }
        private int _ysize; public int YSize { get { return _ysize; } }
        private int _zsize; public int ZSize { get { return _zsize; } }

        public Map3Cycle(int xsize, int ysize, int zsize) : this(xsize, ysize, zsize, null) { }
        public Map3Cycle(int xsize, int ysize, int zsize, TValue[] items)
        {
            if (xsize < 1) throw new ArgumentOutOfRangeException("X SIZE < 1");
            if (ysize < 1) throw new ArgumentOutOfRangeException("Y SIZE < 1");
            if (zsize < 1) throw new ArgumentOutOfRangeException("Z SIZE < 1");

            _xsize = xsize;
            _ysize = ysize;
            _zsize = zsize;

            int size = _xsize * _ysize * _zsize;

            if (items == null || items.Length != size)
            {
                _items = new TValue[size];
            }
            else { _items = items; }
            _emptyItem = default(TValue);
        }


        public TValue this[XYZ64 coord]
        {
            get
            {
                int px = MapUtils.ModM(coord.X, _xsize);
                int py = MapUtils.ModM(coord.Y, _ysize);
                int pz = MapUtils.ModM(coord.Z, _zsize);
                return _items[_zsize * (px * _ysize + py) + pz];
            }
            set
            {
                int px = MapUtils.ModM(coord.X, _xsize);
                int py = MapUtils.ModM(coord.Y, _ysize);
                int pz = MapUtils.ModM(coord.Z, _zsize);

                _items[_zsize * (px * _ysize + py) + pz] = value;
            }
        }

        public TValue this[int px, int py, int pz]
        {
            get
            {
                px = MapUtils.ModM(px, _xsize);
                py = MapUtils.ModM(py, _ysize);
                pz = MapUtils.ModM(pz, _zsize);
                return _items[_zsize * (px * _ysize + py) + pz];
            }
            set
            {
                px = MapUtils.ModM(px, _xsize);
                py = MapUtils.ModM(py, _ysize);
                pz = MapUtils.ModM(pz, _zsize);

                _items[_zsize * (px * _ysize + py) + pz] = value;
            }
        }

        public Vector3I GetCyclePosition(Vector3I position)
        {
            int px = MapUtils.ModM(position.X, _xsize);
            int py = MapUtils.ModM(position.Y, _ysize);
            int pz = MapUtils.ModM(position.Z, _ysize);
            return new Vector3I(px, py, pz);
        }

        public bool IsBounded(Vector3I position)
        {
            return true;
        }

        public void Clear()
        {
            Array.Clear(_items, 0, _items.Length);
        }
    }
}
