namespace TreasureHunt;

internal static partial class Constants
{
    public static class Commands
    {
        public static string[] CannotMoveMessage = [string.Empty, "[yellow]YOU CAN'T MOVE THAT DIRECTION.[/]"];
        public static string[] OkMessage = [string.Empty, "OK."];
        public static string[] CommandError = [string.Empty, "[red]ERROR RUNNING COMMAND.[/]"];

        public static string[] CannotCarryMoreMessage = [string.Empty, "[yellow]YOU CAN'T CARRY ANY MORE.[/]"];
        public static string[] EmptyRoomMessage = [string.Empty, "[yellow]THIS ROOM IS EMPTY.[/]"];
        public static string[] TreasureGrabbedMessage(string treasure) =>
            [string.Empty, $"OK. YOU'RE CARRYING THE {treasure}."];

        public const string RoomContentsLabel = "CONTENTS OF ROOMS:";
        public static string TreasureCarriedLabel(string treasure) => $"YOU ARE CARRYING : {treasure}";
        public const string TreasureNothing = "NOTHING";

        public static string[] PutNotCarryingMessage = [string.Empty, "[yellow]YOU'RE NOT CARRYING ANYTHING.[/]"];
        public static string[] TreasurePlacedMessage(string treasure, int room) =>
            [string.Empty, $"{treasure} PLACED IN ROOM {room}."];
    }
}
