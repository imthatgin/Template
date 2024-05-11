using Godot;

namespace Template.Core.Config;

/// <summary>
/// Used to access the loaded game configuration for the project.
/// </summary>
public static class GameConfig
{
    private static GameSettings? _loadedConfig;
    
    /// <summary>
    /// Returns the loaded configuration for the project.
    /// </summary>
    public static GameSettings Config =>
        _loadedConfig ??= ResourceLoader.Load<GameSettings>("res://Config/GameConfig.tres");
}