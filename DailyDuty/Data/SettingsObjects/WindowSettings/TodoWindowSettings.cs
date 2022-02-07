﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using DailyDuty.Utilities;

namespace DailyDuty.Data.SettingsObjects.WindowSettings;

public class TodoWindowSettings : GenericWindowSettings
{
    public bool ShowDaily = true;
    public bool ShowWeekly = true;
    public bool HideWhenTasksComplete = false;

    public Vector4 HeaderColor = Colors.White;
    public Vector4 IncompleteColor = Colors.Red;
    public Vector4 CompleteColor = Colors.Green;
}