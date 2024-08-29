﻿namespace MartianTrail.Inventory;

internal sealed record InventorySelectionState(
    int Batteries = 0,
    int Food = 0,
    int Furs = 0,
    int LaserCharges = 0,
    int AtmosphereSuits = 0,
    int MediPacks = 0,
    int Credits = 0,
    bool PlayerIsHappyWithSelection = false)
    : InventoryState(Batteries, Food, Furs, LaserCharges, AtmosphereSuits, MediPacks, Credits)
{
    public static InventorySelectionState From(InventoryState inv) =>
        new(inv.Batteries, inv.Food, inv.Furs, inv.LaserCharges, inv.AtmosphereSuits, inv.MediPacks, inv.Credits);
}