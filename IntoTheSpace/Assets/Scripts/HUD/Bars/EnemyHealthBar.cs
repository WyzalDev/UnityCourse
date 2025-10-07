// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using Core;

namespace HUD.Bars
{
    public class EnemyHealthBar : HealthBar
    {
        protected void Start()
        {
            if (!TryGetComponent<IDamageable>(out var damageable))
                return;

            _damageable = damageable;
            _damageable.OnDamageTaken += ChangeValue;

            _slider.maxValue = _damageable.MaxHealth;
            _slider.value = _damageable.MaxHealth;
            _image.color = _gradient.Evaluate(_slider.normalizedValue);
        }

        private void OnDestroy()
        {
            if (_damageable != null)
                _damageable.OnDamageTaken -= ChangeValue;
        }
    }
}