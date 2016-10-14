/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 28.07.2016 18:33:00
*/

using System;

namespace Nano3.Map
{
    public class Map2<TValue> : IMap2<TValue>
    {
        protected TValue _emptyItem;
        protected TValue[] _items;
        private int _xsize; public int XSize { get { return _xsize; } }
        private int _ysize; public int YSize { get { return _ysize; } }
        private int _size; public int Size { get { return _size; } }

        public Map2(int xsize, int ysize) : this(xsize, ysize, null) { }
        public Map2(int xsize, int ysize, TValue[] items)
        {
            if (xsize < 1) throw new ArgumentOutOfRangeException("X SIZE < 1");
            if (ysize < 1) throw new ArgumentOutOfRangeException("Y SIZE < 1");

            _xsize = xsize;
            _ysize = ysize;

            _size = _xsize * _ysize;

            if (items == null || items.Length != _size) {
                _items = new TValue[_size];
            }
            else { _items = items; }
            _emptyItem = default(TValue);
        }

        public TValue this[int x, int y]
        {
            get
            {
                int index = x * _ysize + y;
                return index < 0 || index >= _items.Length 
                    ? _emptyItem : _items[index];
            }
            set
            {
                int index = x * _ysize + y;
                if (index < 0 || index >= _items.Length) return;
                _items[index] = value;
            }
        }
        public TValue this[int index]
        {
            get
            {
                return index < 0 || index >= _items.Length 
                    ? _emptyItem : _items[index];
            }
            set
            {
                if (index < 0 || index >= _items.Length) return;
                _items[index] = value;
            }
        }

        public int ToIndex(int x, int y)
        {
            if (x < 0 || x >= _xsize || y < 0 || y >= _ysize) return -1;
            return x * _ysize + y;
        }
        public Vector2I ToMapPosition(int index)
        {
            if (index < 0 || index >= _size) { return new Vector2I(-1, -1); }
            int x = index / _ysize;
            return new Vector2I(x, index - (x * _ysize));
        }

        public bool IsBounded(Vector2I position)
        {
            if (position.X < 0 || position.X >= _xsize || position.Y < 0 || position.Y >= _ysize) return false;
            return true;
        }

        public void Clear()
        {
            Array.Clear(_items, 0, _items.Length);
        }
    }
}
