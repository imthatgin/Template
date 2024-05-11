using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Godot.Collections;
using Template.Core;
using Template.Core.Dependencies;
using Template.Core.UserOptions;

namespace Template.Views.Menu.Options.GraphicsOptions;

public partial class GraphicsOptionsMenu : MarginContainer
{
	[ExportCategory("Dependencies")]
	[Export, Require] public OptionButton ResolutionOptionButton { get; set; } = null!;
	[Export, Require] public OptionButton WindowTypeOptionButton { get; set; } = null!;
	[Export, Require] public Button ApplyButton { get; set; } = null!;
	[Export, Require] public Button ResetButton { get; set; } = null!;

	private UserConfig LocalUserConfig { get; set; } = UserOptions.UserConfig!;
	
	private static readonly Array<Vector2I> AvailableResolutions = new()
	{
		new Vector2I(1366, 768),
		new Vector2I(1920, 1080),
		new Vector2I(2560, 1440),
		new Vector2I(2560, 1600),
		new Vector2I(3440, 1440),
		new Vector2I(3840, 2160)
	};

	public override void _Ready()
	{
		UserOptions.Load(this);
		ApplyConfig();
		
		BuildOptions();
		PreselectOptions();
		

		ResetButton.Pressed += () =>
		{
			UserOptions.UserConfig = LocalUserConfig;
			UserOptions.Save();
			
			ApplyConfig();
		};
		
		ApplyButton.Pressed += () =>
		{
			UserOptions.UserConfig = LocalUserConfig;
			UserOptions.Save();
			
			ApplyConfig();
		};

		ResolutionOptionButton.ItemSelected += index =>
		{
			var res = AvailableResolutions.ElementAt((int)index);
			LocalUserConfig.Resolution = res;
		};

		WindowTypeOptionButton.ItemSelected += index => LocalUserConfig.WindowMode = (Window.ModeEnum)index;
	}

	private void ApplyConfig()
	{
		DisplayServer.WindowSetSize(UserOptions.UserConfig.Resolution);
		DisplayServer.WindowSetMode((DisplayServer.WindowMode)UserOptions.UserConfig.WindowMode);
	}

	private void PreselectOptions()
	{
		var currentResolution = DisplayServer.WindowGetSize();

		if(!AvailableResolutions.Contains(currentResolution))
			AvailableResolutions.Add(currentResolution);

		var currentResIndex = AvailableResolutions.IndexOf(currentResolution);
		
		BuildOptions();
		
		WindowTypeOptionButton.Select((int)DisplayServer.WindowGetMode());
		ResolutionOptionButton.Select(currentResIndex);
	}

	private void BuildOptions()
	{
		ResolutionOptionButton?.Clear();
		foreach (var res in AvailableResolutions)
			ResolutionOptionButton?.AddItem($"{res.X}x{res.Y}");

		WindowTypeOptionButton.Clear();
		var windowModes = (Window.ModeEnum[])Enum.GetValues(typeof(Window.ModeEnum));
		foreach (var mode in windowModes)
		{
			//if(mode == Window.ModeEnum.Maximized || mode == Window.ModeEnum.Minimized) continue;
			WindowTypeOptionButton.AddItem(mode.ToString());
		}
	}
}