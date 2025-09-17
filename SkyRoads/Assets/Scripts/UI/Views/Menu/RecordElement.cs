// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System;
using TMPro;
using UnityEngine;
using Core.Data;

namespace UI.Views.Menu
{
    public class RecordElement : MonoBehaviour
    {
        [SerializeField] private TMP_Text _rankText;
        [SerializeField] private TMP_Text _dateText;
        [SerializeField] private TMP_Text _scoreText;
        [SerializeField] private TMP_Text _timingText;

        public void Initialize(int rank, RecordInfo recordInfo)
        {
            _rankText.text = rank.ToString();
            _dateText.text = recordInfo.RaceDate.ToString("d");
            _scoreText.text = recordInfo.Score.ToString();
            _timingText.text = TimeSpan.FromSeconds(recordInfo.RaceTime).ToString("mm':'ss':'fff");
        }
    }
}