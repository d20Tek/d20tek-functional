using Spectre.Console;
using Spectre.Console.Rendering;

namespace Games.Common;

public static class TeletypePresenter
{
    public sealed record Config(Func<int>? DelayFunc = null, Style? Style = null, Justify? Alignment = null);

    private static Func<int> DefaultDelay => () => Random.Shared.Next(100, 300);

    public static void TeletypeMarkup(this IAnsiConsole console, string[] text, Config? config = null) =>
        console.TeletypeMarkup(string.Join(Environment.NewLine, text), config);

    public static void TeletypeMarkup(this IAnsiConsole console, string text, Config? config = null) =>
        SplitSegments(console, new Markup(text, config?.Style).Justify(config?.Alignment))
            .Select(segment => new SegmentRenderable(segment))
            .ToList()
            .ForEach(renderable =>
            {
                console.Write(renderable);
                Thread.Sleep((config?.DelayFunc ?? DefaultDelay)());
            });

    private static IEnumerable<Segment> SplitSegments(IAnsiConsole console, IRenderable renderable) =>
        renderable.GetSegments(console)
                  .SelectMany(segment =>
                      segment is { IsLineBreak: false, Text.Length: > 1 }
                      ? segment.Text.Select(c => new Segment(c.ToString(), segment.Style))
                      : [segment]);

    private sealed class SegmentRenderable(Segment segment) : Renderable
    {
        private readonly Segment _segment = segment ?? throw new ArgumentNullException(nameof(segment));

        protected override IEnumerable<Segment> Render(RenderOptions options, int maxWidth) => [_segment];
    }
}
