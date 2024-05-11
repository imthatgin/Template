using Godot;
using Godot.Collections;

namespace Template.Core.Config;

[GlobalClass]
public partial class GameSettings : Resource
{
    [ExportCategory("Basic options")]
    [Export] public string GameTitle { get; set; } = "AwesomeGame";
    [Export] public string GameDescription { get; set; } = "I swear this game is good";

    [ExportCategory("Options Menu & Input Settings")]
    [Export] public Dictionary KeybindNames = new()
    {
        { "forward", "Move forwards" },
        { "backward", "Move backwards" },
        { "right", "Move right" },
        { "left", "Move left" },
        { "toggle_debug_view", "Toggle Debugging View" }
    };

    [ExportCategory("Steam Integration")]
    [Export] public bool EnableSteamIntegration { get; set; } = false;
    [Export] public uint SteamAppId { get; set; } = 0;
}