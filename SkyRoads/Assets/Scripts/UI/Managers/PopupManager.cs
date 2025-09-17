// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System.Linq;
using UI.Views;

namespace UI.Managers
{
    public class PopupManager : ViewManager<Popup>
    {
        private static PopupManager _instance;

        protected void Awake()
        {
            _instance = this;
            base.Awake();
        }

        public static bool TryGetView<TPopup>(out TPopup view) where TPopup : Popup
        {
            view = _instance._views.FirstOrDefault(x => x is TPopup) as TPopup;

            return view is not null;
        }

        public static void ShowPopup<TPopup>(object data = null) where TPopup : Popup, new()
        {
            if (!TryGetView(out TPopup view))
                return;

            _instance.Show(view, data);
        }

        public static void HidePopup()
        {
            _instance.ShowLast();
        }
    }
}