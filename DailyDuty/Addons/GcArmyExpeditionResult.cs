﻿using System;
using System.Linq;
using DailyDuty.System;
using Dalamud.Hooking;
using Dalamud.Utility.Signatures;
using FFXIVClientStructs.FFXIV.Component.GUI;
using KamiLib.Caching;
using KamiLib.ExceptionSafety;
using KamiLib.Utilities;
using Lumina.Excel.GeneratedSheets;

namespace DailyDuty.Addons;

public record ExpeditionResultArgs(uint MissionType, bool Successful);

public unsafe class GcArmyExpeditionResult : IDisposable
{
    private static GcArmyExpeditionResult? _instance;
    public static GcArmyExpeditionResult Instance => _instance ??= new GcArmyExpeditionResult();
    
    private delegate IntPtr AddonOnSetup(AtkUnitBase* addon, uint valueCount, AtkValue* values);
    [Signature("48 89 5C 24 ?? 55 56 57 41 56 41 57 48 83 EC 30 44 8B FA", DetourName = nameof(OnSetup))]
    private readonly Hook<AddonOnSetup>? onSetupHook = null!;

    public event EventHandler<ExpeditionResultArgs>? Setup;

    private GcArmyExpeditionResult()
    {
        SignatureHelper.Initialise(this);
        
        AddonManager.AddAddon(this);

        onSetupHook.Enable();
    }

    public void Dispose()
    {
        onSetupHook?.Dispose();
    }

    private IntPtr OnSetup(AtkUnitBase* addon, uint valueCount, AtkValue* values)
    {
        var result = onSetupHook!.Original(addon, valueCount, values);

        Safety.ExecuteSafe(() =>
        {
            var dutyName = values[4].GetString();

            var duty = LuminaCache<GcArmyExpedition>.Instance
                .Where(row => row.Name.RawString == dutyName)
                .FirstOrDefault();

            if (duty != null)
            {
                Setup?.Invoke(this, new ExpeditionResultArgs(duty.GcArmyExpeditionType.Row, values[2].Int == 1));
            }
        });
        
        return result;
    }
}