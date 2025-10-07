// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using UnityEngine;
using UnityEngine.UI;
using Core;

namespace HUD
{
    public class UltimateIcon : MonoBehaviour
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
                _ultimate.OnUltimateUsed -= UltimateUsed;
                _ultimate.OnUltimateReady -= UltimateReady;
            }

            if (ultimate == null)
                return;

            _ultimate = ultimate;
            _ultimate.OnUltimateUsed += UltimateUsed;
            _ultimate.OnUltimateReady += UltimateReady;
        }

        private void UltimateUsed(float damage)
        {
            _image.color = _inactiveColor;
        }

        private void UltimateReady()
        {
            _image.color = Color.white;
        }

        private void OnDestroy()
        {
            if (_ultimate == null)
                return;

            _ultimate.OnUltimateUsed -= UltimateUsed;
            _ultimate.OnUltimateReady -= UltimateReady;
        }
    }
}