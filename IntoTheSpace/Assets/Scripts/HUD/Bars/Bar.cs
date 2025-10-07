// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using UnityEngine;
using UnityEngine.UI;

namespace HUD.Bars
{
    public abstract class Bar : MonoBehaviour
    {
        [SerializeField] protected Slider _slider;
        [SerializeField] protected Image _image;
        [SerializeField] protected Gradient _gradient;

        protected abstract void ChangeValue();
    }
}