/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 28.07.2016 22:58:14
*/

using System;

namespace Nano3.Map
{
    public class Map3<TValue> : IMap3<TValue>
    {
        protected TValue _emptyItem;
        public TValue[] _items;
        private int _xsize; public int XSize { get { return _xsize; } }
        private int _ysize; public int YSize { get { return _ysize; } }
        private int _zsize; public int ZSize { get { return _zsize; } }

        public Map3(int xsize, int ysize, int zsize) : this(xsize, ysize, zsize, null) { }
        public Map3(int xsize, int ysize, int zsize, TValue[] items)
        {
            if (xsize < 1) throw new ArgumentOutOfRangeException("X SIZE < 1");
            if (ysize < 1) throw new ArgumentOutOfRangeException("Y SIZE < 1");
            if (zsize < 1) throw new ArgumentOutOfRangeException("Z SIZE < 1");

            _xsize = xsize;
            _ysize = ysize;
            _zsize = zsize;

            int size = _xsize * _ysize * _zsize;

            if (items == null || items.Length != size){
                _items = new TValue[size];
            }
            else { _items = items; }

            _emptyItem = default(TValue);
        }

        public TValue this[XYZ64 coord]
        {
            get
            {
                int px = coord.X; if (px < 0 || px >= _xsize) { return _emptyItem; }
                int py = coord.Y; if (py < 0 || py >= _ysize) { return _emptyItem; }
                int pz = coord.Z; if (pz < 0 || pz >= _zsize) { return _emptyItem; }

                return _items[_zsize * (px * _ysize + py) + pz];
            }
            set
            {
                int px = coord.X; if (px < 0 || px >= _xsize) { return; }
                int py = coord.Y; if (py < 0 || py >= _ysize) { return; }
                int pz = coord.Z; if (pz < 0 || pz >= _zsize) { return; }

                _items[_zsize * (px * _ysize + py) + pz] = value;
            }
        }

        public TValue this[int px, int py, int pz]
        {
            get
            {
                if (px < 0 || px >= _xsize || py < 0 || py >= _ysize || pz < 0 || pz >= _zsize) { return _emptyItem; }
                return _items[_zsize * (px * _ysize + py) + pz];
            }
            set
            {
                if (px < 0 || px >= _xsize || py < 0 || py >= _ysize || pz < 0 || pz >= _zsize) { return; }
                _items[_zsize * (px * _ysize + py) + pz] = value;
            }
        }

        public bool IsBounded(Vector3I position)
        {
            if (position.X < 0 || position.Y < 0 || position.X >= _xsize || position.Y >= _ysize || position.Z < 0 || position.Z >= _zsize) { return false; }
            return true;
        }

        public void Clear()
        {
            Array.Clear(_items, 0, _items.Length);
        }
    }
}
