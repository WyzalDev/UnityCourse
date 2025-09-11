// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using UnityEngine;

namespace Core.Managers
{
    public class Timer : MonoBehaviour, IPausable, IRestorable
    {
        public static float Value { get; private set; }
        public static float GameDifficultMultiplier { get; private set; }

        public bool IsPaused { get; set; }

        private void Start()
        {
            Restore();
        }

        public void Restore()
        {
            Value = 0.001f;
            GameDifficultMultiplier = 0f;
        }

        private void Update()
        {
            if (IsPaused)
                return;

            Value += Time.deltaTime;
            GameDifficultMultiplier = Mathf.Log10(1 + Value / 100);
        }
    }
}