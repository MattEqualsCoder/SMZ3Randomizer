﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Logging;
using Randomizer.Data.Options;
using Randomizer.Data.WorldData;
using Randomizer.Shared;
using Randomizer.SMZ3.Contracts;
using Randomizer.SMZ3.FileData;

namespace Randomizer.SMZ3.Generation;

public class Smz3MultiplayerRomGenerator : ISeededRandomizer
{

    private readonly MultiplayerFillerFactory _fillerFactory;
    private readonly IWorldAccessor _worldAccessor;
    private readonly ILogger<Smz3MultiplayerRomGenerator> _logger;
    private readonly IPatcherService _patcherService;

    public Smz3MultiplayerRomGenerator(MultiplayerFillerFactory fillerFactory, IWorldAccessor worldAccessor,
        ILogger<Smz3MultiplayerRomGenerator> logger, IPatcherService patcherService)
    {
        _fillerFactory = fillerFactory;
        _worldAccessor = worldAccessor;
        _logger = logger;
        _patcherService = patcherService;
    }

    public SeedData GenerateSeed(Config config, CancellationToken cancellationToken = default) =>
        GenerateSeed(config, "", cancellationToken);

    public SeedData GenerateSeed(Config config, string? seed, CancellationToken cancellationToken = default) =>
        GenerateSeed(new List<Config>() { config }, seed, cancellationToken);

    public SeedData GenerateSeed(List<Config> configs, string? seed = "", CancellationToken cancellationToken = default)
    {
        var orderedConfigs = configs.OrderBy(x => x.Id).ToList();
        var primaryConfig = orderedConfigs.First();

        var seedNumber = ISeededRandomizer.ParseSeed(ref seed);
        primaryConfig.Seed = seedNumber.ToString();

        var worlds = orderedConfigs
            .Select(c => new World(c, c.PlayerName, c.Id, c.PlayerGuid, c.IsLocalConfig)).ToList();

        _logger.LogDebug(
            "Seed: {SeedNumber} | Race: {PrimaryConfigRace} | World Count: {Count}",
            seedNumber, primaryConfig.Race, configs.Count);

        var filler = _fillerFactory.Create();
        filler.Fill(worlds, primaryConfig, cancellationToken);
        var playthrough = Playthrough.Generate(worlds, primaryConfig);

        var seedData = new SeedData
        (
            guid: Guid.NewGuid().ToString("N"),
            seed: string.IsNullOrEmpty(seed) ? primaryConfig.Seed : seed,
            game: "SMZ3 Cas’ Multiplayer",
            mode: primaryConfig.GameMode.ToLowerString(),
            worldGenerationData: new WorldGenerationDataCollection(),
            playthrough: primaryConfig.Race ? new Playthrough(primaryConfig, Enumerable.Empty<Playthrough.Sphere>()) : playthrough,
            configs: orderedConfigs,
            primaryConfig: primaryConfig
        );

        foreach (var world in worlds.OrderBy(x => x.Id))
        {
            var hints = world.Config.MultiplayerPlayerGenerationData!.Hints;
            var patches = _patcherService.GetPatches(new GetPatchesRequest()
            {
                World = world,
                Worlds = worlds,
                SeedGuid = seedData.Guid,
                Seed = seedNumber,
                Random = new Random(seedNumber % 1000 + world.Id).Sanitize(),
                Hints = hints
            });

            var worldGenerationData = new WorldGenerationData(world, patches, hints);
            seedData.WorldGenerationData.Add(worldGenerationData);
        }

        Debug.WriteLine("Regenerated seed on randomizer instance " + GetHashCode());
        _worldAccessor.World = worlds.Single(x => x.IsLocalWorld);
        _worldAccessor.Worlds = worlds;
        return seedData;
    }

    public bool ValidateSeedSettings(SeedData seedData) => true;
}
