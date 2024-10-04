﻿using Apps.Repositories;
using BudgetTracker.Entities;
using D20Tek.LowDb;

namespace BudgetTracker.Persistence;

internal static class RepositoryFactory
{
    private const string _databaseFile = "budget-data-test.json";
    private const string _appFolder = "\\d20tek-fin";

    private static readonly LowDb<BudgetDataStore> _dataStore =
        LowDbFactory.CreateLowDb<BudgetDataStore>(b =>
            b.UseFileDatabase(_databaseFile)
                .WithFolder(Environment.GetFolderPath(
                    Environment.SpecialFolder.ApplicationData) + _appFolder));

    public static ICategoryRepository CreateCategoryRepository() =>
        new FileRepositoryOld<BudgetCategory, BudgetDataStore>(_dataStore, store => store.Categories);

    public static IExpenseRepository CreateExpenseRepository() =>
        new FileRepositoryOld<Expense, BudgetDataStore>(_dataStore, store => store.Expenses);

    public static IIncomeRepository CreateIncomeRepository() =>
        new FileRepositoryOld<Income, BudgetDataStore>(_dataStore, store => store.Incomes);

    public static IReconciledSnapshotRepository CreateSnapshotRepository() =>
        new FileRepositoryOld<ReconciledSnapshot, BudgetDataStore>(_dataStore, store => store.CompletedSnapshots);
}
