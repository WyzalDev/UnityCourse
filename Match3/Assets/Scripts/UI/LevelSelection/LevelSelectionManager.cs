// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Core.Data;

namespace UI.LevelSelection
{
    public static class LevelSelectionManager
    {
        private static Dictionary<int, string> _levels;
        private static Button _playButton;
        private static int _lastSelectedIndex;

        private const string LevelsPath = "Levels";
        private const string LevelPattern = "Level{0}.txt";

        public static int LevelsCount => _levels.Count;

        static LevelSelectionManager()
        {
            LoadLevelsDictionary();
        }

        private static void LoadLevelsDictionary()
        {
            _levels = new Dictionary<int, string>();
            var i = 1;
            while (File.Exists($"Assets/Resources/{LevelsPath}/{string.Format(LevelPattern, i)}"))
            {
                _levels.Add(i, string.Format(LevelPattern, i));
                i++;
            }
        }

        public static LevelConfig GetSelectedLevelConfig()
        {
            try
            {
                var levelConfigText =
                    File.ReadAllText(
                        $"Assets/Resources/{LevelsPath}/{_levels[_lastSelectedIndex]}");

                return JsonUtility.FromJson<LevelConfig>(levelConfigText);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }

            return null;
        }

        public static void SelectNewLevel(int levelIndex)
        {
            _lastSelectedIndex = levelIndex;
        }
    }
}