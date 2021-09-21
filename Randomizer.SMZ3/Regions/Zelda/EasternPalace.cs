﻿using System.Collections.Generic;

using static Randomizer.SMZ3.ItemType;

namespace Randomizer.SMZ3.Regions.Zelda
{
    public class EasternPalace : Z3Region, IHasReward
    {

        public override string Name => "Eastern Palace";

        public Reward Reward { get; set; } = Reward.None;

        public EasternPalace(World world, Config config) : base(world, config)
        {
            RegionItems = new[] { BigKeyEP, MapEP, CompassEP };

            Locations = new List<Location> {
                new Location(this, 256+103, 0x1E9B3, LocationType.Regular, "Eastern Palace - Cannonball Chest"),
                new Location(this, 256+104, 0x1E9F5, LocationType.Regular, "Eastern Palace - Map Chest"),
                new Location(this, 256+105, 0x1E977, LocationType.Regular, "Eastern Palace - Compass Chest"),
                new Location(this, 256+106, 0x1E97D, LocationType.Regular, "Eastern Palace - Big Chest",
                    items => items.BigKeyEP),
                new Location(this, 256+107, 0x1E9B9, LocationType.Regular, "Eastern Palace - Big Key Chest",
                    items => items.Lamp),
                new Location(this, 256+108, 0x308150, LocationType.Regular, "Eastern Palace - Armos Knights",
                    items => items.BigKeyEP && items.Bow && items.Lamp),
            };
        }

        public bool CanComplete(Progression items)
        {
            return GetLocation("Eastern Palace - Armos Knights").Available(items);
        }

    }

}
