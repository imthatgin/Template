using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using Godot;
using Template.Core.Converters;

namespace Template.Core.UserOptions;

public record UserConfig
{
    [JsonConverter(typeof(Vector2IConverter))]
    public Vector2I Resolution { get; set; } = new (1920, 1080);
    public Window.ModeEnum WindowMode { get; set; } = Window.ModeEnum.Windowed;
}

public static class UserOptions
{
    private const string UserOptionsPath = "user://UserOptions.json";
    public static UserConfig UserConfig { get; set; } = new();

    public static void Save()
    {
        using var fileHandle = FileAccess.Open(UserOptionsPath, FileAccess.ModeFlags.Write);
        var json = JsonSerializer.Serialize(UserConfig);
        
        fileHandle.StoreString(json);
    }
    
    public static void Load(Node node)
    {
        if (!FileAccess.FileExists(UserOptionsPath))
        {
            UserConfig = new UserConfig
            {
                Resolution = DisplayServer.WindowGetSize(),
                WindowMode = (Window.ModeEnum)DisplayServer.WindowGetMode()
            };
            Save();
            return;
        }
        
        using var fileHandle = FileAccess.Open(UserOptionsPath, FileAccess.ModeFlags.Read);
        var deserialized = JsonSerializer.Deserialize<UserConfig>(fileHandle.GetAsText())!;
        UserConfig = deserialized;
    }
}