﻿using Apps.Repositories;
using BudgetTracker.Entities;
using D20Tek.LowDb;

namespace BudgetTracker.Persistence;

internal static class RepositoryFactory
{
    private const string _databaseFile = "budget-data.json";
    private const string _appFolder = "\\d20tek-fin";

    private static readonly LowDb<BudgetDataStore> _dataStore =
        LowDbFactory.CreateLowDb<BudgetDataStore>(b =>
            b.UseFileDatabase(_databaseFile)
                .WithFolder(Environment.GetFolderPath(
                    Environment.SpecialFolder.ApplicationData) + _appFolder));

    public static ICategoryRepository CreateCategoryRepository() =>
        new FileRepository<BudgetCategory, BudgetDataStore>(_dataStore, store => store.Categories);
}
