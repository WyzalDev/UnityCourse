// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using TMPro;
using UnityEngine;

namespace UI.Views
{
    public class WinPage : Page
    {
        [SerializeField] private TMP_Text _score;
        
        public override void Show(object data = null)
        {
            if (data is long score)
                _score.text = score.ToString();

            base.Show(data);
        }
    }
}