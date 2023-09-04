﻿using System;
using System.Collections.Generic;
using System.Linq;
using Randomizer.Data.Configuration.ConfigTypes;
using Randomizer.Data.Options;
using Randomizer.Data.WorldData;
using Randomizer.Data.WorldData.Regions;
using Randomizer.Shared;
using Randomizer.SMZ3.Generation;
using Randomizer.SMZ3.Text;

namespace Randomizer.SMZ3.FileData.Patches;

public class ZeldaTextsPatch : RomPatch
{
    private PatcherServiceData _data = null!;
    private StringTable _stringTable = null!;
    private PlandoTextConfig _plandoText = null!;

    public override IEnumerable<GeneratedPatch> GetChanges(PatcherServiceData data)
    {
        _data = data;
        _stringTable = new StringTable();
        _plandoText = data.PlandoConfig?.Text ?? new PlandoTextConfig();

        var regions = data.LocalWorld.Regions.OfType<IHasReward>().ToList();

        var greenPendantDungeon = regions
            .Where(x => x.RewardType == RewardType.PendantGreen)
            .Select(x => GetRegionName((Region)x))
            .First();

        var redCrystalDungeons = regions
            .Where(x => x.RewardType == RewardType.CrystalRed)
            .Select(x => GetRegionName((Region)x));

        yield return SetText(0x308A00, StringTable.SahasrahlaReveal,
            data.GameLines.SahasrahlaReveal, _plandoText.SahasrahlaReveal,
            greenPendantDungeon);

        yield return SetText(0x308E00, StringTable.BombShopReveal,
            data.GameLines.BombShopReveal, _plandoText.BombShopReveal,
            redCrystalDungeons.First(), redCrystalDungeons.Last());

        yield return SetText(0x308800, StringTable.BlindIntro,
            data.GameLines.BlindIntro, _plandoText.BlindIntro);

        yield return SetText(0x308C00, StringTable.TavernMan,
            data.GameLines.TavernMan, _plandoText.TavernMan);

        foreach (var text in GanonText(data))
        {
            yield return text;
        }

        SetMerchantText();

        var hintText = GetPedestalHint(data, LocationId.MasterSwordPedestal);
        yield return SetText(0x308300, StringTable.MasterSwordPedestal, hintText, _plandoText.MasterSwordPedestal);

        hintText = GetPedestalHint(data, LocationId.EtherTablet);
        yield return SetText(0x308F00, StringTable.EtherTablet, hintText, _plandoText.EtherTablet);

        hintText = GetPedestalHint(data, LocationId.BombosTablet);
        yield return SetText(0x309000, StringTable.BombosTablet, hintText, _plandoText.BombosTablet);

        yield return SetText(0x308400, StringTable.TriforceRoom,
            data.GameLines.TriforceRoom, _plandoText.TriforceRoom);

        SetHintText();

        yield return new GeneratedPatch(Snes(0x1C8000), _stringTable.GetPaddedBytes());
    }

    private GeneratedPatch SetText(int address, string? textKey, SchrodingersString? defaultText, string? overrideText, params object[] formatData)
    {
        return SetText(address, textKey, defaultText?.ToString(), overrideText, formatData);
    }

    private GeneratedPatch SetText(int address, string? textKey, string? defaultText, string? overrideText, params object[] formatData)
    {
        var text = string.IsNullOrEmpty(overrideText) ? defaultText : overrideText;
        if (string.IsNullOrEmpty(text))
            text = "{NOTEXT}";

        var formattedText =
            Dialog.GetGameSafeString(string.Format(text, formatData));

        if (!string.IsNullOrEmpty(textKey))
        {
            _stringTable.SetText(textKey, formattedText);
        }

        if (address < 0)
        {
            return new GeneratedPatch(0, Array.Empty<byte>());
        }

        return new GeneratedPatch(Snes(address), Dialog.Simple(formattedText));
    }

    private void SetChoiceText(string textKey, SchrodingersString? defaultText, string? overrideText, string item)
    {
        var text = string.IsNullOrEmpty(overrideText) ? defaultText?.ToString() : overrideText;
        if (string.IsNullOrEmpty(text))
            text = "{NOTEXT}";

        _stringTable.SetText(textKey,
            Dialog.GetChoiceText(string.Format(text, item),
                _data.GameLines.ChoiceYes?.ToString() ?? "Yes",
                _data.GameLines.ChoiceNo?.ToString() ?? "No"));

    }

    private IEnumerable<GeneratedPatch> GanonText(PatcherServiceData data)
    {
        yield return SetText(0x308600, StringTable.GanonIntro,
            data.GameLines.GanonIntro, _plandoText.GanonIntro);

        // Todo: Verify these two are correct if ganon invincible patch is
        // ever added ganon_fall_in_alt in v30
        var ganonFirstPhaseInvincible = "You think you\nare ready to\nface me?\n\nI will not die\n\nunless you\ncomplete your\ngoals. Dingus!";
        yield return new GeneratedPatch(Snes(0x309100), Dialog.Simple(ganonFirstPhaseInvincible));

        // ganon_phase_3_alt in v30
        var ganonThirdPhaseInvincible = "Got wax in\nyour ears?\nI cannot die!";
        yield return new GeneratedPatch(Snes(0x309200), Dialog.Simple(ganonThirdPhaseInvincible));
        // ---

        var silversLocation = data.Worlds.SelectMany(world => world.Locations)
            .FirstOrDefault(l => l.ItemIs(ItemType.SilverArrows, data.LocalWorld));
        var silversText = silversLocation == null
            ? data.GameLines.GanonSilversHint
            : data.GameLines.GanonNoSilvers;
        var silverLocationPlayer = data.Config.MultiWorld && silversLocation?.World != data.LocalWorld
            ? silversLocation?.World.Player
            : "you";
        yield return SetText(0x308700, StringTable.GanonPhaseThreeText,
            silversText, _plandoText.GanonSilversHint,
            silverLocationPlayer ?? "", silversLocation?.Region.Area ?? "");
    }

