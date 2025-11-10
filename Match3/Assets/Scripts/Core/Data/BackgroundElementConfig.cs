// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Data
{
    [Serializable]
    public class BackgroundElementConfig
    {
        [SerializeField] private BackgroundElementType _backgroundElementType;
        [SerializeField] private List<Sprite> _possibleSprites;

        public BackgroundElementType BackgroundElementType => _backgroundElementType;

        public List<Sprite> PossibleSprites => _possibleSprites;
    }
}