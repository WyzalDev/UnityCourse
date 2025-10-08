// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using Core;

namespace UI.HUD.Bars
{
    public abstract class HealthBar : Bar
    {
        protected IDamageable _damageable;

        protected override void ChangeValue()
        {
            _slider.value = _damageable.CurrentHealth;
            _image.color = _gradient.Evaluate(_slider.normalizedValue);
        }
    }
}