﻿using System.Collections.Generic;

namespace Randomizer.SMZ3.Regions.Zelda.DarkWorld.DeathMountain
{

    public class DarkWorldDeathMountainEast : Z3Region
    {

        public override string Name => "Dark World Death Mountain East";
        public override string Area => "Dark World";

        public DarkWorldDeathMountainEast(World world, Config config) : base(world, config)
        {
            Locations = new List<Location> {
                new Location(this, 256+65, 0x1EB51, LocationType.Regular, "Hookshot Cave - Top Right",
                    items => items.MoonPearl && items.Hookshot),
                new Location(this, 256+66, 0x1EB54, LocationType.Regular, "Hookshot Cave - Top Left",
                    items => items.MoonPearl && items.Hookshot),
                new Location(this, 256+67, 0x1EB57, LocationType.Regular, "Hookshot Cave - Bottom Left",
                    items => items.MoonPearl && items.Hookshot),
                new Location(this, 256+68, 0x1EB5A, LocationType.Regular, "Hookshot Cave - Bottom Right",
                    items => items.MoonPearl && (items.Hookshot || items.Boots)),
                new Location(this, 256+69, 0x1EA7C, LocationType.Regular, "Superbunny Cave - Top",
                    items => items.MoonPearl),
                new Location(this, 256+70, 0x1EA7F, LocationType.Regular, "Superbunny Cave - Bottom",
                    items => items.MoonPearl),
            };
        }

        public override bool CanEnter(Progression items)
        {
            return items.CanLiftHeavy() && World.LightWorldDeathMountainEast.CanEnter(items);
        }

    }

}
