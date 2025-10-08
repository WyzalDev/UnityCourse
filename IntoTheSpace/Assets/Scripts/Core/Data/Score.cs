// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System;

namespace Core.Data
{
    public static class Score
    {
        public static Action OnValueChanged;

        private static long _value;

        public static long Value
        {
            get => _value;
            private set
            {
                _value = value;
                OnValueChanged?.Invoke();
            }
        }

        public static void AddScore(long value)
        {
            if (value <= 0)
                return;

            Value += value;
        }

        public static void Restore()
        {
            Value = 0;
        }
    }
}