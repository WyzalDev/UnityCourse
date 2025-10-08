// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using Core;

namespace UI.HUD.Bars
{
    public class UltimateBar : Bar
    {
        private Ultimate _ultimate;

        public void SetNewUltimate(Ultimate ultimate)
        {
            if (_ultimate != null)
                _ultimate.OnUltimateFill -= ChangeValue;

            if (ultimate == null)
                return;

            _ultimate = ultimate;
            _ultimate.OnUltimateFill += ChangeValue;

            _slider.value = 0;
            _image.color = _gradient.Evaluate(_slider.normalizedValue);
        }

        private void ChangeValue(float percentage)
        {
            _slider.value = percentage;
            ChangeValue();
        }

        protected override void ChangeValue()
        {
            _image.color = _gradient.Evaluate(_slider.normalizedValue);
        }

        private void OnDestroy()
        {
            if (_ultimate != null)
                _ultimate.OnUltimateFill -= ChangeValue;
        }
    }
}