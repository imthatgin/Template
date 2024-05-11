using Godot;
using Template.Core;
using Template.Core.Config;
using Template.Core.Dependencies;

namespace Template.Views.Menu;

public partial class MenuScene : Control
{
	[ExportCategory("Dependencies")]
	[Export, Require] public Label TitleLabel { get; set; } = null!;
	[Export, Require] public Label DescriptionLabel { get; set; } = null!;
	[Export, Require] public Button OptionsButton { get; set; } = null!;
	[Export, Require] public Button ExitButton { get; set; } = null!;
	[Export, Require] public Button PlayButton { get; set; } = null!;
	[Export, Require] public Options.OptionsMenu OptionsMenu { get; set; } = null!;
	[Export, Require] public PackedScene GameScene { get; set; } = null!;

	public override void _Ready()
	{
		TitleLabel.Text = GameConfig.Config.GameTitle;
		DescriptionLabel.Text = GameConfig.Config.GameDescription;
		OptionsButton.Pressed += () => OptionsMenu.Visible = !OptionsMenu.Visible;
		ExitButton.Pressed += () => GetTree().Quit();
		PlayButton.Pressed += () => GetTree().ChangeSceneToPacked(GameScene);
	}
}
