// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using UnityEngine;
using UI.Views.Menu;

namespace UI.Managers
{
    public class BootManager : MonoBehaviour
    {
        private void Start()
        {
            PageManager.Show<MainMenuPage>();
        }
    }
}