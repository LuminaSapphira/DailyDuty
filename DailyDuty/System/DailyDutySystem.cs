﻿using System;
using Dalamud.Game;

namespace DailyDuty.System;

public class DailyDutySystem : IDisposable
{
    public readonly ModuleController ModuleController;
    private readonly AddonController addonController;
    public readonly TodoController TodoController;
    public readonly FontController FontController;
    
    public DailyDutySystem()
    {
        LocalizationController.Instance.Initialize();
        PayloadController.Instance.Initialize();
        FontController = new FontController();
        ModuleController = new ModuleController();
        addonController = new AddonController();
        TodoController = new TodoController();
        
        if (Service.ClientState.IsLoggedIn)
        {
            OnLogin(this, EventArgs.Empty);
        }
        
        Service.Framework.Update += OnFrameworkUpdate;
        Service.ClientState.Login += OnLogin;
        Service.ClientState.Logout += OnLogout;
        Service.ClientState.TerritoryChanged += OnZoneChange;
        AddonController.AddonPreSetup += OnAddonPreSetup;
        AddonController.AddonPostSetup += OnAddonPostSetup;
        AddonController.AddonFinalize += OnAddonFinalize;
    }

    public void Dispose()
    {
        Service.Framework.Update -= OnFrameworkUpdate;
        Service.ClientState.Login -= OnLogin;
        Service.ClientState.Logout -= OnLogout;
        Service.ClientState.TerritoryChanged -= OnZoneChange;
        AddonController.AddonPreSetup -= OnAddonPreSetup;
        AddonController.AddonPostSetup -= OnAddonPostSetup;
        AddonController.AddonFinalize -= OnAddonFinalize;

        FontController.Dispose();
        ModuleController.Dispose();
        addonController.Dispose();
        TodoController.Dispose();
        LocalizationController.Cleanup();
        PayloadController.Cleanup();
    }

    private void OnFrameworkUpdate(Framework framework)
    {
        if (Service.ClientState.IsPvP) return;
        if (!Service.ClientState.IsLoggedIn) return;

        // Check for reset, and reset modules that need it 
        ModuleController.ResetModules();
        
        // Update All Modules
        ModuleController.UpdateModules();
        
        // Update TodoDisplay
        TodoController.Update();
    }
    
    private void OnLogin(object? sender, EventArgs e)
    {
        ModuleController.LoadModules();
        
        TodoController.Load();
    }
    
    private void OnLogout(object? sender, EventArgs e)
    {
        ModuleController.UnloadModules();
        
        TodoController.Unload();
    }
    
    private void OnZoneChange(object? sender, ushort territoryTypeId)
    {
        if (Service.ClientState.IsPvP) return;
        if (!Service.ClientState.IsLoggedIn) return;
        
        ModuleController.ZoneChange(territoryTypeId);
    }
    
    private void OnAddonPreSetup(AddonArgs addonInfo)
    {
        if (Service.ClientState.IsPvP) return;
        
        ModuleController.AddonPreSetup(addonInfo);
    }
    
    private void OnAddonPostSetup(AddonArgs addonInfo)
    {
        if (Service.ClientState.IsPvP) return;
        
        ModuleController.AddonPostSetup(addonInfo);
    }
    
    private void OnAddonFinalize(AddonArgs addonInfo)
    {
        if (Service.ClientState.IsPvP) return;
        
        ModuleController.AddonFinalize(addonInfo);
    }
}