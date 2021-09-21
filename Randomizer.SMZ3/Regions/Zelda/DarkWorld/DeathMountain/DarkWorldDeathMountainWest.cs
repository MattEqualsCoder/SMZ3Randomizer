﻿using System.Collections.Generic;

namespace Randomizer.SMZ3.Regions.Zelda.DarkWorld.DeathMountain
{
    public class DarkWorldDeathMountainWest : Z3Region
    {

        public override string Name => "Dark World Death Mountain West";
        public override string Area => "Dark World";

        public DarkWorldDeathMountainWest(World world, Config config) : base(world, config)
        {
            Locations = new List<Location> {
                new Location(this, 256+64, 0x1EA8B, LocationType.Regular, "Spike Cave",
                    items => items.MoonPearl && items.Hammer && items.CanLiftLight() &&
                        (items.CanExtendMagic() && items.Cape || items.Byrna) &&
                        World.LightWorldDeathMountainWest.CanEnter(items)),
            };
        }

    }

}
