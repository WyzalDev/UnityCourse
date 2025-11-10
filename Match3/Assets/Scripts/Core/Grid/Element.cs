// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;
using Core.Data;

namespace Core.Grid
{
    public class Element
    {
        private ElementsGrid _gridModel;

        public ElementType Type { get; private set; }

        public int X { get; }

        public int Y { get; }

        public Element(ElementsGrid gridModel, ElementType type, int x, int y)
        {
            _gridModel = gridModel;
            Type = type;
            X = x;
            Y = y;
        }

        public void SetElementType(ElementType elementType)
        {
            Type = elementType;
        }

        public bool CanFall()
        {
            if (Type is ElementType.None or ElementType.Ice or ElementType.BrokenIce)
                return false;

            if (X + 1 >= _gridModel.Height)
                return false;

            if (_gridModel.Grid[X + 1][Y].Type is ElementType.None)
                return true;

            if (Y + 1 < _gridModel.Width && _gridModel.Grid[X + 1][Y + 1].Type is ElementType.None)
                return true;

            return Y - 1 >= 0 && _gridModel.Grid[X + 1][Y - 1].Type is ElementType.None;
        }

        public Vector2Int NextFallingPoint()
        {
            if (X + 1 >= _gridModel.Height)
                return new Vector2Int(-1, -1);

            if (_gridModel.Grid[X + 1][Y].Type is ElementType.None)
                return new Vector2Int(X + 1, Y);

            if (Y + 1 < _gridModel.Width && _gridModel.Grid[X + 1][Y + 1].Type is ElementType.None)
                return new Vector2Int(X + 1, Y + 1);

            if (Y - 1 >= 0 && _gridModel.Grid[X + 1][Y - 1].Type is ElementType.None)
                return new Vector2Int(X + 1, Y - 1);

            return new Vector2Int(-1, -1);
        }

        public List<Element> GetNeighbourObstacles()
        {
            var neighbours = new List<Element>();

            if (Mathf.Clamp(X - 1, 0, _gridModel.Height - 1) == X - 1)
            {
                if (_gridModel.Grid[X - 1][Y].Type.IsObstacle())
                    neighbours.Add(_gridModel.Grid[X - 1][Y]);
            }

            if (Mathf.Clamp(X + 1, 0, _gridModel.Height - 1) == X + 1)
            {
                if (_gridModel.Grid[X + 1][Y].Type.IsObstacle())
                    neighbours.Add(_gridModel.Grid[X + 1][Y]);
            }

            if (Mathf.Clamp(Y - 1, 0, _gridModel.Width - 1) == Y - 1)
            {
                if (_gridModel.Grid[X][Y - 1].Type.IsObstacle())
                    neighbours.Add(_gridModel.Grid[X][Y - 1]);
            }

            if (Mathf.Clamp(Y + 1, 0, _gridModel.Width - 1) == Y + 1)
            {
                if (_gridModel.Grid[X][Y + 1].Type.IsObstacle())
                    neighbours.Add(_gridModel.Grid[X][Y + 1]);
            }

            return neighbours;
        }
    }
}