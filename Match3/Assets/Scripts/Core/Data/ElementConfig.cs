// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System;
using UnityEngine;

namespace Core.Data
{
    [Serializable]
    public class ElementConfig
    {
        [SerializeField] private ElementType _elementType;
        [SerializeField] private Sprite _sprite;

        public ElementType ElementType => _elementType;

        public Sprite Sprite => _sprite;
    }
}