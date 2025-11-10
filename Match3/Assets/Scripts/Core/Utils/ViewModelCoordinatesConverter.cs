// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using UnityEngine;

namespace Core.Utils
{
    public static class ViewModelCoordinatesConverter
    {
        private static Vector2 _viewOffset;
        private static Vector2 _gridViewSpriteSize;

        public static void UpdateOffsetAndSize(Vector2 viewOffset, Vector2 gridViewSpriteSize)
        {
            _viewOffset = viewOffset;
            _gridViewSpriteSize = gridViewSpriteSize;
        }

        public static Vector2 GetViewCoordinates(Vector2Int modelCoordinates)
        {
            return GetViewCoordinates(modelCoordinates.x, modelCoordinates.y);
        }

        public static Vector2 GetViewCoordinates(int x, int y)
        {
            return new Vector2(y + _viewOffset.x, -x + _viewOffset.y);
        }

        public static Vector2Int GetModelCoordinates(Vector2 viewCoordinates)
        {
            return GetModelCoordinates(viewCoordinates.x, viewCoordinates.y);
        }

        public static Vector2Int GetModelCoordinates(float x, float y)
        {
            return new Vector2Int(
                -Mathf.FloorToInt(y - _viewOffset.y + _gridViewSpriteSize.y / 2),
                Mathf.FloorToInt(x - _viewOffset.x + _gridViewSpriteSize.x / 2));
        }
    }
}