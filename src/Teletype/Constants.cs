using Games.Common;
using Spectre.Console;

namespace Teletype;

internal static class Constants
{
    public const string GameTitle = "Teletype";
    public static TeletypePresenter.Config TeletypeConfig = new(
        () => 20,
        new Style(foreground: Color.DarkGreen),
        Justify.Center);

    public static string[] Introduction =
    [
        "[bold white]FREE NEW PHLAN![/]",
        "The New Phlan city council is leading the fight to free their captive city.",
        "Heroes are retaking the city block by block from evil hordes.",
        string.Empty,
        "RICHES & FAME!",
        "The council is looking for soldiers and rogues, mages and clerics, heroes of",
        "all kinds to come to New Phlan. The wealth and land of an ancient city await",
        "those willing to reach out and take it.",
        string.Empty,
        "GLORY!",
        "Legends will be written about the heroic struggle to free New Phlan! Ships to",
        "New Phlan depart twice monthly. When you arrive, see the New Phlan city",
        "council for the latest news and information.",
        string.Empty,
        "[bold white]MAKE YOUR FORTUNE IN NEW PHLAN![/]",
    ];
}
