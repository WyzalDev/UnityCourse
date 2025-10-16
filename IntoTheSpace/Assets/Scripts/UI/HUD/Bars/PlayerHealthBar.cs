// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using DG.Tweening;
using Core;
using Core.Managers;

namespace UI.HUD.Bars
{
    public class PlayerHealthBar : HealthBar
    {
        private Tween _animationTween;
        
        public void SetNewPlayer(IDamageable damageable)
        {
            if (_damageable != null)
                _damageable.OnDamageTaken -= ChangeValue;

            if (damageable is not Ship playerShip)
                return;

            _damageable = damageable;
            _damageable.OnDamageTaken += ChangeValue;
            _image.color = _gradient.Evaluate(_slider.normalizedValue);
            playerShip.OnShipInitialized += OnShipInitialized;
        }

        protected override void ChangeValue()
        {
            base.ChangeValue();

            VignetteManager.StartTakeDamageEffect();
            _animationTween = transform.DOShakePosition(1f, 30f, 5);
        }

        private void OnShipInitialized()
        {
            _slider.maxValue = _damageable.MaxHealth;
            _slider.value = _damageable.CurrentHealth;

            if (_damageable is Ship playerShip)
                playerShip.OnShipInitialized -= OnShipInitialized;
        }

        private void OnDestroy()
        {
            if (_damageable != null)
                _damageable.OnDamageTaken -= ChangeValue;
        }
    }
}