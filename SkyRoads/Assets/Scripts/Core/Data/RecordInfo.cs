// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using System;

namespace Core.Data
{
    [Serializable]
    public class RecordInfo
    {
        public long Score;
        public DateTime RaceDate;
        public float RaceTime;

        public RecordInfo(long score, DateTime raceDate, float raceTime)
        {
            Score = score;
            RaceDate = raceDate;
            RaceTime = raceTime;
        }
    }
}