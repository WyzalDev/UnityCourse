// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System.Linq;

namespace UI
{
    public class PageManager : ViewManager<Page>
    {
        private static PageManager _instance;

        protected void Awake()
        {
            _instance = this;
        }

        public static bool TryGetView<TPage>(out TPage view) where TPage : Page
        {
            view = _instance._views.FirstOrDefault(x => x is TPage) as TPage;

            return view is not null;
        }

        public static void Show<TPage>(object data = null) where TPage : Page
        {
            if (!TryGetView(out TPage view))
                return;

            _instance.Show(view, data);
        }

        public static void Last()
        {
            _instance.ShowLast();
        }
    }
}