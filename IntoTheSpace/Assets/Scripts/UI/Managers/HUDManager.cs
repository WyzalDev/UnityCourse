// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using UnityEngine;
using Core;
using UI.HUD;
using UI.HUD.Bars;

namespace UI.Managers
{
    public class HUDManager : MonoBehaviour
    {
        [SerializeField] private PlayerHealthBar _healthBar;
        [SerializeField] private UltimateBar _ultimateBar;
        [SerializeField] private UltimateIcon _ultimateIcon;

        public void SetNewPlayer(Ship player)
        {
            _healthBar.SetNewPlayer(player);

            Ultimate ultimate = null;
            if (player != null)
                ultimate = player.Ultimate as Ultimate;

            _ultimateBar.SetNewUltimate(ultimate);
            _ultimateIcon.SetNewUltimate(ultimate);
        }
    }
}