﻿using D20Tek.Functional;

namespace Zorgos;

internal sealed record GameState(int Zchips, int CurrentBet, string[] LatestMove, RollResult Result) : IState
{
    public bool IsGameComplete() => Zchips <= 0 || Zchips >= Constants.WinningTotal;

    public bool IsGameLoss() => Zchips <= 0;
}
