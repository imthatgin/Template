using Godot;
using Steamworks;
using Template.Core.Config;

namespace Template.Core;

public partial class Steamworks : Node
{
    public override void _Ready()
    {
        if (!GameConfig.Config.EnableSteamIntegration || GameConfig.Config.SteamAppId == 0) return;
        
        SteamClient.Init(GameConfig.Config.SteamAppId);
		
        var ticket = SteamUser.GetAuthSessionTicket().Data;
        var result = SteamUser.BeginAuthSession(ticket, (ulong)SteamClient.SteamId);

        if (result == BeginAuthResult.OK) return;
        Logger.Info(this, $"Quitting because auth result is {result}");
        GetTree().Quit();
    }

    public override void _Process(double delta) => SteamClient.RunCallbacks();

    protected override void Dispose(bool disposing)
    {
        if (!GameConfig.Config.EnableSteamIntegration) return;
		
        SteamClient.Shutdown();
    }
}