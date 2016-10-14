/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 28.07.2016 19:26:30
*/

using System;

namespace Nano3.Map
{
    public class Map2Cycle<TValue> : IMap2<TValue>
    {
        protected TValue _emptyItem;
        protected TValue[] _items;
        private int _xsize; public int XSize { get { return _xsize; } }
        private int _ysize; public int YSize { get { return _ysize; } }
        private int _size; public int Size { get { return _size; } }

        public Map2Cycle(int xsize, int ysize) : this(xsize, ysize, null) { }
        public Map2Cycle(int xsize, int ysize, TValue[] items)
        {
            if (xsize < 1) throw new ArgumentOutOfRangeException("X SIZE < 1");
            if (ysize < 1) throw new ArgumentOutOfRangeException("Y SIZE < 1");

            _xsize = xsize;
            _ysize = ysize;
            _size = _xsize * _ysize;
            _emptyItem = default(TValue);

            if (items == null || items.Length != _size){
                _items = new TValue[_size];
            }
            else { _items = items; }
        }

        public TValue this[int index]
        {
            get { return _items[MapUtils.ModM(index, _size)]; }
            set { _items[MapUtils.ModM(index, _size)] = value; }
        }
        public TValue this[int x, int y]
        {
            get
            {
                int px = MapUtils.ModM(x, _xsize);
                int py = MapUtils.ModM(y, _ysize);
                return _items[px * _ysize + py];
            }
            set
            {
                int px = MapUtils.ModM(x, _xsize);
                int py = MapUtils.ModM(y, _ysize);
                _items[px * _ysize + py] = value;
            }
        }

        public int ToIndex(int x, int y)
        {
            int px = MapUtils.ModM(x, _xsize);
            int py = MapUtils.ModM(y, _ysize);
            return px * _ysize + py;
        }

        public Vector2I ToMapPosition(int index)
        {
            int cid = MapUtils.ModM(index, _size);
            int x = cid / _ysize;
            return new Vector2I(x, cid - (x * _ysize));
        }

        public Vector2I GetCyclePosition(Vector2I position)
        {
            int px = MapUtils.ModM(position.X, _xsize);
            int py = MapUtils.ModM(position.Y, _ysize);
            return new Vector2I(px, py);
        }

        public bool IsBounded(Vector2I position)
        {
            return true;
        }

        public void Clear()
        {
            Array.Clear(_items, 0, _items.Length);
        }
    }
}
