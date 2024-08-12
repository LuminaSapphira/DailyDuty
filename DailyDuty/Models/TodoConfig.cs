﻿using System.Drawing;
using System.Numerics;
using DailyDuty.Modules;
using Dalamud.Interface;
using KamiLib.Configuration;
using KamiToolKit.Nodes;

namespace DailyDuty.Models;

public class CategoryConfig {
    public ModuleType ModuleType;
    public LayoutAnchor LayoutAnchor = LayoutAnchor.TopLeft;
    public bool Enabled = true;
    
    public bool ShowHeader = true;
    
    public bool HeaderItalic = false;
    public bool ModuleItalic = false;
    public bool Edge = true;
    public bool Glare = false;

    public string HeaderLabel = "ERROR Initializing Data";
    
    public uint ModuleFontSize = 12;
    public uint HeaderFontSize = 24;

    public bool UseCustomLabel = false;
    public string CustomLabel = string.Empty;

    public Vector4 CategoryMargin = new(5.0f);
    public Vector4 ModuleMargin = new(1.0f);
    
    public Vector4 HeaderTextColor = KnownColor.White.Vector();
    public Vector4 HeaderTextOutline = DefaultColors.DefaultHeaderOutlineColor;
    public Vector4 ModuleTextColor = KnownColor.White.Vector();
    public Vector4 ModuleOutlineColor = DefaultColors.DefaultModuleOutlineColor;

    public static class DefaultColors {
        public static Vector4 DefaultHeaderOutlineColor = new Vector4(142, 106, 12, 255) / 255;
        public static Vector4 DefaultModuleOutlineColor = new Vector4(10, 105, 146, 255) / 255;
    }
}

public class TodoConfig {
    public bool Enabled = false;

    public CategoryConfig[] CategoryConfigs = [
        new CategoryConfig {
            ModuleType = ModuleType.Daily,
            HeaderLabel = "Daily Tasks",
        },
        
        new CategoryConfig {
            ModuleType = ModuleType.Weekly,
            HeaderLabel = "Weekly Tasks",
        },
        
        new CategoryConfig {
            ModuleType = ModuleType.Special,
            HeaderLabel = "Special Tasks",
        },
    ];

    public LayoutAnchor Anchor = LayoutAnchor.TopLeft;
    public Vector2 Position = new Vector2(1024, 720) / 2.0f;
    public Vector2 Size = new(600.0f, 400.0f);
    public bool SingleLine = true;
    public bool ShowListBackground = true;
    public bool ShowListBorder;
    public bool FitBackground = true;
    
    public bool HideDuringQuests = true;
    public bool HideInDuties = true;

    public Vector4 ListBackgroundColor = KnownColor.Aqua.Vector() with { W = 0.40f };
    public float Scale = 1.0f;

    public static TodoConfig Load() 
        => Service.PluginInterface.LoadCharacterFile(Service.ClientState.LocalContentId, "TodoList.config.json", () => new TodoConfig());

    public void Save()
        => Service.PluginInterface.SaveCharacterFile(Service.ClientState.LocalContentId, "TodoList.config.json", this);
}