// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using UnityEngine;

namespace Core.Movements
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class Movement : MonoBehaviour, IPausable, IRestorable, IInitializable
    {
        protected float _shipSpeed;
        protected Rigidbody2D _rigidbody2D;

        private Vector3 _startPosition;

        public bool IsPaused { get; set; }

        protected virtual void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _startPosition = transform.position;
        }

        public void Initialize(object data)
        {
            if (data is float shipSpeed)
                _shipSpeed = shipSpeed;
        }

        protected abstract void FixedUpdate();

        public void Restore()
        {
            transform.position = _startPosition;
        }

        protected virtual void OnDestroy()
        {
        }
    }
}