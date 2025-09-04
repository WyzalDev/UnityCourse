// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System;
using UnityEngine;
using Core.RoadObjects;

namespace Core.MovingContainer
{
    public class PlayerArea : MonoBehaviour
    {
        public Action AsteroidPassed;

        private void InvokeAsteroidPassed()
        {
            AsteroidPassed?.Invoke();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Asteroid>(out var asteroid))
                InvokeAsteroidPassed();
        }
    }
}