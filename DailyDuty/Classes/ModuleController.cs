﻿using System;
using System.Collections.Generic;
using System.Linq;
using DailyDuty.Interfaces;
using DailyDuty.Modules;
using DailyDuty.Modules.BaseModules;
using KamiLib.Classes;

namespace DailyDuty.Classes;

public class ModuleController : IDisposable {
    public List<Module> Modules { get; }
    private readonly GoldSaucerMessageController goldSaucerMessageController;
    private bool modulesLoaded;

    public ModuleController() {
        Modules = Reflection.ActivateOfType<Module>().ToList();
        goldSaucerMessageController = new GoldSaucerMessageController();
        goldSaucerMessageController.GoldSaucerUpdate += OnGoldSaucerMessage;
    }
    
    public void Dispose() {
        goldSaucerMessageController.GoldSaucerUpdate -= OnGoldSaucerMessage;
        goldSaucerMessageController.Dispose();

        foreach (var module in Modules.OfType<IDisposable>()) {
            module.Dispose();
        }
    }

    public IEnumerable<Module> GetModules(ModuleType? type = null) => 
        type is null ? Modules : Modules.Where(module => module.ModuleType == type);

    public void UpdateModules() {
        if (!modulesLoaded) return; 
        
        foreach (var module in Modules) {
            module.Update();
        }
    }

    public void LoadModules() {
        foreach (var module in Modules) {
            module.Load();
        }

        modulesLoaded = true;
    }
    
    public void UnloadModules() {
        foreach (var module in Modules) {
            module.Unload();
        }

        modulesLoaded = false;
    }

    public void ResetModules() {
        var now = DateTime.UtcNow;
        
        foreach (var module in Modules) {
            if (now >= module.GetNextReset()) {
                module.Reset();
            }
        }
    }

    public void ZoneChange(uint newZone) {
        foreach (var module in Modules) {
            module.ZoneChange();
        }
    }
    
    private void OnGoldSaucerMessage(object? sender, GoldSaucerEventArgs e) {
        foreach (var module in Modules.OfType<IGoldSaucerMessageReceiver>()) {
            module.GoldSaucerUpdate(sender, e);
        }
    }
}