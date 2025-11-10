// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using Core.Grid;

namespace Core.Data
{
    public static class SelectedElement
    {
        public static Element Element { get; private set; }

        public static void SetNewSelectedElement(Element element)
        {
            Element = element;
        }
    }
}