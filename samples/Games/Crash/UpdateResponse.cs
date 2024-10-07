namespace Crash;

internal sealed record UpdateResponse(char[,] Scene, int RoadUpdate, int NewCarPosition);