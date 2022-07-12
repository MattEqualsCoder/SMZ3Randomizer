﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Randomizer.Shared;
using Randomizer.SMZ3.Tracking.Configuration;
using Randomizer.SMZ3.Tracking.VoiceCommands;

namespace Randomizer.SMZ3.Tracking.AutoTracking
{
    public class GameInteractor
    {
        private readonly AutoTracker _autoTracker;
        private readonly Tracker _tracker;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameInteractor"/>
        /// class.
        /// </summary>
        /// <param name="tracker">The tracker instance.</param>
        /// <param name="autoTracker">The auto tracker to associate with this module</param>
        public GameInteractor(Tracker tracker, AutoTracker autoTracker)
        {
            _tracker = tracker;
            _autoTracker = autoTracker;
            tracker.GameInteractor = this;
            autoTracker.GameInteractor = this;
        }

        /// <summary>
        /// Gives an item to the player
        /// </summary>
        /// <param name="item">The item to give</param>
        /// <param name="fromPlayerId">The id of the player giving the item to the player (0 for tracker)</param>
        public void GiveItem(ItemData item, int fromPlayerId = 0)
        {
            // First give the player the item by the requested 
            var bytes = new List<byte>();
            bytes.AddRange(Int16ToBytes(fromPlayerId + 1));
            bytes.AddRange(Int16ToBytes((int)item.InternalItemType));
            var action = new EmulatorAction()
            {
                Type = EmulatorActionType.WriteBytes,
                Domain = MemoryDomain.CartRAM,
                Address = 0xA26000 + (ItemCounter * 4),
                WriteValues = bytes
            };
            _autoTracker.WriteToMemory(action);

            // Up the item counter to have them actually pick it up
            action = new EmulatorAction()
            {
                Type = EmulatorActionType.WriteBytes,
                Domain = MemoryDomain.CartRAM,
                Address = 0xA26602,
                WriteValues = Int16ToBytes(ItemCounter + 1)
            };
            _autoTracker.WriteToMemory(action);

            // Track the item
            _tracker.TrackItem(item, null, null, false, true);

            ItemCounter++;
        }

        /// <summary>
        /// Restores the player to max health
        /// </summary>
        public void HealPlayer()
        {
            if (_autoTracker.CurrentGame == Game.Zelda)
            {
                _autoTracker.WriteToMemory(new EmulatorAction()
                {
                    Type = EmulatorActionType.WriteBytes,
                    Domain = MemoryDomain.WRAM,
                    Address = 0x7EF372,
                    WriteValues = new List<byte>() { 0xA0 }
                });
            }
            else if (_autoTracker.CurrentGame == Game.SM && _autoTracker.MetroidState != null)
            {
                var maxHealth = _autoTracker.MetroidState.MaxHealth;
                var maxReserves = _autoTracker.MetroidState.MaxReserveTanks;
                _autoTracker.WriteToMemory(new EmulatorAction()
                {
                    Type = EmulatorActionType.WriteBytes,
                    Domain = MemoryDomain.WRAM,
                    Address = 0x7E09C2,
                    WriteValues = Int16ToBytes(maxHealth)
                });

                _autoTracker.WriteToMemory(new EmulatorAction()
                {
                    Type = EmulatorActionType.WriteBytes,
                    Domain = MemoryDomain.WRAM,
                    Address = 0x7E09D6,
                    WriteValues = Int16ToBytes(maxReserves)
                });
            }
        }

        /// <summary>
        /// The number of items given to the player
        /// </summary>
        public int ItemCounter { get; set; }

        private static byte[] Int16ToBytes(int value)
        {
            return new byte[] { (byte)(value % 256), (byte)(value / 256) };
        }
    }
}
