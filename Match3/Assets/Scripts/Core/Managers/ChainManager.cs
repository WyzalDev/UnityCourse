// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;
using Core.Data;
using Core.Grid;
using Core.Utils;

namespace Core.Managers
{
    public class ChainManager : MonoBehaviour
    {
        [SerializeField] private GridManager _gridManager;
        [SerializeField] private LineRenderer _chainLineRenderer;

        private LinkedList<Element> _chainElements = new();
        private bool _isChainStarted;

        public ICollection<Element> ChainElements => _chainElements;

        private Element StartElement => _chainElements.First.Value;

        private Element CurrentElement => _chainElements.Last.Value;

        private Element PreviousElement => _chainElements.Last.Previous?.Value;

        public bool TryStartChain()
        {
            if (SelectedElement.Element.Type == ElementType.None || _isChainStarted ||
                !_gridManager.IsChainPossible(SelectedElement.Element))
                return false;

            _isChainStarted = true;
            _chainElements.AddLast(SelectedElement.Element);
            UpdateLineRenderer();
            _chainLineRenderer.enabled = true;
            return true;
        }

        public bool TryChangeElementInChain(Element element)
        {
            if (element.Type != StartElement.Type && !element.Type.IsBonus())
                return false;

            if (Mathf.Clamp(element.X, CurrentElement.X - 1, CurrentElement.X + 1) != element.X ||
                Mathf.Clamp(element.Y, CurrentElement.Y - 1, CurrentElement.Y + 1) != element.Y)
                return false;

            if (PreviousElement != null && PreviousElement.X == element.X && PreviousElement.Y == element.Y)
            {
                _chainElements.RemoveLast();
                return true;
            }

            if (_chainElements.Contains(element))
                return false;

            _chainElements.AddLast(element);
            return true;
        }

        public void UpdateLineRenderer()
        {
            _chainLineRenderer.positionCount = _chainElements.Count;

            var i = 0;

            foreach (var chainElement in _chainElements)
            {
                _chainLineRenderer.SetPosition(i,
                    ViewModelCoordinatesConverter.GetViewCoordinates(chainElement.X, chainElement.Y));

                i++;
            }
        }

        public void Restore()
        {
            _isChainStarted = false;
            _chainElements.Clear();
        }

        public bool EndChain()
        {
            _chainLineRenderer.enabled = false;
            return _chainElements.Count >= 3;
        }
    }
}