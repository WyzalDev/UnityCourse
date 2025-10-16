// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core.Records;
using UI.Managers;

namespace UI.Views.Menu
{
    public class RecordsPage : Page
    {
        [SerializeField] private RecordElement _recordElementPrefab;
        [SerializeField] private Transform _parent;
        [SerializeField] private Button _backButton;

        private List<RecordElement> _recordElements = new List<RecordElement>();

        private void Start()
        {
            _backButton.onClick.AddListener(OnBackButtonClicked);
        }

        public override void Show(object data = null)
        {
            for (var i = 0; i < ScoreRecords.Records.Count; i++)
            {
                var recordGameObject = Instantiate(_recordElementPrefab, _parent);
                recordGameObject.Initialize(i + 1, ScoreRecords.Records[i]);
                _recordElements.Add(recordGameObject);
            }

            base.Show(data);
        }

        public override void Hide()
        {
            base.Hide();

            foreach (var recordElement in _recordElements)
                Destroy(recordElement.gameObject);

            _recordElements.Clear();
        }

        private void OnBackButtonClicked()
        {
            PageManager.Last();
        }

        private void OnDestroy()
        {
            _backButton.onClick.RemoveListener(OnBackButtonClicked);
        }
    }
}