// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System;
using UnityEngine;
using Core.RoadObjects;

namespace Core.MovingContainer
{
    public class BehindCameraArea : MonoBehaviour
    {
        public Action PlatformHitted;

        private void InvokePlatformHitted()
        {
            PlatformHitted?.Invoke();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<Asteroid>(out var asteroid))
                asteroid.gameObject.SetActive(false);

            if (other.TryGetComponent<Platform>(out var platform))
                InvokePlatformHitted();
        }
    }
}