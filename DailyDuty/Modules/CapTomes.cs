using System;
using DailyDuty.Classes;
using DailyDuty.Modules.BaseModules;
using DailyDuty.Localization;
using DailyDuty.Models;
using FFXIVClientStructs.FFXIV.Client.Game;

namespace DailyDuty.Modules;

public class CapTomesData : ModuleData
{
    public int ExpertTomestones;
    public int ExpertTomestoneCap;
    public bool AtTomeCap;

    protected override void DrawModuleData() {
        DrawDataTable([
            (Strings.CurrentWeeklyTomestones, ExpertTomestones.ToString()),
            (Strings.WeeklyTomestoneLimit, ExpertTomestoneCap.ToString()),
            (Strings.AtWeeklyTomestoneLimit, AtTomeCap.ToString()),
        ]);
    }
}

public class CapTomesConfig : ModuleConfig;

public unsafe class CapTomes : Modules.Weekly<CapTomesData, CapTomesConfig>
{
    public override ModuleName ModuleName => ModuleName.CapTomes;

    public override void Update()
    {
        Data.ExpertTomestones = TryUpdateData(Data.ExpertTomestones, InventoryManager.Instance()->GetWeeklyAcquiredTomestoneCount());
        Data.ExpertTomestoneCap = TryUpdateData(Data.ExpertTomestoneCap, InventoryManager.GetLimitedTomestoneWeeklyLimit());
        Data.AtTomeCap = TryUpdateData(Data.AtTomeCap, Data.ExpertTomestones == Data.ExpertTomestoneCap);
        base.Update();
    }

    protected override ModuleStatus GetModuleStatus() => Data.AtTomeCap ? ModuleStatus.Complete : ModuleStatus.Incomplete;

    protected override StatusMessage GetStatusMessage() => new()
    {
        Message = $"{Data.ExpertTomestones} / {Data.ExpertTomestoneCap} {Strings.CurrentWeeklyTomestones}"
    };
}