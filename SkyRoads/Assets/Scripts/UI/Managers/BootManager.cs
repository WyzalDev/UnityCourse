// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using UnityEngine;
using Audio.Managers;
using UI.Views.Menu;

namespace UI.Managers
{
    public class BootManager : MonoBehaviour
    {
        private void Start()
        {
            PageManager.Show<MainMenuPage>();

            if (!ComicPage.IsFirstTime)
                return;

            PageManager.Show<ComicPage>();
            ComicPage.IsFirstTime = false;

            AudioManager.PlayMusic("MenuMusic");
        }
    }
}