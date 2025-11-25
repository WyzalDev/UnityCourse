// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;
using Audio.Managers;
using Core.Data;
using Core.Grid;

namespace Core.Managers
{
    public class ChainManager : MonoBehaviour
    {
        [SerializeField] private GridManager _gridManager;

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
            _gridManager.GetElementView(SelectedElement.Element.X, SelectedElement.Element.Y).PlaySelect();
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
                _gridManager.GetElementView(CurrentElement.X, CurrentElement.Y).PlayDeselect();
                _chainElements.RemoveLast();
                AudioManager.PlaySfxWithPitch("RemoveFromChain");
                return true;
            }

            if (_chainElements.Contains(element))
                return false;

            _chainElements.AddLast(element);
            _gridManager.GetElementView(element.X, element.Y).PlaySelect();
            PlayAddToChainSfxOnType(element.Type);
            return true;
        }

        private void PlayAddToChainSfxOnType(ElementType type)
        {
            if (type.IsGem())
                AudioManager.PlaySfxWithPitch("AddToChain");

            if (type.IsBonus())
                AudioManager.PlaySfxWithPitch("AddBonusToChain");
        }

        public void Restore()
        {
            _isChainStarted = false;
            _chainElements.Clear();
        }

        public bool EndChain()
        {
            foreach (var chainElement in _chainElements)
                _gridManager.GetElementView(chainElement.X, chainElement.Y).PlayDeselect();

            return _chainElements.Count >= 3;
        }
    }
}