// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public abstract class ViewManager<T> : MonoBehaviour where T : View
    {
        [SerializeField] protected T[] _views;

        private View _currentView;
        protected readonly Stack<T> _history = new();

        protected void Start()
        {
            foreach (var view in _views)
                view.Hide();
        }

        protected void Show(T view, object data = null)
        {
            if (_currentView != null)
                _currentView.Hide();

            view.Show(data);
            _history.Push(view);
            _currentView = view;
        }

        protected void ShowLast()
        {
            if (_history.Count <= 0)
                return;

            var view = _history.Pop();
            view.Hide();

            if (_history.Count <= 0)
                return;

            _currentView = _history.Peek();
            _currentView.Show();
        }
    }
}