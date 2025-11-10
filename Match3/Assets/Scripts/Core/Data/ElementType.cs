// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System;
using UnityEngine;

namespace Core.Data
{
    [Serializable]
    public enum ElementType
    {
        [InspectorName(null)]
        None = 0,
        RedGem = 1,
        GreenGem = 2,
        BlueGem = 3,
        YellowGem = 4,
        PurpleGem = 5,
        Rocket = 6,
        Bomb = 7,
        Rock = 8,
        BrokenRock = 9,
        Ice = 10,
        BrokenIce = 11,
    }

    public static class ElementTypeExtensions
    {
        public static bool IsGem(this ElementType elementType)
        {
            return elementType is ElementType.RedGem or ElementType.GreenGem or ElementType.BlueGem
                or ElementType.YellowGem or ElementType.PurpleGem;
        }

        public static bool IsBonus(this ElementType elementType)
        {
            return elementType is ElementType.Rocket or ElementType.Bomb;
        }

        public static bool IsObstacle(this ElementType elementType)
        {
            return elementType is ElementType.Rock or ElementType.BrokenRock or ElementType.Ice
                or ElementType.BrokenIce;
        }
    }
}