﻿using Microsoft.AspNetCore.SignalR;
using Randomizer.Shared.Multiplayer;

namespace Randomizer.Multiplayer.Server;

public class GameManager
{
    private readonly ILogger<GameManager> _logger;
    private readonly IHubContext<MultiplayerHub> _hubContext;
    private readonly MultiplayerDbService _dbService;
    private readonly int _expirationMinutes;
    private readonly int _checkFrequencyMinutes;
    private readonly int _databaseExpirationDays;

    public GameManager(ILogger<GameManager> logger, IHubContext<MultiplayerHub> hubContext, IConfiguration configuration, MultiplayerDbService dbService)
    {
        _logger = logger;
        _hubContext = hubContext;
        _dbService = dbService;

        _expirationMinutes = configuration.GetValue("SMZ3:GameMemoryExpirationInMinutes", 60);
        _checkFrequencyMinutes = configuration.GetValue("SMZ3:GameCheckFrequencyInMinutes", 15);
        _databaseExpirationDays = configuration.GetValue("SMZ3:GameDatabaseExpirationInDays", 30);

        Task.Run(CheckGames);
    }

    /// <summary>
    /// Checks games to see if they need to be cleared from memory or deleted from the database
    /// </summary>
    private async Task CheckGames()
    {
        while (true)
        {
            var expiredGuids = MultiplayerGame.ExpireGamesInMemory(_expirationMinutes);
            _logger.LogInformation("Removed {Amount} inactive games(s) from memory", expiredGuids.Count);

            // For any games that we removed from memory, tell any connected players to disconnect
            if (expiredGuids.Any())
            {
                foreach (var guid in expiredGuids)
                {
                    await _hubContext.Clients.Group(guid).SendAsync("Disconnect", new MultiplayerRequest());
                }
            }

            expiredGuids = await _dbService.DeleteOldGameStates(_databaseExpirationDays);
            _logger.LogInformation("Removed {Amount} inactive games(s) from database", expiredGuids.Count);

            _logger.LogInformation("Current active games: {GameCount} | Current connected players: {PlayerCount}", MultiplayerGame.GameCount, MultiplayerGame.PlayerCount);

            await Task.Delay(TimeSpan.FromMinutes(_checkFrequencyMinutes));
        }
    }

}
