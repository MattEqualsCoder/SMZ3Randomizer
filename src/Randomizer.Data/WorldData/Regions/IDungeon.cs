﻿using System.Linq;
using Randomizer.Data.Configuration.ConfigTypes;
using Randomizer.Data.WorldData;
using Randomizer.Shared;
using Randomizer.Shared.Models;

namespace Randomizer.Data.WorldData.Regions
{
    /// <summary>
    /// Defines a region that offers a reward for completing it, e.g. a Zelda
    /// dungeon or a Super Metroid boss.
    /// </summary>
    public interface IDungeon
    {
        /// <summary>
        /// Gets or sets the reward for completing the region, e.g. pendant or
        /// crystal.
        /// </summary>
        DungeonInfo DungeonMetadata { get; set; }

        /// <summary>
        /// The current tracking state of the dungeon
        /// </summary>
        TrackerDungeonState DungeonState { get; set; }

        /// <summary>
        /// Calculates the number of treasures in the dungeon
        /// </summary>
        /// <returns></returns>
        public int GetTreasureCount()
        {
            var region = (Region)this;
            return region.Locations.Count(x => x.Item != null && (!x.Item.IsDungeonItem || region.World.Config.ZeldaKeysanity) && x.Type != LocationType.NotInDungeon);
        }

        /// <summary>
        /// Retrieves the base name of the dungeon
        /// </summary>
        public string DungeonName => ((Region)this).Name;

        /// <summary>
        /// The reward object for the dungeon, if any
        /// </summary>
        public Reward? Reward => HasReward ? ((IHasReward)this).Reward : null;

        /// <summary>
        /// The type of reward in the dungeon, if any
        /// </summary>
        public RewardType RewardType => Reward?.Type ?? RewardType.None;

        /// <summary>
        /// The reward marked by the player
        /// </summary>
        public RewardType MarkedReward
        {
            get
            {
                return DungeonState?.MarkedReward ?? RewardType.None;
            }
            set
            {
                DungeonState.MarkedReward = value;
            }
        }

        /// <summary>
        /// If this dungeon has a reward in it
        /// </summary>
        public bool HasReward => this is IHasReward;

        /// <summary>
        /// If this dungeon needs a medallion
        /// </summary>
        public bool NeedsMedallion => this is INeedsMedallion;

        /// <summary>
        /// The medallion required to entered the dungeon, if any
        /// </summary>
        public ItemType Medallion => NeedsMedallion ? ((INeedsMedallion)this).Medallion : ItemType.Nothing;

        /// <summary>
        /// The required medallion marked by the player
        /// </summary>
        public ItemType MarkedMedallion
        {
            get
            {
                return DungeonState?.MarkedMedallion ?? ItemType.Nothing;
            }
            set
            {
                DungeonState.MarkedMedallion = value;
            }
        }

        /// <summary>
        /// The overworld region that this dungeon is within
        /// </summary>
        public Region ParentRegion { get; }
    }
}
