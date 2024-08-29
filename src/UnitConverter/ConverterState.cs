﻿using D20Tek.Minimal.Functional;
using Spectre.Console;

namespace UnitConverter;

internal sealed record ConverterState(IAnsiConsole Console, string Command = "", bool CanContinue = true)
    : State;