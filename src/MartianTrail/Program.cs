using MartianTrail;
using MartianTrail.Common;
using Spectre.Console;

var webApiClient = new WebApiClient(new HttpClient());
Game.Play(AnsiConsole.Console, webApiClient, RandomGenerator.Roll);