﻿using System;

using Randomizer.SMZ3.Tracking.Vocabulary;

namespace Randomizer.SMZ3.Tracking
{
    public class ZeldaDungeon
    {
        public ZeldaDungeon(SchrodingersString name, string abbreviation)
        {
            Name = name;
            Abbreviation = abbreviation;
        }

        public SchrodingersString Name { get; }

        public string Abbreviation { get; }

        public SchrodingersString Boss { get; init; }

        public int? Column { get; set; }

        public int? Row { get; set; }

        public RewardItem Reward { get; set; }
            = RewardItem.Unknown;

        public bool Cleared { get; set; }
    }
}
