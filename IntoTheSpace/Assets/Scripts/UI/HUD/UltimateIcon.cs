// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using UnityEngine;
using UnityEngine.UI;
using Core;

namespace UI.HUD
{
    public class UltimateIcon : MonoBehaviour, IRestorable
    {
        [SerializeField] private Image _image;
        [SerializeField] Color _inactiveColor;

        private Ultimate _ultimate;

        private void Start()
        {
            _image.color = _inactiveColor;
        }

        public void SetNewUltimate(Ultimate ultimate)
        {
            if (_ultimate != null)
            {
                _ultimate.OnUltimateUsed -= OnUltimateUsed;
                _ultimate.OnUltimateReady -= OnUltimateReady;
            }

            if (ultimate == null)
                return;

            _ultimate = ultimate;
            _ultimate.OnUltimateUsed += OnUltimateUsed;
            _ultimate.OnUltimateReady += OnUltimateReady;
        }

        private void OnUltimateUsed(float damage)
        {
            SetImageColor(_inactiveColor);
        }

        private void OnUltimateReady()
        {
            SetImageColor(Color.white);
        }

        private void SetImageColor(Color color)
        {
            _image.color = color;
        }

        private void OnDestroy()
        {
            if (_ultimate == null)
                return;

            _ultimate.OnUltimateUsed -= OnUltimateUsed;
            _ultimate.OnUltimateReady -= OnUltimateReady;
        }

        public void Restore()
        {
            SetImageColor(_inactiveColor);
        }
    }
}