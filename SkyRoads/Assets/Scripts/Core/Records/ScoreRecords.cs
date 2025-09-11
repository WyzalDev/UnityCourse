// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System.Collections.Generic;
using Core.Data;

namespace Core.Records
{
    public static class ScoreRecords
    {
        private static List<RecordInfo> _scoreRecords = new();

        public static bool TryAdd(RecordInfo record)
        {
            if (_scoreRecords.Count == 0)
            {
                _scoreRecords.Add(record);
                return true;
            }

            if(_scoreRecords.Contains(record) || _scoreRecords[0].Score >= record.Score)
                return false;

            _scoreRecords.Insert(0, record);
            return true;
        }
    }
}