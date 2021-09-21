﻿using System.Collections.Generic;

using static Randomizer.SMZ3.ItemType;

namespace Randomizer.SMZ3.Regions.Zelda
{

    class CastleTower : Z3Region, IHasReward {

        public override string Name => "Castle Tower";

        public Reward Reward { get; set; } = Reward.Agahnim;

        public CastleTower(World world, Config config) : base(world, config) {
            RegionItems = new[] { KeyCT };

            Locations = new List<Location> {
                new Location(this, 256+101, 0x1EAB5, LocationType.Regular, "Castle Tower - Foyer"),
                new Location(this, 256+102, 0x1EAB2, LocationType.Regular, "Castle Tower - Dark Maze",
                    items => items.Lamp && items.KeyCT >= 1),
            };
        }

        public override bool CanEnter(Progression items) {
            return items.CanKillManyEnemies() && (items.Cape || items.MasterSword);
        }

        public bool CanComplete(Progression items) {
            return CanEnter(items) && items.Lamp && items.KeyCT >= 2 && items.Sword;
        }

    }

}
