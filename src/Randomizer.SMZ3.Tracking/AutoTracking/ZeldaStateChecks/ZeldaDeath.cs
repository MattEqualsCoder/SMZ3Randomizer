﻿using System;
using System.Linq;
using Randomizer.Data.WorldData.Regions;
using Randomizer.SMZ3.Tracking.Services;

namespace Randomizer.SMZ3.Tracking.AutoTracking.ZeldaStateChecks
{
    /// <summary>
    /// Zelda State check for when dying
    /// Checks if the player is in the death spiral animation without a fairy
    /// </summary>
    public class ZeldaDeath : IZeldaStateCheck
    {
        public ZeldaDeath(IItemService itemService)
        {
            Items = itemService;
        }

        public IItemService Items { get; }

        /// <summary>
        /// Executes the check for the current state
        /// </summary>
        /// <param name="tracker">The tracker instance</param>
        /// <param name="currentState">The current state in Zelda</param>
        /// <param name="prevState">The previous state in Zelda</param>
        /// <returns>True if the check was identified, false otherwise</returns>
        public bool ExecuteCheck(Tracker tracker, AutoTrackerZeldaState currentState, AutoTrackerZeldaState prevState)
        {
            if (tracker.AutoTracker == null || currentState.State != 0x12 || prevState.State == 0x12 || tracker.AutoTracker.PlayerHasFairy) return false;

            var silent = tracker.GameService!.PlayerRecentlyKilled;

            // Say specific message for dying in the particular screen/room the player is in
            if (!silent && tracker.CurrentRegion is { WhenDiedInRoom: not null })
            {
                var region = tracker.CurrentRegion.GetRegion(tracker.World) as Z3Region;
                if (region is { IsOverworld: true } && tracker.CurrentRegion.WhenDiedInRoom.TryGetValue(prevState.OverworldScreen, out var locationResponse))
                {
                    tracker.Say(locationResponse);
                }
                else if (region is { IsOverworld: false } && tracker.CurrentRegion.WhenDiedInRoom.TryGetValue(prevState.CurrentRoom, out locationResponse))
                {
                    tracker.Say(locationResponse);
                }
            }

            tracker.TrackDeath(true);

            var death = Items.FirstOrDefault("Death");
            if (death is not null)
            {
                tracker.TrackItem(death, autoTracked: true, silent: silent);
                return true;
            }
            return false;
        }
    }
}
