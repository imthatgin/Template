using System.Linq;
using Godot;
using Godot.Collections;

namespace Template.Core;

public static class InputMapping
{
    private const string InputMapPath = "user://UserInputMap.dat";

    public delegate void InputMappingChanged(StringName action, InputEvent newEvent);
    public static event InputMappingChanged? OnChanged;
    
    public static void SaveInputMap()
    {
        var map = InputMap.GetActions()
            .ToDictionary(a => a, InputMap.ActionGetEvents);
        var godotMap = new Dictionary<StringName, Array<InputEvent>>(map);

        using var fileHandle = FileAccess.Open(InputMapPath, FileAccess.ModeFlags.Write);
        fileHandle.StoreVar(godotMap, true);
        Logger.Info($"Stored {godotMap.Count} input map entries");
    }

    public static void ChangeMapping(StringName action, InputEvent @event)
    {
        InputMap.ActionEraseEvents(action);
        InputMap.ActionAddEvent(action, @event);
        
        OnChanged?.Invoke(action, @event);
    }

    public static void LoadInputMap()
    {
        if (!FileAccess.FileExists(InputMapPath))
            return;
		
        if (InputMap.GetActions().Count == 0)
        {
            InputMap.LoadFromProjectSettings();
            return;
        }
		
        // Clears our map
        InputMap.GetActions()
            .ToList()
            .ForEach(InputMap.EraseAction);
		
        using var fileHandle = FileAccess.Open(InputMapPath, FileAccess.ModeFlags.Read);
        var inputs = (Dictionary<StringName, Array<InputEvent>>)fileHandle.GetVar(true);
        Logger.Info($"Loaded {inputs.Count} saved keybindings");

        foreach (var inputPair in inputs)
        {
            InputMap.AddAction(inputPair.Key);
            foreach (var @event in inputPair.Value)
                InputMap.ActionAddEvent(inputPair.Key, @event);
        }
    }
}