// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using UnityEngine;
using TMPro;
using Core.Data;

namespace UI.Views.Menu
{
    public class RecordElement : MonoBehaviour
    {
        [SerializeField] private TMP_Text _rankText;
        [SerializeField] private TMP_Text _dateText;
        [SerializeField] private TMP_Text _scoreText;

        public void Initialize(int rank, RecordInfo recordInfo)
        {
            _rankText.text = rank.ToString();
            _dateText.text = recordInfo.Date.ToString("d");
            _scoreText.text = recordInfo.Score.ToString();
        }
    }
}