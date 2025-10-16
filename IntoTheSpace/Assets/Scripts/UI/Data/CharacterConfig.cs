// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using UnityEngine;
using Core;

namespace UI.Data
{
    [CreateAssetMenu(fileName = "CharacterConfig", menuName = "UI/CharacterConfig", order = 0)]
    public class CharacterConfig : ScriptableObject
    {
        [Tooltip("Parameters : {0} - name, {1} - turretType, {2} - Attack speed")]
        [SerializeField] [TextArea] private string _description;
        [SerializeField] private Ship _shipPrefab;

        public string Description => string.Format(_description, _shipPrefab.ShipConfig.ShipName,
            _shipPrefab.ShipConfig.BaseTurretData.Description, _shipPrefab.ShipConfig.BaseTurretData.FireRate);
        public Ship ShipPrefab => _shipPrefab;
    }
}