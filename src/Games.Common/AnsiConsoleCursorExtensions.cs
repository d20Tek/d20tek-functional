using Spectre.Console;

namespace Games.Common;

public static class IAnsiConsoleCursorExtensions
{
    public static void ResetPosition(this IAnsiConsoleCursor cursor, int x = 0, int y = 0) =>
        cursor.SetPosition(x, y);
}
