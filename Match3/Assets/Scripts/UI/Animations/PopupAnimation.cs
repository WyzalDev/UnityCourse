// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using UnityEngine;
using DG.Tweening;

namespace UI.Animations
{
    [RequireComponent(typeof(RectTransform))]
    public class PopupAnimation : MonoBehaviour
    {
        [SerializeField] private float _shiftUp;
        [SerializeField] private float _duration;

        private void Start()
        {
            var rectTransform = GetComponent<RectTransform>();

            rectTransform.DOAnchorPosY(rectTransform.anchoredPosition.y + _shiftUp, _duration)
                .SetEase(Ease.OutQuart)
                .SetUpdate(true)
                .OnComplete(() => Destroy(gameObject));
        }
    }
}