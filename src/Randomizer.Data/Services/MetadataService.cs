﻿using System;
using System.Collections.Generic;
using System.Linq;
using Randomizer.Data.WorldData.Regions;
using Randomizer.Data.WorldData;
using Randomizer.Data.Configuration.ConfigTypes;
using Microsoft.Extensions.Logging;
using Randomizer.Shared.Enums;
using Randomizer.Shared;
using Randomizer.Data.Configuration;

namespace Randomizer.Data.Services
{
    /// <summary>
    /// Service for retrieving additional metadata information
    /// about objects and locations within the world
    /// </summary>
    public class MetadataService : IMetadataService
    {
        private readonly ILogger<MetadataService> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configs">All configs</param>
        /// <param name="logger"></param>
        public MetadataService(Configs configs, ILogger<MetadataService> logger)
        {
            Regions = configs.Regions;
            Dungeons = configs.Dungeons;
            Rooms = configs.Rooms;
            Locations = configs.Locations;
            Bosses = configs.Bosses;
            Items = configs.Items;
            Rewards = configs.Rewards;
            _logger = logger;
        }

        /// <summary>
        /// Collection of all additional region information
        /// </summary>
        public IReadOnlyCollection<RegionInfo> Regions { get; }

        /// <summary>
        /// Collection of all additional dungeon information
        /// </summary>
        public IReadOnlyCollection<DungeonInfo> Dungeons { get; }

        /// <summary>
        /// Collection of all additional room information
        /// </summary>
        public IReadOnlyCollection<RoomInfo> Rooms { get; }

        /// <summary>
        /// Collection of all additional location information
        /// </summary>
        public IReadOnlyCollection<LocationInfo> Locations { get; }

        /// <summary>
        /// Collection of all additional boss information
        /// </summary>
        public IReadOnlyCollection<BossInfo> Bosses { get; }

        /// <summary>
        /// Collection of all additional item information
        /// </summary>
        public IReadOnlyCollection<ItemData> Items { get; }

        /// <summary>
        /// Collection of all additional reward information
        /// </summary>
        public IReadOnlyCollection<RewardInfo> Rewards { get; }

        /// <summary>
        /// Returns extra information for the specified region.
        /// </summary>
        /// <param name="name">
        /// The name or fully qualified type name of the region.
        /// </param>
        /// <returns>
        /// A new <see cref="RegionInfo"/> for the specified region.
        /// </returns>
        public RegionInfo Region(string name)
            => Regions.Single(x => x.Type?.FullName == name || x.Region == name);

        /// <summary>
        /// Returns extra information for the specified region.
        /// </summary>
        /// <param name="type">
        /// The Randomizer.SMZ3 type matching the region.
        /// </param>
        /// <returns>
        /// A new <see cref="RegionInfo"/> for the specified region.
        /// </returns>
        public RegionInfo Region(Type type)
            => Regions.Single(x => x.Type == type);

        /// <summary>
        /// Returns extra information for the specified region.
        /// </summary>
        /// <param name="region">The region to get extra information for.</param>
        /// <returns>
        /// A new <see cref="RegionInfo"/> for the specified region.
        /// </returns>
        public RegionInfo Region(Region region)
            => Region(region.GetType());

        /// <summary>
        /// Returns extra information for the specified region.
        /// </summary>
        /// <typeparam name="TRegion">
        /// The type of region to get extra information for.
        /// </typeparam>
        /// <returns>
        /// A new <see cref="RegionInfo"/> for the specified region.
        /// </returns>
        public RegionInfo Region<TRegion>() where TRegion : Region
            => Region(typeof(TRegion));

        /// <summary>
        /// Returns extra information for the specified dungeon.
        /// </summary>
        /// <param name="name">
        /// The name or fully qualified type name of the dungeon region.
        /// </param>
        /// <returns>
        /// A new <see cref="DungeonInfo"/> for the specified dungeon region, or
        /// <c>null</c> if <paramref name="name"/> is not a valid dungeon.
        /// </returns>
        public DungeonInfo? Dungeon(string name)
            => Dungeons.SingleOrDefault(x => x.Type?.FullName == name || x.Dungeon == name);

        /// <summary>
        /// Returns extra information for the specified dungeon.
        /// </summary>
        /// <param name="type">
        /// The type of dungeon to be looked up
        /// </param>
        /// <returns>
        /// A new <see cref="DungeonInfo"/> for the specified dungeon region, or
        /// <c>null</c> if <paramref name="type"/> is not a valid dungeon.
        /// </returns>
        public DungeonInfo Dungeon(Type type)
            => Dungeons.Single(x => type == x.Type);

