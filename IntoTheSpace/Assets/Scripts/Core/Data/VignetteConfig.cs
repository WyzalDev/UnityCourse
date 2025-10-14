// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using UnityEngine;

namespace Core.Data
{
    [CreateAssetMenu(fileName = "VignetteConfig", menuName = "Effects/VignetteConfig")]
    public class VignetteConfig : ScriptableObject
    {
        [SerializeField] private float _intensity;
        [SerializeField] private float _screenTime;
        [SerializeField] private float _appearanceTime;
        [SerializeField] private float _disappearanceTime;
        

        public float Intensity => _intensity;
        public float ScreenTime => _screenTime;
        public float AppearanceTime => _appearanceTime;
        public float DisappearanceTime => _disappearanceTime;
    }
}