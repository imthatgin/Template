using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Godot;

namespace Template.Core;

public class WatcherContainer
{
    public Node Node { get; set; }
    public string Key { get; set; }
    public string? Value { get; set; } = null;

    public WatcherContainer(Node node, string key)
    {
        Node = node;
        Key = key;
    }
}

public partial class Debugging : Node
{
    public static List<WatcherContainer> Watchers = new();

    private PackedScene? DebugViewScene { get; set; }

    public override void _Ready()
    {
        DebugViewScene = GD.Load<PackedScene>("res://Views/Debug/DebugView.tscn");
    }

    public static void Watch(Node node, string key)
    {
        var container = new WatcherContainer(node, key);
        Watchers.Add(container);
        
        // Ensures it clears out if the object disappears.
        node.TreeExiting += () =>
        {
            var watcher = Watchers.FirstOrDefault(w => w.Node == node && w.Key == key);
            if (watcher is not null) Watchers.Remove(watcher);
        };
    }

    public override void _Process(double delta)
    {
        Watchers.ForEach(watcher =>
            watcher.Value = GetMemberValue(watcher.Node, watcher.Key)?.ToString());
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey && @event.IsPressed())
        {
            if (@event.IsActionPressed("toggle_debug_view"))
            {
                ToggleDebugView();
            }
        }
    }

    private static object? GetMemberValue(object obj, string member)
    {
        Type type = obj.GetType();
        PropertyInfo? propertyInfo = type.GetProperty(member, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

        if (propertyInfo is not null)
        {
            return propertyInfo.GetValue(obj);
        }

        return $"<'{member}' not found>";
    }

    private void ToggleDebugView()
    {
        var debugView = GetTree().Root.GetNodeOrNull<Views.Debug.DebugView>("DebugView");
        if (debugView is not null)
        {
            GetTree().Root.RemoveChild(debugView);
            return;
        }

        if (DebugViewScene is null)
            return;

        var debugSceneInstance = DebugViewScene.Instantiate();
        GetTree().Root.AddChild(debugSceneInstance);
    }
}