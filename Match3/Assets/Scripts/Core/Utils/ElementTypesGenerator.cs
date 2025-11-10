// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System;
using Core.Data;

namespace Core.Utils
{
    public class ElementTypesGenerator
    {
        private Random random;

        private const int GemTypesCount = 5;

        public ElementTypesGenerator(int seed)
        {
            random = new Random(seed);
        }

        public ElementType GetNextGemType()
        {
            var randomNumber = random.Next(GemTypesCount) + 1;

            if (Enum.IsDefined(typeof(ElementType), randomNumber))
                return (ElementType)randomNumber;

            return ElementType.None;
        }
    }
}