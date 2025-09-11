// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;

namespace Utility
{
    public class CircularList<T>
    {
        public T Current => _list[_current];

        private List<T> _list;
        private int _current;

        public T Next()
        {
            Increment();

            return _list[_current];
        }

        public T Previous()
        {
            Decrement();

            return _list[_current];
        }

        public void Initialize(List<T> items)
        {
            _list = items;
            _current = 0;
        }

        public void Add(T item)
        {
            _list.Insert(_current, item);
        }

        public List<T> RemoveAll()
        {
            var result = _list;
            _list = new List<T>();
            return result;
        }

        private void Increment()
        {
            if (_current == _list.Count - 1)
                _current = 0;
            else
                _current++;
        }

        private void Decrement()
        {
            if (_current == 0)
                _current = _list.Count - 1;
            else
                _current--;
        }
    }
}