    private void SetMerchantText()
    {
        // Have bottle merchant and zora say what they have if requested
        if (_data.Config.CasPatches.PreventScams)
        {
            var item = GetItemName(_data, _data.LocalWorld.LightWorldNorthWest.BottleMerchant.Item);
            SetChoiceText(StringTable.BottleMerchant, _data.GameLines.BottleMerchant,
                _plandoText.BottleMerchant, item);

            item = GetItemName(_data, _data.LocalWorld.FindLocation(LocationId.KingZora).Item);
            SetChoiceText(StringTable.KingZora, _data.GameLines.KingZora,
                _plandoText.KingZora, item);
        }
        else
        {
            if (!string.IsNullOrEmpty(_plandoText.BottleMerchant))
            {
                SetChoiceText(StringTable.BottleMerchant, null, _plandoText.BottleMerchant, "");
            }

            if (!string.IsNullOrEmpty(_plandoText.KingZora))
            {
                SetChoiceText(StringTable.KingZora, null, _plandoText.KingZora, "");
            }
        }
    }

    private void SetHintText()
    {
        // Get the correct number of hints
        var hints = _data.Hints.ToList();
        if (hints.Any() && _data.Config.UniqueHintCount > 0)
        {
            hints = hints.Take(_data.Config.UniqueHintCount).ToList();
            while (hints.Count < GameHintService.HintLocations.Count)
            {
                hints.AddRange(hints.Take(Math.Min(GameHintService.HintLocations.Count - hints.Count,
                    hints.Count)));
            }

            hints.Shuffle(_data.Random);
        }
        else
        {
            hints = Enumerable.Repeat("",GameHintService.HintLocations.Count).ToList();
        }

        SetHintTileText(StringTable.TelepathicTileEasternPalace, hints[0],
            _plandoText.TelepathicTileEasternPalace);
        SetHintTileText(StringTable.TelepathicTileTowerOfHeraFloor4, hints[1],
            _plandoText.TelepathicTileTowerOfHeraFloor4);
        SetHintTileText(StringTable.TelepathicTileSpectacleRock, hints[2],
            _plandoText.TelepathicTileSpectacleRock);
        SetHintTileText(StringTable.TelepathicTileSwampEntrance, hints[3],
            _plandoText.TelepathicTileSwampEntrance);
        SetHintTileText(StringTable.TelepathicTileThievesTownUpstairs, hints[4],
            _plandoText.TelepathicTileThievesTownUpstairs);
        SetHintTileText(StringTable.TelepathicTileMiseryMire, hints[5],
            _plandoText.TelepathicTileMiseryMire);
        SetHintTileText(StringTable.TelepathicTilePalaceOfDarkness, hints[6],
            _plandoText.TelepathicTilePalaceOfDarkness);
        SetHintTileText(StringTable.TelepathicTileDesertBonkTorchRoom, hints[7],
            _plandoText.TelepathicTileDesertBonkTorchRoom);
        SetHintTileText(StringTable.TelepathicTileCastleTower, hints[8],
            _plandoText.TelepathicTileCastleTower);
        SetHintTileText(StringTable.TelepathicTileIceLargeRoom, hints[9],
            _plandoText.TelepathicTileIceLargeRoom);
        SetHintTileText(StringTable.TelepathicTileTurtleRock, hints[10],
            _plandoText.TelepathicTileTurtleRock);
        SetHintTileText(StringTable.TelepathicTileIceEntrance, hints[11],
            _plandoText.TelepathicTileIceEntrance);
        SetHintTileText(StringTable.TelepathicTileIceStalfosKnightsRoom, hints[12],
            _plandoText.TelepathicTileIceStalfosKnightsRoom);
        SetHintTileText(StringTable.TelepathicTileTowerOfHeraEntrance, hints[13],
            _plandoText.TelepathicTileTowerOfHeraEntrance);
        SetHintTileText(StringTable.TelepathicTileSouthEastDarkworldCave, hints[14],
            _plandoText.TelepathicTileSouthEastDarkworldCave);
    }

    private void SetHintTileText(string key, string? defaultText, string? overrideText)
    {
        var text = string.IsNullOrEmpty(overrideText) ? defaultText : overrideText;
        if (string.IsNullOrEmpty(text)) return;
        _stringTable.SetHintText(key, Dialog.GetGameSafeString(text));
    }

    private string GetRegionName(Region region)
    {
        return Dialog.GetGameSafeString(_data.GetRegionInfo(region)?.Name.ToString() ?? region.Name);
    }

    private string GetItemName(PatcherServiceData data, Item item)
    {
        var itemName = _data.GetItemData(item)?.NameWithArticle ?? item.Name;
        if (!data.Config.MultiWorld)
        {
            return itemName;
        }
        else
        {
            return data.LocalWorld == item.World
                ? $"{itemName} belonging to you"
                : $"{itemName} belonging to {item.World.Player}";
        }
    }

    private string GetPedestalHint(PatcherServiceData data, LocationId locationId)
    {
        var item = data.LocalWorld.FindLocation(locationId).Item;
        var hintText = data.GetItemData(item)?.PedestalHints?.ToString() ?? item.Name;
        if (!data.Config.MultiWorld)
        {
            return hintText;
        }
        else
        {
            return data.LocalWorld == item.World
                ? $"{hintText} belonging to you"
                : $"{hintText} belonging to {item.World.Player}";
        }
    }
}
