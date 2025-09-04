// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;
using Utility;
using Core.MovingContainer;
using Core.RoadObjects;

namespace Core.Managers
{
    public class PlatformManager : MonoBehaviour, IRestorable
    {
        [SerializeField] private BehindCameraArea _behindCameraArea;

        [Header("Platform Settings")]
        [SerializeField] private Platform _platformPrefab;
        [SerializeField] private Transform _platformParent;
        [SerializeField] private int _maxCount;
        [SerializeField] private int _size;

        private CircularList<Platform> _platforms;
        
        private void Start()
        {
            _behindCameraArea.PlatformHitted += BuildNextPlatform;
            
            Initialize();
        }

        private void Initialize()
        {
            _platforms = new CircularList<Platform>();
            var platformList = new List<Platform>();
            
            for (var i = 0; i < _maxCount; i++)
            {
                var newPlatform = Instantiate(_platformPrefab, _platformParent);
                newPlatform.transform.localScale = _platformPrefab.transform.localScale;
                newPlatform.transform.position = new Vector3(transform.position.x, transform.position.y, i * _size);
                platformList.Add(newPlatform);
            }
            
            _platforms.Initialize(platformList);
        }

        public void Restore()
        {
            var platforms = _platforms.RemoveAll();

            foreach (var platform in platforms)
                Destroy(platform.gameObject);
            
            Initialize();
        }

        private void BuildNextPlatform()
        {
            var nextPlatform = _platforms.Current;

            _platforms.Next();
            nextPlatform.transform.Translate(
                new Vector3(transform.position.x, transform.position.y, _maxCount * _size));
        }

        private void OnDestroy()
        {
            _behindCameraArea.PlatformHitted -= BuildNextPlatform;
        }
    }
}