namespace MatrixDrop;

struct MatrixCell
{
    public char Char;
    public string Color;

    public override readonly string ToString() => $"[{Color}]{Char}[/]";
}