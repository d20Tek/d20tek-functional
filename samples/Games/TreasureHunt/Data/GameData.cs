﻿namespace TreasureHunt.Data;

internal static class GameData
{
    private static readonly Room[] _rooms =
    [
        new (1, 2, 7, 6, Constants.DirectionNotAllowed, "COLD AND CREEPY"),
        new (2, Constants.DirectionNotAllowed, 3, 7, 1, "DARK AND DINGY"),
        new (3, Constants.DirectionNotAllowed, Constants.DirectionNotAllowed, 4, 2, "GREY AND GHOSTLY"),
        new (4, 3, Constants.DirectionNotAllowed, 5, 7, "FOUL AND FOGGY"),
        new (5, 7, 4, Constants.DirectionNotAllowed, 6, "EMPTY AND EERIE"),
        new (6, 1, 5, Constants.DirectionNotAllowed, Constants.DirectionNotAllowed, "HAUNTED AND HORRIBLE"),
        new (7, 2, 4, 5, 1, "SPOOKY AND SCARY")
    ];

    private static readonly Treasure[] _treasures =
    [
        new (1, "GOLD", 1),
        new (2, "CHEWING GUM", 2),
        new (3, "SANDWICHES", 3),
        new (4, "RUBBISH", 4),
        new (5, "POTS OF HONEY", 5),
        new (6, "JEWELS", 6),
        new (7, "COINS", 7)
    ];

    public static Room GetRoomById(int id) => _rooms.First(x => x.Id == id);

    public static Location[] GetTreasureLocations() =>
        _treasures.Select(t => new Location(t.Id, t.StartingRoom)).ToArray();

    public static Treasure GetTreasureById(int id) => _treasures.First(x => x.Id == id);
}
