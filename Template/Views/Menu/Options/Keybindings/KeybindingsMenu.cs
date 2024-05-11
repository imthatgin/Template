using System;
using System.Linq;
using Godot;
using Template.Core;
using Template.Core.Config;
using Template.Core.Dependencies;

namespace Template.Views.Menu.Options.Keybindings;

public partial class KeybindingsMenu : MarginContainer
{
    [ExportCategory("Dependencies")]
    [Export, Require] public PackedScene InputButton { get; set; } = null!;
    [Export, Require] public VBoxContainer KeyInputScrollContainer { get; set; } = null!;
    [Export, Require] public Button ResetButton { get; set; } = null!;

    private StringName? RemappingAction { get; set; }
    private Button? RemappingButton { get; set; }

    private const string PressAnyKey = "Press any key...";

    public override void _Ready()
    {
        InputMapping.LoadInputMap();
        BuildInputList();

        Debugging.Watch(this, "RemappingAction");

        ResetButton.Pressed += ResetToDefault;
    }

    private void BuildInputList()
    {
        // Clear children
        foreach (var child in KeyInputScrollContainer.GetChildren())
            child.QueueFree();

        // Build new list
        foreach (var action in InputMap.GetActions())
        {
            // Skip built in actions in this list, because they control basic UI functions
            if (action.ToString().StartsWith("ui_")) continue;

            var button = InputButton?.Instantiate() as Button;
            var nameLabel = button?.FindChild("NameLabel") as Label;
            var keyLabel = button?.FindChild("KeyLabel") as Label;

            if (nameLabel is null || keyLabel is null || button is null)
                throw new InvalidOperationException();

            var keyText = InputMap.ActionGetEvents(action)
                .Select(a => a.AsText().TrimSuffix(" (Physical)"))
                .ToList();

            Variant actionVariant = Variant.CreateFrom(action.ToString());
            var isDefined = GameConfig.Config.KeybindNames.ContainsKey(actionVariant);
            nameLabel.Text = isDefined ? GameConfig.Config.KeybindNames[actionVariant].ToString() : action;
            keyLabel.Text = string.Join(", ", keyText);

            button.Pressed += () => StartRebinding(button, action);
            KeyInputScrollContainer.AddChild(button);
        }
    }

    private void StartRebinding(Button button, StringName action)
    {
        if (RemappingAction is not null)
            return;

        var keyLabel = button.FindChild("KeyLabel") as Label;
        if (keyLabel is null)
            return;

        keyLabel.Text = PressAnyKey;
        RemappingAction = action;
        RemappingButton = button;
    }

    public override void _Input(InputEvent @event)
    {
        if (RemappingAction is null)
            return;

        if ((@event is not InputEventKey && @event is not InputEventMouseButton) || !@event.IsPressed())
            return;

        // Swap the input for the action
        InputMapping.ChangeMapping(RemappingAction, @event);

        // Save to disk
        InputMapping.SaveInputMap();

        RemappingAction = null;
        RemappingButton = null;

        BuildInputList();
    }

    private void ResetToDefault()
    {
        InputMap.LoadFromProjectSettings();
        InputMapping.SaveInputMap();

        BuildInputList();
    }
}