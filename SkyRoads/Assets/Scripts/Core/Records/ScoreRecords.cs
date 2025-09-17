// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using Core.Data;

namespace Core.Records
{
    public static class ScoreRecords
    {
        public static List<RecordInfo> Records { get; } = new();

        public static bool TryAdd(RecordInfo record)
        {
            if (Records.Count == 0)
            {
                Records.Add(record);
                return true;
            }

            if (Records.Contains(record) || Records[0].Score >= record.Score)
                return false;

            Records.Insert(0, record);
            return true;
        }
    }
}