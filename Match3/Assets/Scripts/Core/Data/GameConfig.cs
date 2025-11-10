// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using UnityEngine;

namespace Core.Data
{
    [CreateAssetMenu(fileName = "NewGameConfig", menuName = "Game/Config")]
    public class GameConfig : ScriptableObject
    {
        [SerializeField] private float _endGameDelay;

        public float EndGameDelay => _endGameDelay;
    }
}