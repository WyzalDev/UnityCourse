// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;
using Core.Grid;

namespace Core.Data
{
    [CreateAssetMenu(fileName = "DefaultElementsConfig", menuName = "Gameplay/ElementsConfig", order = 0)]
    public class ElementsConfig : ScriptableObject
    {
        [SerializeField] private ElementView _elementViewPrefab;
        [SerializeField] private List<ElementConfig> _elementConfigs;
        [SerializeField] private List<BackgroundElementConfig> _backgroundElementConfigs;

        [Header("Animation Settings")]
        [SerializeField] private SpriteRenderer _animationSpritePrefab;
        [SerializeField] private float _destroyGemsAnimationDuration;
        [SerializeField] private float _moveGemsAnimationDuration;

        [Header("ChainSettings")]
        [SerializeField] private int _rocketChainSize;
        [SerializeField] [Tooltip("Must be greater than rocket chain size")] private int _bombChainSize;

        public ElementView ElementViewPrefab => _elementViewPrefab;

        public List<ElementConfig> ElementConfigs => _elementConfigs;

        public List<BackgroundElementConfig> BackgroundElementConfigs => _backgroundElementConfigs;

        public SpriteRenderer AnimationSpritePrefab => _animationSpritePrefab;

        public float DestroyGemsAnimationDuration => _destroyGemsAnimationDuration;

        public float MoveGemsAnimationDuration => _moveGemsAnimationDuration;

        public int RocketChainSize => _rocketChainSize;

        public int BombChainSize => _bombChainSize;
    }
}