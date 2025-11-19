// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System;
using UnityEngine;

namespace Core.Data
{
    [Serializable]
    public class FieldConfig
    {
        [SerializeField] private int _height;
        [SerializeField] private int _width;
        [SerializeField] private FieldRow[] _field;

        public FieldConfig(int height, int width, ElementType[][] field)
        {
            _height = height;
            _width = width;

            _field = new FieldRow[height];
            for (var i = 0; i < height; i++)
                _field[i] = new FieldRow(field[i]);
        }

        public FieldConfig()
        {
        }

        public int Height => _height;

        public int Width => _width;

        public FieldRow[] Field => _field;

        [Serializable]
        public class FieldRow
        {
            [SerializeField] private ElementType[] _fieldPart;

            public ElementType[] FieldPart => _fieldPart;

            public FieldRow(ElementType[] fieldPart)
            {
                _fieldPart = fieldPart;
            }
        }
    }
}