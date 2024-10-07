namespace ChutesAndLadders;

internal record Player(int Position, int Number, string Color)
{
    public bool IsWinner() => Position >= Constants.WinningPosition;
}