using D20Tek.Functional;

namespace Games.Common;

public static class ConsoleSizer
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "Analyzer doesn't work with functional programming style code.")]
    public static void EnsureMinimumSize(int width, int height) =>
        OperatingSystem.IsWindows().Fork(
            x => (Console.WindowWidth < width && OperatingSystem.IsWindows())
                    ? Console.WindowWidth = width + 1
                    : Console.WindowWidth,
            x => (Console.WindowHeight < height && OperatingSystem.IsWindows())
                    ? Console.WindowHeight = height + 1
                    : Console.WindowHeight,
            (windowWidth, windowHeight) =>
            {
                Console.BufferWidth = Console.WindowWidth = windowWidth;
                Console.BufferHeight = Console.WindowHeight = windowHeight;
            });

    public static bool IsValidSize(int width, int height) =>
        Console.WindowHeight >= height && Console.WindowWidth >= width;
}