        /// <summary>
        /// Returns extra information for the specified dungeon.
        /// </summary>
        /// <param name="region">
        /// The dungeon region to get extra information for.
        /// </param>
        /// <returns>
        /// A new <see cref="DungeonInfo"/> for the specified dungeon region.
        /// </returns>
        public DungeonInfo Dungeon(Region region)
            => Dungeon(region.GetType());

        /// <summary>
        /// Returns extra information for the specified dungeon.
        /// </summary>
        /// <typeparam name="TRegion">
        /// The type of region that represents the dungeon to get extra
        /// information for.
        /// </typeparam>
        /// <returns>
        /// A new <see cref="DungeonInfo"/> for the specified dungeon region.
        /// </returns>
        public DungeonInfo Dungeon<TRegion>() where TRegion : Region
            => Dungeon(typeof(TRegion));

        /// <summary>
        /// Returns extra information for the specified dungeon.
        /// </summary>
        /// <param name="dungeon">
        /// The dungeon to get extra information for.
        /// </param>
        /// <returns>
        /// A new <see cref="DungeonInfo"/> for the specified dungeon region.
        /// </returns>
        public DungeonInfo Dungeon(IDungeon dungeon)
            => Dungeon(dungeon.GetType());

        /// <summary>
        /// Returns extra information for the specified room.
        /// </summary>
        /// <param name="name">
        /// The name or fully qualified type name of the room.
        /// </param>
        /// <returns>
        /// A new <see cref="RoomInfo"/> for the specified room.
        /// </returns>
        public RoomInfo Room(string name)
            => Rooms.Single(x => x.Type?.FullName == name || x.Room == name);

        /// <summary>
        /// Returns extra information for the specified room.
        /// </summary>
        /// <param name="type">
        /// The type of the room.
        /// </param>
        /// <returns>
        /// A new <see cref="RoomInfo"/> for the specified room.
        /// </returns>
        public RoomInfo? Room(Type type)
            => Rooms.SingleOrDefault(x => x.Type == type);

        /// <summary>
        /// Returns extra information for the specified room.
        /// </summary>
        /// <param name="room">The room to get extra information for.</param>
        /// <returns>
        /// A new <see cref="RoomInfo"/> for the specified room.
        /// </returns>
        public RoomInfo? Room(Room room)
            => Room(room.GetType());

        /// <summary>
        /// Returns extra information for the specified room.
        /// </summary>
        /// <typeparam name="TRoom">
        /// The type of room to get extra information for.
        /// </typeparam>
        /// <returns>
        /// A new <see cref="RoomInfo"/> for the specified room.
        /// </returns>
        public RoomInfo? Room<TRoom>() where TRoom : Room
            => Room(typeof(TRoom));

        /// <summary>
        /// Returns extra information for the specified location.
        /// </summary>
        /// <param name="id">The ID of the location.</param>
        /// <returns>
        /// A new <see cref="LocationInfo"/> for the specified room.
        /// </returns>
        public LocationInfo Location(LocationId id)
            => Locations.Single(x => x.LocationNumber == (int)id);

        /// <summary>
        /// Returns extra information for the specified location.
        /// </summary>
        /// <param name="location">
        /// The location to get extra information for.
        /// </param>
        /// <returns>
        /// A new <see cref="LocationInfo"/> for the specified room.
        /// </returns>
        public LocationInfo Location(Location location)
            => Locations.Single(x => x.LocationNumber == (int)location.Id);

        /// <summary>
        /// Returns information about a specified boss
        /// </summary>
        /// <param name="name">The name of the boss</param>
        /// <returns>The <see cref="BossInfo"/> for the specified boss.</returns>
        public BossInfo? Boss(string name)
            => Bosses.SingleOrDefault(x => x.Boss == name);

        /// <summary>
        /// Returns information about a specified boss
        /// </summary>
        /// <param name="boss">The type of the boss</param>
        /// <returns>The <see cref="BossInfo"/> for the specified boss.</returns>
        public BossInfo? Boss(BossType boss)
            => Bosses.SingleOrDefault(x => x.Type == boss);

        /// <summary>
        /// Returns information about a specified item
        /// </summary>
        /// <param name="type">The type of the item</param>
        /// <returns></returns>
        public ItemData? Item(ItemType type)
            => Items.FirstOrDefault(x => x.InternalItemType == type);

        /// <summary>
        /// Returns information about a specified item
        /// </summary>
        /// <param name="name">The name of the item</param>
        /// <returns></returns>
        public ItemData? Item(string name)
            => Items.SingleOrDefault(x => x.Item == name);

        /// <summary>
        /// Returns information about a specified reward
        /// </summary>
        /// <param name="type">The type of the reward</param>
        /// <returns></returns>
        public RewardInfo? Reward(RewardType type)
            => Rewards.FirstOrDefault(x => x.RewardType == type);
    }
}
