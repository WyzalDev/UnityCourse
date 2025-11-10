// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Data
{
    [Serializable]
    public class ElementFallData
    {
        public Dictionary<int, List<(Vector2Int, Vector2Int, ElementType)>> FallingPath { get; private set; } = new();

        public void AddFallingPoint(Vector2Int fromPoint, Vector2Int toPoint, ElementType elementType, int tick)
        {
            if (!FallingPath.ContainsKey(tick))
                FallingPath.Add(tick, new List<(Vector2Int, Vector2Int, ElementType)>());

            var currentTickList = FallingPath[tick];
            currentTickList.Add((fromPoint, toPoint, elementType));
        }
    }
}