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
        [SerializeField] private FieldString[] _field;

        public int Height => _height;

        public int Width => _width;

        public FieldString[] Field => _field;

        [Serializable]
        public class FieldString
        {
            [SerializeField] private ElementType[] _fieldPart;

            public ElementType[] FieldPart => _fieldPart;

            public FieldString(ElementType[] fieldPart)
            {
                _fieldPart = fieldPart;
            }
        }
    }
}