// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Core.Data;
using Core.Grid;
using Core.Utils;

namespace Core.Managers
{
    public class ElementSelectionManager : MonoBehaviour, IPausable, IRestorable
    {
        [SerializeField] private GridManager _gridManager;

        public Action OnSelectionEnd;

        private InputAction _cursorPositionAction;
        private Camera _camera;

        public bool IsPaused { get; set; }

        public bool IsActive { get; set; }

        private void Start()
        {
            _cursorPositionAction = InputSystem.actions.FindAction("CursorPosition");
            _camera = Camera.main;
        }

        private void Update()
        {
            if (IsPaused)
                return;

            if (!IsActive)
                return;

            var currentCursorPosition = _cursorPositionAction.ReadValue<Vector2>();
            var worldCursorPosition = _camera.ScreenToWorldPoint(currentCursorPosition);
            var modelCoordinates = ViewModelCoordinatesConverter.GetModelCoordinates(worldCursorPosition);

            if (!SelectedElementValidation(modelCoordinates, out var checkedElement))
                return;

            SelectedElement.SetNewSelectedElement(checkedElement);
            OnSelectionEnd?.Invoke();
        }

        private bool SelectedElementValidation(Vector2Int modelCoordinates, out Element checkedElement)
        {
            checkedElement = null;

            if (SelectedElement.Element.X == modelCoordinates.x && SelectedElement.Element.Y == modelCoordinates.y)
                return false;

            checkedElement = _gridManager.GetElement(modelCoordinates.x, modelCoordinates.y);

            return checkedElement != null;
        }

        public void Restore()
        {
            SelectedElement.SetNewSelectedElement(null);
        }
    }
}