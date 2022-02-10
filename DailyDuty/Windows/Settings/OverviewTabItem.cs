﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DailyDuty.Components.Graphical;
using DailyDuty.Data.Enums;
using DailyDuty.Interfaces;

namespace DailyDuty.Windows.Settings;

internal class OverviewTabItem : ITabItem
{
    public string TabName => "Overview";

    private readonly ITaskCategoryDisplay dailyTasks;
    private readonly ITaskCategoryDisplay weeklyTasks;
    public OverviewTabItem()
    {
        var dailyCompletables = Service.ModuleManager.GetCompletables(CompletionType.Daily);
        var weeklyCompletables = Service.ModuleManager.GetCompletables(CompletionType.Weekly);

        dailyTasks = new FormattedDailyTasks(dailyCompletables);
        weeklyTasks = new FormattedWeeklyTasks(weeklyCompletables);
    }

    public void Draw()
    {
        dailyTasks.Draw();

        weeklyTasks.Draw();
    }

    public void Dispose()
    {
    }
}