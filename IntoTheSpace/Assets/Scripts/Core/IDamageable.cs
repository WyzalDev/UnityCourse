// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System;

namespace Core
{
    public interface IDamageable
    {
        public Action OnDamageTaken { get; set; }
        public int CurrentHealth { get; }
        public int MaxHealth { get; }

        public void TakeDamage(int damage);
    }
}