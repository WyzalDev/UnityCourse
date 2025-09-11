// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using UnityEngine;
using Core.Data;

namespace Core.Camera
{
    public class SmoothFollower : MonoBehaviour, IBoostable, IPausable
    {
        [SerializeField] private Transform _target;
        [SerializeField] private Transform _boostTarget;
        [SerializeField] private float _smoothSpeed;

        public bool IsPaused { get; set; }

        private Vector3 _defaultOffset;
        private Vector3 _boostOffset;
        private Vector3 _currentOffset;

        private void Start()
        {
            _defaultOffset = transform.position - _target.position;
            _boostOffset = transform.position - _boostTarget.position;

            _currentOffset = _defaultOffset;
        }

        public void OnBoostStart(BoostData parameters)
        {
            _currentOffset = _boostOffset;
        }

        public void OnBoostEnd(BoostData parameters)
        {
            _currentOffset = _defaultOffset;
        }

        private void LateUpdate()
        {
            if(IsPaused)
                return;
            
            var targetPosition = _target.position + _currentOffset;

            transform.position = Vector3.Lerp(transform.position, targetPosition, _smoothSpeed * Time.deltaTime);
        }
    }
}