// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System;

namespace Core.Data
{
    [Serializable]
    public class RecordInfo
    {
        public long Score;
        public DateTime Date;

        public RecordInfo(long score, DateTime date)
        {
            Score = score;
            Date = date;
        }
    }
}