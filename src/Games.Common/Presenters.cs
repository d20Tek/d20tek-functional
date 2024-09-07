using Spectre.Console;

namespace Games.Common;

public static class Presenters
{
    public static FigletText GameHeader(string title) =>
        new FigletText(title).Color(Color.Green).Centered();

    public static FigletText AppHeader(string title) =>
        new FigletText(title).Color(Color.Green).Centered();
}
